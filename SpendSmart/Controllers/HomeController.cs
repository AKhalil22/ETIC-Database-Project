using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SpendSmartDBContext _context;

    public HomeController(ILogger<HomeController> logger, SpendSmartDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy() 
    {
        return View();
    }

    public IActionResult Expenses()
    {
        var allExpenses = _context.Expenses.ToList();
        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? id)
    {

        if (id != null)
        {
            //loading an expense by id
            var expenseInDB = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            return View(expenseInDB);
        }


        return View();
    }

    public IActionResult CreateEditExpenseForm(Expense model)
    {
        if (model.Id == 0)
        {
            // Create new expense
            _context.Expenses.Add(model);
        }
        else
        {
            // Editing an existing expense
            var existingExpense = _context.Expenses.Find(model.Id);
            if (existingExpense != null)
            {
                _context.Entry(existingExpense).CurrentValues.SetValues(model);
            }
        }

        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }


    public IActionResult DeleteExpense(int id)
    {
        var expenseInDB = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
        _context.Expenses.Remove(expenseInDB);
        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
