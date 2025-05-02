using System.Collections.Generic;
using System.Threading.Tasks;
using TODOAPI.Dtos;
using TODOAPI.Models;

namespace TODOAPI.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync(int page = 1, int pageSize = 10);
        Task<TodoResponseDto> GetTodoByIdAsync(int id);
        Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto todoDto);
        Task<TodoResponseDto> UpdateTodoAsync(int id, CreateTodoDto todoDto);
        Task<bool> DeleteTodoAsync(int id);
        Task<bool> UpdateTodoStatusAsync(int id, TodoStatus status);
        Task<IEnumerable<TodoResponseDto>> GetTodosByCategoryAsync(Category category);
        Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAsync(Priority priority);
        Task<IEnumerable<TodoResponseDto>> GetTodosByStatusAsync(TodoStatus status);
    }
}