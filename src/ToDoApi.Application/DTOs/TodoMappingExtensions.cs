using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.DTOs;

public static class TodoMappingExtensions
{
    public static TodoResponseDto ToDto(this Todo todo) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description,
        IsCompleted = todo.IsCompleted,
        CreatedAt = todo.CreatedAt,
        CompletedAt = todo.CompletedAt
    };

    public static IEnumerable<TodoResponseDto> ToDtoList(this IEnumerable<Todo> todos) =>
        todos.Select(t => t.ToDto());
}
    





