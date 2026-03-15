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




    public async Task CompleteAsync(int id)
    {
       
        var todo = await _repository.GetByIdAsync(id);
        if (todo is null)
            throw new KeyNotFoundException($"Todo {id} not found.");

        todo.Complete();
        await _repository.UpdateAsync(todo);

    }

    public async Task<TodoResponseDto> CreateAsync(CreateTodoDto dto)
    {
        var todo = new Todo(dto.Title, dto.Description);
        await _repository.AddAsync(todo);
        return MapToDto(todo);


    }

    public async Task DeleteAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo is null)
            throw new KeyNotFoundException($"Todo {id} not found.");

        await _repository.DeleteAsync(todo);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllAsync()
    {
        var todo = await _repository.GetAllAsync();
        return todo.Select(MapToDto);




    }

    public async Task<TodoResponseDto?> GetByIdAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);
        return todo is null ? null : MapToDto(todo);
    }

    public async Task UpdateAsync(int id, UpdateTodoDto dto)
    {
        var todo = await _repository.GetByIdAsync(id);  
        if (todo is null)
            throw new KeyNotFoundException($"Todo {id} not found.");

        todo.Update(dto.Title, dto.Description);
        await _repository.UpdateAsync(todo);


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
















