using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Diet.Contexts;
using Diet.Models;
using Diet.ViewModels;

namespace Diet.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly MainContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public StatisticsController(MainContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [Route("[controller]/[action]/{year:int}/{week:int}", Name = "week")]
        public async Task<IActionResult> Week(int year, int week)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var weightEntries = _context.WeightEntries.Where(e => e.User == user);
            var foodEntries = _context.Entries.Include(e => e.Unit).Include(e => e.Food).Where(e => e.User == user);

            var weightList = new List<double?>();
            var calorieList = new List<double>();

            var days = new List<int> {1, 2, 3, 4, 5, 6, 0};

            foreach (int i in days)
            {
                var weightEntry = weightEntries.FirstOrDefault(e => (int)e.Date.DayOfWeek == i
                    && DateUtils.GetIso8601WeekOfYear(e.Date) == week
                    && e.Date.Year == year)?.Weight;

                weightList.Add(weightEntry);

                var calories = foodEntries.Where(e => (int)e.Date.DayOfWeek == i
                    && DateUtils.GetIso8601WeekOfYear(e.Date) == week
                    && e.Date.Year == year).ToList().Sum(e => e.EnergyKcal);

                calorieList.Add(calories);
            }

            return View(new StatisticsViewWeek(year, week, weightList, calorieList));
        }

        [Route("[controller]/[action]/{year:int}/{month:int}", Name = "month")]
        public async Task<IActionResult> Month(int year, int month)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var weightEntries = _context.WeightEntries.Where(e => e.User == user);
            var foodEntries = _context.Entries.Where(e => e.User == user).Include(e => e.Food).Include(e => e.Unit);

            var weightList = new List<double?>();
            var calorieList = new List<double>();

            var days = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= days; i++)
            {
                var weightEntry = weightEntries.FirstOrDefault(e => e.Date.Day == i)?.Weight;
                weightList.Add(weightEntry);

                var calories = foodEntries.Where(e => e.Date.Day == i).ToList().Sum(e => e.EnergyKcal);
                calorieList.Add(calories);
            }

            return View(new StatisticsViewMonth(year, month, weightList, calorieList));
        }

        [Route("[controller]/[action]/{year:int}", Name = "year")]
        public async Task<IActionResult> Year(int year)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var weightEntries = _context.WeightEntries.Where(e => e.User == user);
            var foodEntries = _context.Entries.Where(e => e.User == user).Include(e => e.Food).Include(e => e.Unit);

            var weightList = new List<double?>();
            var calorieList = new List<double>();

            var days  = 365; //TODO: Leap year;

            for (int i = 1 ; i <= days; i++){
                var weightEntry = weightEntries.FirstOrDefault(e => e.Date.Day == i)?.Weight;
                weightList.Add(weightEntry);

                var calories = foodEntries.Where(e => e.Date.Day == i).ToList().Sum(e => e.EnergyKcal);
                calorieList.Add(calories);
            }
            return View(new StatisticsViewYear(year, weightList, calorieList));
        }

    }
}