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

    public class Entry
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Date")]
        public DateTime Date {get; set;}

        [Display(Name = "Food")]
        public Food Food {get; set;}     
        public int FoodId{get; set;}

        [Display(Name = "Unit")]
        public Unit Unit {get; set;}
        public int UnitId {get; set;}

        [Display(Name = "Quantity")]
        public double Quantity {get; set;}

        [Display(Name = "Energy (kcal)")]
        public int EnergyKcal {
            get {
                return (int)(Quantity * Unit.Factor * 0.01 * Food.EnergyKcal * Unit.GetDencityFactor(Food.Dencity));
            }
        }

        public double GetWeight(){
            return Quantity * Unit.Factor * Unit.GetDencityFactor(Food.Dencity);
        }

        public ApplicationUser User {get; set;}
        public string UserId {get; set;}

        public string GetColor() {
            byte[] bytes = Encoding.ASCII.GetBytes(Food.Title.Count() > 3 ? Food.Title.Substring(0, 3) : "000" );
            var value = (double)bytes[0] - 65.0;
            value /= 57;
            value = Math.Clamp(value, 0.0, 1.0);
            var color = Util.ColorFromHSV(value * 360.0, 0.5, 1.0); 
            return ColorTranslator.ToHtml(color);
        }
    }
}