using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Diet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diet.ViewModels
{
    public class DiaryView
    {
        public int UnitId{get; set;}

        public IEnumerable<Entry> Entries {get; set;}

        public IEnumerable<IEnumerable<Entry>> GroupedEntries {get; set;}

        public IEnumerable<FoodView> Foods {get; set;}

        public IEnumerable<FoodView> RecentFoods {get; set;}

        [Display(Name = "Food id")]
        public int? FoodId { get; set; }

        [Display(Name = "Amount")]   
        public float? Amount {get; set;}

        [Display(Name = "Time")]
        [DataType(DataType.Time)]       
        public DateTime Time {get; set;}

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date {get; set;}

        public string q {get; set;}  

        public int RecommendedKcal {get; set;}
        
        [Display(Name = "Weight (kg)")]
        public double? Weight {get; set;}

        public int FatKcal {get; set;}
        public int CarbohydratesKcal {get; set;}
        public int ProteinKcal {get; set;}
    }
}