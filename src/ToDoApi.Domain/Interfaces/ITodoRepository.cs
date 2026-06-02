using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain.Shared;

namespace ToDoApi.Domain.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task AddAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Todo todo);
    Task<(IEnumerable<Todo> Items, int Total)> GetPaginationAsync(PaginationParams pagination);

}

