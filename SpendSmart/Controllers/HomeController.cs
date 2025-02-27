using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using SpendSmart.Dtos.Expense;

// Async = Asynchronous Programming = Allows the program to run other tasks while waiting for the response
// Await = Pauses the execution of the method until the awaited task completes

namespace SpendSmart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase // Controller inherics ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDBContext _context;

        // Constructor
        public HomeController(ILogger<HomeController> logger, SpendSmartDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("expenses")] // GET (Read): api/Home/expenses
        public async Task<IActionResult> GetAllExpenses() 
        {
            return await _context.Expenses.ToListAsync(); // Returns all expenses via Defered Execution
        }

        [HttpGet("expense/{id}")] // GET (Read): api/Home/expense/#id
        public async Task<IActionResult> GetExpense([FromRoute] int id) // FromRoute = Binds the value from the route
        {
            var expense = await _context.Expenses.FindAsync(id); // Retrieve the the expense with the specified id from the database

            if (expense == null)
            {
                return NotFound(); // Returns a 404 Not Found response
            }

            return Ok(expense); // Returns a 200 OK response along with the expense
        }

        [HttpPost] // POST (Create): api/Home
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequestDto expenseDto)
        {
            // Convert DTO to Model
            var expenseModel = expenseDto.ToExpense();

            // Create expense via the repository (Data Access Layer)
            await _context.Expenses.AddAsync(expenseModel);

            // Save changes to the database
            await _context.SaveChangesAsync();


        }


    }
}