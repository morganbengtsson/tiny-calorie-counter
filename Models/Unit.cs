using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Diet.Models
{
    public abstract class Unit
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public string Symbol {get; set;}
        
        [Required]
        public string Name {get; set;}
        
        [Required]
        public double Factor {get; set;}

        public ICollection<Entry> Entries {get; set;}

        abstract public double GetDencityFactor(double dencity);

        public override string ToString()
        {
            return Name + (Symbol.Length > 0 ? " (" + Symbol + ")" : "");
        }
    }
}