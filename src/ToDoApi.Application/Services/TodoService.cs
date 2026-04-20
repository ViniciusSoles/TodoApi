using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain.Interfaces;

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
        return Result.Ok(todo.Select(MapToDto));
    }


    public async Task<Result<TodoResponseDto?>> GetByIdAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);
        return todo is null ? null : MapToDto(todo);
    }


    public async Task<Result<TodoResponseDto>> CreateAsync(CreateTodoDto dto)
    {
        var todo = new Todo(dto.Title, dto.Description);
        await _repository.AddAsync(todo);
        return Result.Ok(MapToDto(todo));


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

        todo.Complete();
        await _repository.UpdateAsync(todo);
        return Result.Ok();
    }


    private static TodoResponseDto MapToDto(Todo todo) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description,
        IsCompleted = todo.IsCompleted,
        CreatedAt = todo.CreatedAt,
        CompletedAt = todo.CompletedAt
    };

    
}
















