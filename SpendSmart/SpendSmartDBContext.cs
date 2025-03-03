using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpendSmart.Models;
using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    public class SpendSmartDBContext : DbContext
    {
        public SpendSmartDBContext(DbContextOptions<SpendSmartDBContext> options)
            : base(options)
        {

        }

        public DbSet<Expense> Expenses { get; set; }
    }
}