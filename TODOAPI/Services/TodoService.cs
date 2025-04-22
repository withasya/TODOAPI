using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TODOAPI.Data;
using TODOAPI.Dtos;
using TODOAPI.Models;

namespace TODOAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;

        public TodoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Todos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<TodoResponseDto> GetTodoByIdAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return null;

            return MapToDto(todo);
        }

        public async Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto todoDto)
        {
            // Aynı başlıkta tamamlanmamış todo var mı kontrol et
            var existingTodo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Title == todoDto.Title && t.Status != TodoStatus.Completed);

            if (existingTodo != null)
            {
                throw new InvalidOperationException("Bu başlıkta tamamlanmamış bir todo zaten mevcut.");
            }

            var todo = new Todo
            {
                Title = todoDto.Title,
                Description = todoDto.Description,
                CreatedAt = DateTime.UtcNow,
                Deadline = todoDto.Deadline,
                Priority = (Priority)Enum.Parse(typeof(Priority), todoDto.Priority),
                Category = (Category)Enum.Parse(typeof(Category), todoDto.Category),
                Status = TodoStatus.ToDo
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return MapToDto(todo);
        }

        public async Task<TodoResponseDto> UpdateTodoAsync(int id, CreateTodoDto todoDto)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return null;

            // Başlık değişiyorsa ve yeni başlıkta tamamlanmamış todo var mı kontrol et
            if (todo.Title != todoDto.Title)
            {
                var existingTodo = await _context.Todos
                    .FirstOrDefaultAsync(t => t.Title == todoDto.Title && t.Status != TodoStatus.Completed && t.Id != id);

                if (existingTodo != null)
                {
                    throw new InvalidOperationException("Bu başlıkta tamamlanmamış bir todo zaten mevcut.");
                }
            }

            todo.Title = todoDto.Title;
            todo.Description = todoDto.Description;
            todo.Deadline = todoDto.Deadline;
            todo.Priority = (Priority)Enum.Parse(typeof(Priority), todoDto.Priority);
            todo.Category = (Category)Enum.Parse(typeof(Category), todoDto.Category);

            await _context.SaveChangesAsync();
            return MapToDto(todo);
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTodoStatusAsync(int id, TodoStatus status)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;

            todo.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TodoResponseDto>> GetTodosByCategoryAsync(Category category)
        {
            return await _context.Todos
                .Where(t => t.Category == category)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAsync(Priority priority)
        {
            return await _context.Todos
                .Where(t => t.Priority == priority)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        private static TodoResponseDto MapToDto(Todo todo)
        {
            return new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                CreatedAt = todo.CreatedAt,
                Deadline = todo.Deadline,
                Status = todo.Status.ToString(),
                Priority = todo.Priority.ToString(),
                Category = todo.Category.ToString()
            };
        }
    }
}