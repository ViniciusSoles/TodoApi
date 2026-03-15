using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoResponseDto>> GetAllAsync();
    Task<TodoResponseDto?> GetByIdAsync(int id);
    Task<TodoResponseDto> CreateAsync(CreateTodoDto dto);
    Task UpdateAsync(int id, UpdateTodoDto dto);
    Task DeleteAsync(int id);
    Task CompleteAsync(int id);
}
