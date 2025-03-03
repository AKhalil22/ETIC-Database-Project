using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using System.Threading.Tasks;

namespace SpendSmart.Controllers
{
    [Route("api/home")] // route for the controller
    [ApiController]
    public class HomeController : ControllerBase
    {
        // Dependency Injection of SpendSmartDBContext to access the db
        // Preventing direct access to the database
        private readonly SpendSmartDBContext _context;
        public HomeController(SpendSmartDBContext context)
        {
            _context = context;
        }

        // GET request for testing 
        [HttpGet("test")]
        public Task<IActionResult> Test()
        {
            return Task.FromResult<IActionResult>(Ok(new { message = "Connection Sucessful!" }));
        }

        // GET (Read) request to retrieve all expenses
        [HttpGet("expenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return Ok(expenses);
        }

        // POST (Create) request to create a new expense
        [HttpPost("create-expense")]
        public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
        {
            if (expense == null)
            {
                return BadRequest("Expense data is required.");
            }

            // Add the new expense to the db
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense Added" });
        }

        // PUT (Update) request to update an existing expense
        [HttpPut("update-expense/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense expense)
        {
            if (expense == null)
            {
                return BadRequest("Expense data is required.");
            }

            var existingExpense = await _context.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

            if (existingExpense == null)
            {
                return NotFound("Expense not found.");
            }

            // Update Expense
            existingExpense.Value = expense.Value;
            existingExpense.Description = expense.Description;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense Updated" });
        }


        // DELETE request to delete an existing expense
        [HttpDelete("delete-expense/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            // removing expense from db
            _context.Expenses.Remove(expense);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense Deleted" });
        }

    }
}