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
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        return ItemToDTO(todoItem);
    }
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
    {
        var todoItem = new TodoItem{
            Name = todoItemDTO.Name,
            Description = todoItemDTO.Description,
            IsComplete = todoItemDTO.IsComplete
        };
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItemDTO>> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
    {
        if (id != todoItemDTO.Id)
        {
            return BadRequest();
        }
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return NotFound();
        item.Name = todoItemDTO.Name;
        item.Description = todoItemDTO.Description;
        item.IsComplete = todoItemDTO.IsComplete;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }
        return ItemToDTO(item);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return StatusCode(200);
    }
    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id.Equals(id));
    }
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) => new TodoItemDTO
    {
        Id = todoItem.Id,
        Name = todoItem.Name,
        Description = todoItem.Description,
        IsComplete = todoItem.IsComplete
    };

}
