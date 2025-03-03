using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext(DbContextOptions<ApiDBContext> options) : base(options) // Pass options to base class
        {
        }
        public DbSet<Expense> Expenses { get; set; } // Create a DbSet property for the Expense model
    }
}