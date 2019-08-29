using Diet.Models;
using System.Collections.Generic;

namespace Diet.Models
{    
    public class Gender
    {        
        public int Id {get; set;}
        
        public string Name {get; set;}
        
        public ICollection<ApplicationUser> Users {get; set;}
        
        // First factor in Harris-benedict formula
        public double Factor0 {get; set;}
        
        // Second facotr in Harris-benedict formula
        public double Factor1 {get; set;}
        
        // Third factor in Harris-benedict formula
        public double Factor2 {get; set;}
        
        // Addition term in Harris-Benedict formula
        public double Term {get; set;}
    }
}