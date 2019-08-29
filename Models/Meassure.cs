using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Diet.Models
{
    public class Meassure : Unit
    {    
        public Food Food {get; set;}
        public int FoodId {get; set;}
        public override double GetDencityFactor(double dencity) {
            return 1;
        }
    }
}