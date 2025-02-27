using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpendSmart.Dtos.Expense
{
    public class CreateExpenseRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}