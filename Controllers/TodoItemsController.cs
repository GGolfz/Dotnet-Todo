using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
namespace TodoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() {
        return await _context.TodoItems.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id) {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) {
            return NotFound();
        }
        return todoItem;
    }
}
