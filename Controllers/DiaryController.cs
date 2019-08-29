using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Diet.Models;
using Diet.Contexts;
using Diet.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Diet.Controllers
{
    [Authorize]
    public class DiaryController : Controller
    {
        private readonly MainContext context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DiaryController(MainContext dietContext, SignInManager<ApplicationUser> signInManager)
        {
            context = dietContext;
            _signInManager = signInManager;
        }

        public ActionResult Index(string q, int? FoodId, DateTime? Date)
        {
            if (!Date.HasValue)
            {
                return RedirectToRoute("diary", new { controller = "Diary", action = "Index", Date = DateTime.Now.ToString("yyyy-MM-dd"), q = q, FoodId = FoodId });
            }
            
            var user = context.Users
                .Include(u => u.Entries)
                .Include(u => u.WeightEntries)
                .Include(u => u.Gender)
                .FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            var weight = context.WeightEntries.FirstOrDefault(w => w.Date == Date && w.UserId == user.Id)?.Weight;

            var entries = context.Entries
                .Include(e => e.Food)
                    .ThenInclude(f => f.Meassures)
                .Where(e => e.UserId == user.Id && e.Date.Date == Date.Value.Date)
                .OrderBy(e => e.Date).ToList();

            var recentEntries = context.Entries
                .Include(e => e.Food)
                    .ThenInclude(f => f.Meassures)
                .Where(e => e.UserId == user.Id)
                .OrderByDescending(e => e.Date).Take(3).ToList();


            ICollection<List<Entry>> groupedEntries = entries
                .GroupBy(e =>
                {
                    var updated = e.Date.AddMinutes(30);
                    return new DateTime(updated.Year, updated.Month, updated.Day,
                                        updated.Hour, 0, 0, e.Date.Kind);
                }).Select(cl => cl.ToList()).ToList();

            var foods = Enumerable.Empty<Food>().AsQueryable();
            int? amount = null;

            var search = q;
            IEnumerable<string> searchTerms = Enumerable.Empty<string>();

            if (!String.IsNullOrEmpty(q) && q.Length > 2)
            {
                var amountMatches = Regex.Matches(q, @"[0-9]+g"); //TODO Many units
                if (amountMatches.Any())
                {
                    string amountString = amountMatches[0].Value.Substring(0, amountMatches[0].Value.Length - 1);
                    int amt;
                    Int32.TryParse(amountString, out amt);
                    amount = amt;
                    search = q.Replace(amountMatches[0].Value, "");
                }

                //TODO: American time
                var timeMatches = Regex.Matches(q, @"([01]?[0-9]|2[0-3]):[0-5][0-9]");
                if (timeMatches.Any())
                {
                    string timeString = timeMatches[0].Value.Substring(0, timeMatches[0].Value.Length);
                    var time = DateTime.ParseExact(timeString, "HH:mm",
                                        CultureInfo.InvariantCulture);

                    Date = Date.Value.AddHours(time.Hour).AddMinutes(time.Minute);

                    search = search.Replace(timeMatches[0].Value, "");
                }
                else
                {
                    var now = DateTime.Now;
                    Date = Date.Value.AddHours(now.Hour).AddMinutes(now.Minute);

                }
                search = search.Trim();
                searchTerms = search.Split().ToList();

                foods = context.Foods
                .Where(t => searchTerms.All(term => t.Title.ToLower().Contains(term.ToLower())))
                .Include(f => f.Meassures)
                .OrderBy(f => f.Title.ToLower().IndexOf(searchTerms.ElementAt(0).ToLower()));
            }
            if (FoodId.HasValue)
            {
                foods = context.Foods
                .Where(f => f.Id == FoodId.Value)
                .Include(f => f.Meassures);
            }

            var massUnits = context.MassUnits.ToList();
            var volumeUnits = context.VolumeUnits.ToList();

            var fatKcal = (int)(entries.Sum(e => e.Food.TotalFat * e.GetWeight() * 0.01) * 9);
            var carbohydratesKcal = (int)(entries.Sum(e => e.Food.Carbohydrates * e.GetWeight() * 0.01) * 4);
            var proteinKcal = (int)(entries.Sum(e => e.Food.Protein * e.GetWeight() * 0.01) * 4);

            //var dt = DateTime.Now;
            return View(new DiaryView
            {
                Date = Date.Value,
                //Time = dt.AddSeconds(-dt.Second).AddMilliseconds(-dt.Millisecond),
                Time = Date.Value,
                FatKcal = fatKcal,
                CarbohydratesKcal = carbohydratesKcal,
                ProteinKcal = proteinKcal,
                Weight = weight,
                RecommendedKcal = user.RecommendedKcal,
                q = q,
                FoodId = FoodId,               
                Amount = amount,
                Entries = entries,
                GroupedEntries = groupedEntries,
                RecentFoods = recentEntries.Select(e => new FoodView(e.Food, searchTerms, massUnits, volumeUnits)),
                Foods = foods.Select(f => new FoodView(f,
                    searchTerms,
                    massUnits,
                    volumeUnits)).ToList()
            });
        }

        public ActionResult Delete(int? id, DateTime date)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = context.Users
                .Include(u => u.Entries)
                .FirstOrDefault(u => u.UserName == User.Identity.Name);

            var entry = user.Entries.SingleOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }
            try
            {
                context.Entries.Remove(entry);
                context.SaveChanges();
                return RedirectToRoute("diary", new { date = date.ToString("yyyy-MM-dd"), action = "Index" });
            }
            catch (DbUpdateException /* ex */)
            {
                //TODO: Display error
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public ActionResult Index(DiaryView entryView)
        {
            var user = context.Users.Include(u => u.Entries).FirstOrDefault(u => u.UserName == User.Identity.Name);
            
            if (ModelState.IsValid) {
                if (entryView.Weight.HasValue)
                {
                    context.WeightEntries.Add(new WeightEntry { Date = entryView.Date, Weight = entryView.Weight.Value, User = user });
                }

                if (entryView.FoodId.HasValue && entryView.Amount.HasValue)
                {
                    var d = entryView.Date;
                    d = d.AddHours(entryView.Time.Hour);
                    d = d.AddMinutes(entryView.Time.Minute);
                    var entry = new Entry
                    {
                        Date = d, //TODO: Might not work
                        FoodId = entryView.FoodId.Value,
                        Quantity = entryView.Amount.Value,
                        UnitId = entryView.UnitId,
                        User = user
                    };
                    context.Add(entry);
                }
                context.SaveChanges();
            }
            else {
            entryView.Entries = context.Entries
                .Include(e => e.Food)
                    .ThenInclude(f => f.Meassures)
                .Where(e => e.UserId == user.Id && e.Date.Date == entryView.Date.Date)
                .OrderBy(e => e.Date).ToList();

            entryView.GroupedEntries = entryView.Entries
                .GroupBy(e =>
                {
                    var updated = e.Date.AddMinutes(30);
                    return new DateTime(updated.Year, updated.Month, updated.Day,
                                        updated.Hour, 0, 0, e.Date.Kind);
                }).Select(cl => cl.ToList()).ToList();

                entryView.Foods = new List<FoodView>();

                return View(entryView);
            }
            
            return RedirectToRoute("diary", new { controller = "Diary", action = "Index", date = entryView.Date.ToString("yyyy-MM-dd") });
        }

        public async Task<ActionResult> DeleteWeightEntry(DateTime date)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);

            var entry = context.WeightEntries.FirstOrDefault(w => w.UserId == user.Id && w.Date == date);
            context.WeightEntries.Remove(entry);
            await context.SaveChangesAsync();

            return RedirectToRoute("diary", new { controller = "Diary", action = "Index", date = date.ToString("yyyy-MM-dd") });

        }
    }
}