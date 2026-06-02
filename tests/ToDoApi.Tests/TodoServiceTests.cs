using FluentAssertions; 
using NSubstitute;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.DTOs.TodoDtos;
using ToDoApi.Application.Services;
using ToDoApi.Domain.Entities;  
using ToDoApi.Domain.Interfaces;

namespace ToDoApi.Tests;

    public class TodoServiceTests
    {

      private readonly ITodoRepository _repository;
      private readonly TodoService _service;

    public TodoServiceTests()
    {
        _repository = Substitute.For<ITodoRepository>();
        _service = new TodoService(_repository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTodo_WhenValid()
    {
        // Arrange
        var dto = new CreateTodoDto { Title = "Estudar C#" };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Estudar C#");

        // verifica que o repositório foi chamado
        await _repository.Received(1).AddAsync(Arg.Any<Todo>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTodo_WhenExists()
    {
        // Arrange
        var todo = new Todo("Estudar", null);
        _repository.GetByIdAsync(1).Returns(todo);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Estudar");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFail_WhenNotExists()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Todo?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Message.Should().Contain("99");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFail_WhenNotExists()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Todo?)null);

        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task CompleteAsync_ShouldReturnFail_WhenAlreadyCompleted()
    {
        // Arrange
        var todo = new Todo("Estudar", null);
        todo.Complete(); // já completo
        _repository.GetByIdAsync(1).Returns(todo); 

        // Act
        var result = await _service.CompleteAsync(1);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Message.Should().Contain($"Todo 1 is already completed.");
    }

    [Fact]
    public async Task CompleteAsync_ShouldComplete_WhenValid()
    {
        // Arrange
        var todo = new Todo("Estudar", null);
        _repository.GetByIdAsync(1).Returns(todo);

        // Act
        var result = await _service.CompleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _repository.Received(1).UpdateAsync(Arg.Any<Todo>());
    }

}

