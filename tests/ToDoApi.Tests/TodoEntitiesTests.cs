using FluentAssertions;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Tests;


public class TodoEntitiesTests
{
    [Fact]
    public void Constructor_ShouldCreateTodo_WhenTitleIsValid()
    {

        //Arrange e Act 

        var todo = new Todo("Estudar", "Descirção");

        //Assert 

        todo.Title.Should().Be("Estudar");
        todo.IsCompleted.Should().BeFalse();
        todo.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));


    }


        [Fact]
        public void Constructor_ShouldThrow_WhenTitleIsEmpty()
        {
            // Arrange & Act
            var act = () => new Todo("", "Descrição");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Title cannot be empty.*");
        }


    [Fact]
    public void Complete_ShouldMarkAsCompleted()
    {
        // Arrange
        var todo = new Todo("Estudar", null);

        // Act
        todo.Complete();

        // Assert
        todo.IsCompleted.Should().BeTrue();
        todo.CompletedAt.Should().NotBeNull();
    }


    [Fact]
    public void Update_ShouldChangeTitleAndDescription()
    {
        // Arrange
        var todo = new Todo("Título antigo", null);

        // Act
        todo.Update("Título novo", "Descrição nova");

        // Assert
        todo.Title.Should().Be("Título novo");
        todo.Description.Should().Be("Descrição nova");
    }

    [Fact]
    public void Update_ShouldThrow_WhenTitleIsEmpty()
    {
        // Arrange
        var todo = new Todo("Título", null);

        // Act
        var act = () => todo.Update("", null);

        // Assert
        act.Should().Throw<ArgumentException>();
    }


















}
