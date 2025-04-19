using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Contollers
{
    [Route("api/expense")] // Base controller route
    [ApiController]
    public class ExpenseContoller : ControllerBase 
    {
        // Dependency injection
        private readonly ApiDBContext _context;
        public ExpenseContoller(ApiDBContext context)
        {
            _context = context; // Prevent direct access to the database
        }

        [HttpGet] // Get (Read): api/expense
        public async Task<IActionResult> GetExpenses()
        {
            var expenses = await _context.Expenses.ToListAsync(); // Get all expenses
            return Ok(expenses); // Return all expenses
        }

        [HttpGet("id/{id}")] // Get (Read): api/expense/{id}
        public async Task<IActionResult> GetById([FromRoute] int id)
        { 
            var expense = await _context.Expenses.FindAsync(id); // Find expense by id

            if (expense == null)
            {
                return NotFound(); // Return 404 if expense is not found
            }

            return Ok(expense); // Return expense
        }

        [HttpGet("description/{description}")]
public async Task<IActionResult> SearchByDescription([FromRoute] string description)
{
    var expenses = await _context.Expenses
        .Where(e => e.Description.Contains(description))
        .ToListAsync();

    if (expenses == null || !expenses.Any())
    {
        return NotFound("No expenses match the given description.");
    }

    return Ok(expenses);
}

[HttpGet("amount/{amount}")]
public async Task<IActionResult> SearchByAmount([FromRoute] decimal amount)
{
    var expenses = await _context.Expenses
        .Where(e => e.Amount == amount)
        .ToListAsync();

    if (expenses == null || !expenses.Any())
    {
        return NotFound("No expenses match the given amount.");
    }

    return Ok(expenses);
}


        [HttpPost] // Post (Create): api/expense
        public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
        {
            await _context.Expenses.AddAsync(expense); // Add expense to the database
            await _context.SaveChangesAsync(); // Save changes to the database

            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense); // Return 201 Created
        }

        [HttpPut("{id}")] // Put (Update): api/expense/{id}
        public async Task<IActionResult> UpdateExpense([FromRoute] int id, [FromBody] Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest(); // Return 400 Bad Request
            }

            _context.Entry(expense).State = EntityState.Modified; // Update expense
            await _context.SaveChangesAsync(); // Save changes to the database

            return NoContent(); // Return 204 No Content
        }

        [HttpDelete] // Delete: api/expense/{id}
        [Route("{id}")]
        public async Task<IActionResult> DeleteExpense([FromRoute] int id)
        {
            var expense = await _context.Expenses.FindAsync(id); // Find expense by id

            if (expense == null)
            {
                return NotFound(); // Return 404 if expense is not found
            }

            _context.Expenses.Remove(expense); // Remove expense from the database
            await _context.SaveChangesAsync(); // Save changes to the database

            return NoContent(); // Return 204 No Content
        }
    }
}
