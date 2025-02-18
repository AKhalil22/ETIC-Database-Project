using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<ITasksService>(new InMemoryTasksService());

var app = builder.Build();

// Add middleware to rewrite URL to correct endpoint
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1", 301));

// Middleware run in a pipeline, 
// meaning when one middleware is done, it calls the next middleware

// Order of middleware is important as they run in the order they are added

// Custom middleware to log request
app.Use(async (context, next) => {
    Console.WriteLine($"Request: {context.Request.Path}");
    await next(context);
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});

// List of todos
var todos = new List<Todo>();

/* CRUD operations
Create: POST /todos
Read: GET /todos
Update: PUT /todos/{id}
Delete: DELETE /todos/{id}
*/

// Get all todos
app.MapGet("/todos", (ITasksService service) => service.GetTodos());

// Get matching todos
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id, ITasksService service) => {
    var targetTodo = service.GetTodoById(id);

    if (targetTodo == null) {
        return TypedResults.NotFound(); // 404 HTTPS status code
    } else {
        return TypedResults.Ok(targetTodo); // 200 HTTPS status code
    }
});

// Create new todo
app.MapPost("/todos", (Todo task, ITasksService service) =>
{
    service.AddTodo(task); // Add to list
    return TypedResults.Created($"/todos/{task.Id}", task);
})
.AddEndpointFilter(async (context, next) => {
    var taskArgument = context.GetArgument<Todo>(0); // Retrieve argument from context (Todo object)
    var errors = new Dictionary<string, string[]>(); // Dictionary to store errors

    // Validation Constraint 1
    if (taskArgument.DueDate < DateTime.UtcNow) { // Check if due date is in the past
        errors.Add(nameof(Todo.DueDate), new[] { "Due date must be in the future" });
    }

    // Validation Constraint 2
    if (taskArgument.IsCompleted) { // Check if task is completed
        errors.Add(nameof(Todo.IsCompleted), new[] { "New task cannot be completed" });
    }

    // return validation problem if there are errors
    if (errors.Any()) {
        return TypedResults.ValidationProblem(errors);
    }

    return await next(context);
});

// Delete todo
app.MapDelete("/todos/{id}", (int id, ITasksService service) =>
{
    service.DeleteTodoById(id); // Remove from list
    return TypedResults.NoContent(); // 204 HTTPS status code
});

app.Run();

public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);

interface ITasksService
{
    IEnumerable<Todo> GetTodos();
    Todo GetTodoById(int id);
    void AddTodo(Todo task);
    void DeleteTodoById(int id);
}

class InMemoryTasksService : ITasksService
{
    private readonly List<Todo> _todos = new();

    public IEnumerable<Todo> GetTodos() => _todos;

    public Todo GetTodoById(int id) => _todos.FirstOrDefault(t => t.Id == id);

    public void AddTodo(Todo task) => _todos.Add(task);

    public void DeleteTodoById(int id)
    {
        var task = _todos.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _todos.Remove(task);
        }
    }
}