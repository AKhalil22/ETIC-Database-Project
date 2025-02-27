using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Why Data Transfer Objects (Dtos)?
// - Seperation of Concerns: Sperates API layer from the Data Access
// - Security: Prevents over-posting (Mass Assignment) attacks
// - Flexibility: Allows for easy versioning of the API

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