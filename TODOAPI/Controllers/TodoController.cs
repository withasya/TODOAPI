using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TODOAPI.Dtos;
using TODOAPI.Models;
using TODOAPI.Services;

namespace TODOAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController(ITodoService todoService) : ControllerBase
    {
        private readonly ITodoService _todoService = todoService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var todos = await _todoService.GetAllTodosAsync(page, pageSize);
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponseDto>> GetById(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }


        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> Create(
            [FromQuery] Priority priority,   // Enum olarak tanımlandı
            [FromQuery] Category category,   // Enum olarak tanımlandı
            [FromBody] CreateTodoDto todoDto
        )
        {
            // Enum'ları CreateTodoDto içinde almak
            todoDto.Priority = priority;    // Enum değerini alıyoruz
            todoDto.Category = category;    // Enum değerini alıyoruz

            try
            {
                var todo = await _todoService.CreateTodoAsync(todoDto);
                return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<TodoResponseDto>> Update(int id, CreateTodoDto todoDto)
        {
            try
            {
                var todo = await _todoService.UpdateTodoAsync(id, todoDto);
                if (todo == null) return NotFound();
                return Ok(todo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _todoService.DeleteTodoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromQuery] UpdateTodoStatusDto statusDto)
        {
            var result = await _todoService.UpdateTodoStatusAsync(id, statusDto.Status);
            if (!result) return NotFound();

            // Başarılı olduğunda kullanıcıya bir başarı mesajı döndür
            return Ok(new { message = "Status successfully updated." });
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetByCategory(Category category)
        {
            var todos = await _todoService.GetTodosByCategoryAsync(category);
            return Ok(todos);
        }

        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetByPriority(Priority priority)
        {
            var todos = await _todoService.GetTodosByPriorityAsync(priority);
            return Ok(todos);
        }


        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetByStatus(TodoStatus status)
        {
            var todos = await _todoService.GetTodosByStatusAsync(status);
            return Ok(todos);
        }
    }
}