using Microsoft.EntityFrameworkCore;
using Diet.Models;

namespace Diet.Contexts 
{
    public class SQLiteContext : MainContext
    {
        public SQLiteContext()
            : base(new DbContextOptions<MainContext>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Diet.db");

        public DbSet<Diet.Models.WeightEntry> WeightEntry { get; set; }
    }
}