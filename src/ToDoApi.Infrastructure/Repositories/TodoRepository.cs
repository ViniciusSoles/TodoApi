using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain.Interfaces;
using ToDoApi.Infrastructure.Data;

namespace ToDoApi.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{

    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

public async Task<IEnumerable<Todo>> GetAllAsync()
{
    return await _context.Todos.ToListAsync();
}

public async Task<Todo?> GetByIdAsync(int id)
{
    return await _context.Todos.FindAsync(id);
}

public async Task AddAsync(Todo todo)
{
    await _context.Todos.AddAsync(todo);
    await _context.SaveChangesAsync();
}

public async Task UpdateAsync(Todo todo)
{
    _context.Todos.Update(todo);
    await _context.SaveChangesAsync();
}

public async Task DeleteAsync(Todo todo)
{
    _context.Todos.Remove(todo);
    await _context.SaveChangesAsync();
}
}