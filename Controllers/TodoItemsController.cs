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
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        return todoItem;
    }
    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> UpdateTodoItem(long id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return NotFound();
        item.Name = todoItem.Name;
        item.Description = todoItem.Description;
        item.IsComplete = todoItem.IsComplete;
        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) when (!TodoItemExists(id)) {
            return NotFound();
        }
        return todoItem;
    }
    private bool TodoItemExists(long id) {
        return _context.TodoItems.Any(e => e.Id.Equals(id));
    }
}
