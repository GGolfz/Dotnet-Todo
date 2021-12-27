using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TodoAPI.Models
{
    public class TodoContext : DbContext {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) {

        }
        public DbSet<TodoItem> TodoItems {get; set;} = null!;
    }
}