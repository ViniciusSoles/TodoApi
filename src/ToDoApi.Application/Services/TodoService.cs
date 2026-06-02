using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.DTOs.TodoDtos;
using ToDoApi.Application.Interfaces;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain.Interfaces;
using ToDoApi.Domain.Shared;

namespace ToDoApi.Application.Services;

public class TodoService : ITodoService
{

    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<IEnumerable<TodoResponseDto>>> GetAllAsync()
    {
        var todo = await _repository.GetAllAsync();
        return Result.Ok(todo.ToDtoList());
    }


    public async Task<Result<TodoResponseDto?>> GetByIdAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);
       
        if (todo is null)
            return Result.Fail($"Todo {id} not found.");    

        return Result.Ok(todo.ToDto()); 
    }


    public async Task<Result<TodoResponseDto>> CreateAsync(CreateTodoDto dto)
    {
        var todo = new Todo(dto.Title, dto.Description);
        await _repository.AddAsync(todo);
        return Result.Ok(todo.ToDto());


    }

    public async Task<Result> UpdateAsync(int id, UpdateTodoDto dto)
    {
        var todo = await _repository.GetByIdAsync(id);

        if (todo is null)
            return Result.Fail($"Todo {id} not found.");

        todo.Update(dto.Title, dto.Description);
        await _repository.UpdateAsync(todo);
        return Result.Ok();

    }


    public async Task<Result> DeleteAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo is null)
            return Result.Fail($"Todo {id} not found.");

        await _repository.DeleteAsync(todo);
        return Result.Ok();
    }


    public async Task<Result> CompleteAsync(int id)
    {
       
        var todo = await _repository.GetByIdAsync(id);
        if (todo is null)
            return Result.Fail($"Todo {id} not found.");

        if(todo.IsCompleted)
            return Result.Fail($"Todo {id} is already completed.");

        todo.Complete();
        await _repository.UpdateAsync(todo);
        return Result.Ok();
    }

  

    public async Task<Result<PagedResult<TodoResponseDto>>> GetPaginationAsync(PaginationParams pagination)
    {
        var(items, total) = await _repository.GetPaginationAsync(pagination);

        var result = new PagedResult<TodoResponseDto>
        {
            Data = items.ToDtoList(),
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalItems = total,
            TotalPages = (int)Math.Ceiling(total / (double)pagination.PageSize)
        };

        return Result.Ok(result);


    }
}


















