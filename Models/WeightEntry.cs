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
    public class WeightEntry
    {
        public int Id {get; set;}
        public double Weight {get; set;}
        public DateTime Date {get; set;}
        
        public ApplicationUser User {get; set;}
        public string UserId {get; set;}
    }
}