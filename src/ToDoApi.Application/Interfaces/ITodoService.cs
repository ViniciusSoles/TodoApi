using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.DTOs.TodoDtos;
using ToDoApi.Domain.Shared;

namespace ToDoApi.Application.Interfaces;

public interface ITodoService
{
    Task<Result<IEnumerable<TodoResponseDto>>> GetAllAsync();
    Task<Result<TodoResponseDto?>> GetByIdAsync(int id);
    Task<Result<TodoResponseDto>> CreateAsync(CreateTodoDto dto);
    Task<Result> UpdateAsync(int id, UpdateTodoDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> CompleteAsync(int id);

    Task<Result<PagedResult<TodoResponseDto>>> GetPaginationAsync(PaginationParams pagination);


}
