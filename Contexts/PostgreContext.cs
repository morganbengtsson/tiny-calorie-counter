 using Microsoft.EntityFrameworkCore;
 
 namespace Diet.Contexts 
 {
 public class PostgreContext : MainContext
    {       
        public PostgreContext(DbContextOptions<PostgreContext> options)
            : base(options) 
        {
        }
    }
 }