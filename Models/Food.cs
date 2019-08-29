using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Diet.Models
{
    public class Food
    {
        public string GetColor() {
            byte[] bytes = Encoding.ASCII.GetBytes(Title.Count() > 3 ? Title.Substring(0, 3) : "000" );
            var value = (double)bytes[0] - 65.0;
            value /= 57;
            value = Math.Clamp(value, 0.0, 1.0);
            var color = Util.ColorFromHSV(value * 360.0, 0.5, 1.0); 
            return ColorTranslator.ToHtml(color);
        }

        [Display(Name = "Meassures")]
        // Custom meassurements for the food. eg pint, bar, cube etc
        public ICollection<Meassure> Meassures {get; set;}

        [Display(Name = "Code")]
        [Required]
        // ASNUT code        
        public string Code { get; set; }

        [Display(Name = "Dencity")]
        [Required]
        public double Dencity { get; set; }

        [Display(Name = "Id")]
        public int Id { get; set; }


        [Display(Name = "Entries")]
        public ICollection<Entry> Entries { get; set; }

        [Display(Name = "Title")]
        [Required]
        public string Title {get; set;}
        
        [Display(Name = "Energy (kcal)")]
        [Range(0.0, 900.0)]
        [NotMapped]
        public double EnergyKcal
        {
            get
            {
                return EnergyKJ / 4.184;
            }
            set
            {
                EnergyKJ = value * 4.184;
            }
        }

        [Display(Name = "Energy (kJ)")]
        public double EnergyKJ { get; set; }

        [Display(Name = "Water (g)")]
        [Range(0.0, 100.0)]
        /// Also called moisture
        public double Water { get; set; }

        [Display(Name = "Protein (g)")]
        [Range(0.0, 100.0)]
        public double Protein { get; set; }

        [Display(Name = "Total fat (g)")]
        [Range(0.0, 100.0)]
        public double TotalFat { get; set; }

        [Display(Name = "Carbohydrates (g)")]
        [Range(0.0, 100.0)]
        /// With dietary fibre
        public double Carbohydrates { get; set; }

        [Display(Name = "Starch (g)")]
        [Range(0.0, 100.0)]
        public double Starch { get; set; }

        [Display(Name = "Total sugar (g)")]
        [Range(0.0, 100.0)]
        public double TotalSugars { get; set; }

        [Display(Name = "Dietary fibre (g)")]
        [Range(0.0, 100.0)]
        public double DietaryFibre { get; set; }

        [Display(Name = "Alcohol (g)")]
        [Range(0.0, 100.0)]
        public double Alcohol { get; set; }

        [Display(Name = "Ash (g)")]
        [Range(0.0, 100.0)]
        public double Ash { get; set; }

        [Display(Name = "Retinol (Preformed vitamin A) (µg)")]
        [Range(0.0, 100000000.0)]
        public double Retinol { get; set; }

        [Display(Name = "β-Carotene (µg)")]
        [Range(0.0, 100000000.0)]
        public double BetaCarotene { get; set; }

        [Display(Name = "Provitamin A (β-Carotene equivalents) (µg)")]
        [Range(0.0, 100000000.0)]
        public double ProvitaminA { get; set; }

        [Display(Name = "Vitamin A (Retinol equivalents) (µg)")]
        [Range(0.0, 100000000.0)]
        public double VitaminA { get; set; }

        [Display(Name = "Thiamin (B1) (mg)")]
        [Range(0.0, 100000.0)]
        public double Thiamin { get; set; }

        [Display(Name = "Riboflavin (B2) (mg)")]
        [Range(0.0, 100000.0)]
        public double Riboflavin { get; set; }

        [Display(Name = "Niacin (B3) (mg)")]
        [Range(0.0, 100000.0)]
        public double Niacin { get; set; }

        [Display(Name = "Niacin equivalents (mg)")]
        [Range(0.0, 100000.0)]
        public double NiacinEquivalents { get; set; }

        [Display(Name = "Total folates (µg)")]
        [Range(0.0, 100000000.0)]
        // Total (natural and acid)
        public double TotalFolates { get; set; }

        [Display(Name = "Vitamin B6 (mg)")]
        [Range(0.0, 100000.0)]
        public double VitaminB6 { get; set; }

        [Display(Name = "Vitamin B12 (µg)")]
        [Range(0.0, 100000000.0)]
        public double VitaminB12 { get; set; }

        [Display(Name = "Vitamin C (mg)")]
        [Range(0.0, 100000.0)]
        public double VitaminC { get; set; }

        [Display(Name = "α-Tocopherol (mg)")]
        [Range(0.0, 100000000.0)]
        // E307
        public double AlphaTocopherol { get; set; }

        [Display(Name = "Vitamin E (mg)")]
        [Range(0.0, 1000000.0)]
        public double VitaminE { get; set; }

        [Display(Name = "Calcium (Ca) (mg)")]
        [Range(0.0, 100000.0)]
        public double Calcium { get; set; }

        [Display(Name = "Iodine (I) (µg)")]
        [Range(0.0, 10000000.0)]
        public double Iodine { get; set; }

        [Display(Name = "Iron (Fe) (mg)")]
        [Range(0.0, 100000.0)]
        public double Iron { get; set; }

        [Display(Name = "Magnesium (Mg) (mg)")]
        [Range(0.0, 100000.0)]
        public double Magnesium { get; set; }

        [Display(Name = "Phosphorus (P) (mg)")]
        [Range(0.0, 10000.0)]
        public double Phosphorus { get; set; }

        [Display(Name = "Potassium (K) (mg)")]
        [Range(0.0, 100000.0)]
        public double Potassium { get; set; }

        [Display(Name = "Selenium (Se) (µg)")]
        [Range(0.0, 100000000.0)]
        public double Selenium { get; set; }

        [Display(Name = "Sodium (Na) (mg)")]
        [Range(0.0, 100000.0)]
        public double Sodium { get; set; }

        [Display(Name = "Zinc (Zn) (mg)")]
        [Range(0.0, 100000.0)]
        public double Zinc { get; set; }

        [Display(Name = "Caffeine (mg)")]
        [Range(0.0, 100000.0)]
        public double Caffeine { get; set; }

        [Display(Name = "Cholesterol (mg)")]
        [Range(0.0, 100000.0)]
        public double Cholesterol { get; set; }

        [Display(Name = "Tryptophan (mg)")]
        [Range(0.0, 100000.0)]
        public double Tryptophan { get; set; }

        [Display(Name = "Total saturated fat (g)")]
        [Range(0.0, 100.0)]
        public double TotalSaturatedFat { get; set; }

        [Display(Name = "Total monounsaturated fat (g)")]
        [Range(0.0, 100.0)]
        public double TotalMonounsaturatedFat { get; set; }

        [Display(Name = "Total polyunsaturated fat (g)")]
        [Range(0.0, 100.0)]
        public double TotalPolyunsaturatedFat { get; set; }

        [Display(Name = "Linoleic acid (g)")]
        [Range(0.0, 100.0)]
        public double LinoleicAcid { get; set; }

        [Display(Name = "α-Linolenic acid (g)")]
        [Range(0.0, 100.0)]
        public double AlphaLinolenicAcid { get; set; }

        [Display(Name = "Total omega 3 fatty acids (mg)")]
        [Range(0.0, 100000.0)]
        public double TotalOmega3FattyAcids { get; set; }

        [Display(Name = "Total trans fatty acids (mg)")]
        [Range(0.0, 100000.0)]
        public double TotalTransFattyAcids { get; set; }

        /*
        [Display(Name = "Fatty acid (g)")]
        [Range(0.0, 100.0)]
        public double FattyAcid { get; set; }

        [Display(Name = "Lauric acid (g)")]
        [Range(0.0, 100.0)]
        public double LauricAcid { get; set; }

        [Display(Name = "Myristic acid (g)")]
        [Range(0.0, 100.0)]
        public double MyristicAcid { get; set; }

        [Display(Name = "Palmitic acid (g)")]
        [Range(0.0, 100.0)]
        public double PalmiticAcid { get; set; }

        [Display(Name = "Stearic acid (g)")]
        [Range(0.0, 100.0)]
        public double StearicAcid { get; set; }

        [Display(Name = "Arachidic acid (g)")]
        [Range(0.0, 100.0)]
        public double ArachidicAcid { get; set; }

        [Display(Name = "Palmitoleic acid (g)")]
        [Range(0.0, 100.0)]
        public double PalmitoleicAcid { get; set; }

        [Display(Name = "Oleic acid (g)")]
        [Range(0.0, 100.0)]
        public double OleicAcid { get; set; }
      
        [Display(Name = "Whole grains (g)")]
        [Range(0.0, 100.0)]
        public double WholeGrains { get; set; }

        [Display(Name = "Salt (g)")]
        [Range(0.0, 100.0)]
        public double Salt { get; set; }
      
        [Display(Name = "Arachidonic acid (g)")]
        [Range(0.0, 100.0)]
        public double ArachidonicAcid { get; set; }

        [Display(Name = "EPA (g)")]
        [Range(0.0, 100.0)]
        public double EPA { get; set; }

        [Display(Name = "DPA (g)")]
        [Range(0.0, 100.0)]
        public double DPA { get; set; }

        [Display(Name = "DHA (g)")]
        [Range(0.0, 100.0)]
        public double DHA { get; set; }

        [Display(Name = "Monosaccharides (g)")]
        [Range(0.0, 100.0)]
        public double Monosaccharides { get; set; }

        [Display(Name = "Disaccharides (g)")]
        [Range(0.0, 100.0)]
        public double Disaccharides { get; set; }

        [Display(Name = "Sucrose (g)")]
        [Range(0.0, 100.0)]
        public double Sucrose { get; set; }
        */
    }
}