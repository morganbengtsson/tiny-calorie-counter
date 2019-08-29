using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using Diet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diet.ViewModels
{
    public class FoodView
    {
        public FoodView(Food food,
            IEnumerable<string> searchTerms, 
            ICollection<MassUnit> massUnits, 
            ICollection<VolumeUnit> volumeUnits)
        {
            Id = food.Id;
            string title = food.Title;

            foreach (var search in searchTerms)
            {
                var index = title.IndexOf(search, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    var strong = title.Substring(index, search.Length);
                    title = title.Replace(strong, "<strong>" + strong + "</strong>");
                }
            }
            HtmlTitle = new HtmlString(title);
            Title = food.Title;
            Color = food.GetColor();

            Energy = food.EnergyKcal;
            Carbohydrates = food.Carbohydrates;
            TotalFat = food.TotalFat;
            Protein = food.Protein;
            DietaryFibre = food.DietaryFibre;
            Water = food.Water;
            Alcohol = food.Alcohol;

            var weightUnitsGroup = new SelectListGroup { Name = "Weight" };
            var volumeUnitsGroup = new SelectListGroup { Name = "Volume" };
            var otherUnitsGroup = new SelectListGroup { Name = "Other" };

            Units = new List<SelectListItem>();

            foreach (var unit in massUnits)
            {
                Units.Add(new SelectListItem
                {
                    Value = unit.Id.ToString(),
                    Text = unit.ToString(),
                    Group = weightUnitsGroup
                });
            }
            if (food.Dencity > 0) {
                foreach (var unit in volumeUnits)
                {
                    Units.Add(new SelectListItem
                    {
                        Value = unit.Id.ToString(),
                        Text = unit.ToString(),
                        Group = volumeUnitsGroup
                    });
                }
            }

            foreach (var unit in food.Meassures)
            {
                Units.Add(new SelectListItem
                {
                    Value = unit.Id.ToString(),
                    Text = unit.ToString(),
                    Group = otherUnitsGroup
                });
            }
        }

        public string Color {get; set;}

        [Display(Name = "Date")]
        DateTime Date { get; set; }

        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Html Title")]
        public HtmlString HtmlTitle { get; set; }

        [Display(Name = "Energy")]
        public double Energy { get; set; }

        [Display(Name = "Carbohydrates")]
        public double Carbohydrates { get; set; }

        [Display(Name = "Fat")]
        public double TotalFat { get; set; }

        [Display(Name = "Protein")]
        public double Protein { get; set; }

        [Display(Name = "Fibre (g)")]
        public double DietaryFibre { get; set; }

        [Display(Name = "Water (g)")]
        public double Water { get; set; }

        [Display(Name = "Alcohol (g)")]
        public double Alcohol { get; set; }

        [Display(Name = "Unit")]
        public ICollection<SelectListItem> Units { get; set; }
    }
}