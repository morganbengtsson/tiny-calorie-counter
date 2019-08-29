using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Diet.Models
{
    public class ApplicationUser : IdentityUser
    {   
        [PersonalData]
        public ICollection<Entry> Entries {get; set;}

        [PersonalData]
        public ICollection<WeightEntry> WeightEntries {get; set;}

        [Required]
        public Gender Gender {get; set;}
        public int GenderId {get; set;}
        
        [Required]
        public double Height {get; set;}

        [Required]
        public DateTime BirthDate {get; set;}

        [Required]
        [Display(Name = "Physical activity level")]        
        public decimal PhysicalActivityLevel {get; set;}

        [Required]
        [Display(Name = "Weight loss/gain goal per week")]
        public decimal Goal {get; set;}


        [NotMapped]
        public int RecommendedKcal { 
        get {
            return Convert.ToInt32((TotalEnergyExpenditure.Value * 7.0 + (double)Goal * 7700.0) / 7.0);
        }}

        [NotMapped]
        public int Age { 
        get{
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate > today.AddYears(-age)) age--;
            return age;
        }}

        [NotMapped]
        public double? TotalEnergyExpenditure {
            get {        
                return GetBasalMetabolicRate() * (double)PhysicalActivityLevel;
            }
        }

        [NotMapped]
        public double? Weight {
            get {
                return WeightEntries.OrderByDescending(w => w.Date).FirstOrDefault()?.Weight;
            }
        }

        public double? GetBasalMetabolicRate() {
            return (Gender.Factor0 * Weight) + (Gender.Factor1 * Height) - (Gender.Factor2 * Age) + Gender.Term;
        }
    }
}