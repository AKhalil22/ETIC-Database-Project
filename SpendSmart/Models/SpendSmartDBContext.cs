using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    // db context to conect to db and perform CRUD operations
    // extends DbContext class to use its methods 
    public class SpendSmartDBContext : DbContext
    {
        public IConfiguration _config { get; set; }

        public SpendSmartDBContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
