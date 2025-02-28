using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart.Controllers
{
    [Route("api/home")] // route for the controller
    [ApiController]
    public class HomeController : ControllerBase 
    {

        // dependency injection of SpendSmartDBContext to 

        private readonly SpendSmartDBContext _context;

        public HomeController(SpendSmartDBContext context)
        {
            _context = context;
        }


        //  GET request for testing 
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Connection Sucessful!" });
        }


        // GET request to retrieve all expenses
        [HttpGet("expenses")]
        public async Task<IActionResult> GetAllExpenses()
        {

            // getting all expenses from the db
            var expenses = await _context.Expenses.ToListAsync();
            return Ok(expenses);
        }


        // POST request to create a new expense
        [HttpPost("create-expense")]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {
            if (expense == null)
            {
                return BadRequest("Expense data is required.");
            }

            // Add the new expense to the db
            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return Ok(new {message = "Expense Added"});
        }

        // PUT request to update an existing expense
        [HttpPut("update-expense/{id}")]
        public IActionResult UpdateExpense(int id, [FromBody] Expense expense)
        {
            if (expense == null)
            {
                return BadRequest("Expense data is required."); 
            }
            var existingExpense = _context.Expenses.Find(id);
            if (existingExpense == null)
            {
                return NotFound("Expense not found.");
            }

            // setting new values for existing expense and saving to db
            existingExpense.Value = expense.Value;
            existingExpense.Description = expense.Description;
            _context.SaveChanges();
            return Ok(new { message = "Expense Updated" });
        }


        // DELETE request to delete an existing expense
        [HttpDelete("delete-expense/{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            // removing expense from db
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return Ok(new { message = "Expense Deleted" });
        }

    }
}