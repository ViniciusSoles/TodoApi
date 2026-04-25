using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;

namespace ToDoApi.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _service;

    public TodosController(ITodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Value); 
    }




    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result.IsFailed)
            return NotFound(result.Errors);     


        return Ok(result.Value);    

    }

    [HttpPost]
    public async Task<ActionResult<TodoResponseDto>> Create([FromBody] CreateTodoDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTodoDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        if (result.IsFailed)
            return NotFound(new ProblemDetails
            {
                Title = "Recurso não encontrado.",
                Detail = result.Errors.First().Message,
                Status = 404
            });

            return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {


        var result = await _service.DeleteAsync(id);
            
        if (result.IsFailed)
            return NotFound(new ProblemDetails
            {
                Title = "Recurso não encontrado.",
                Detail = result.Errors.First().Message,
                Status = 404
                });


        return NoContent();
    }

        [HttpPatch("{id}/complete")]
        public async Task<ActionResult> Complete(int id)
        {
            var result = await _service.CompleteAsync(id);

            if (result.IsFailed)
                return BadRequest(new ProblemDetails
                {
                    Title = "Requisição inválida.",
                    Detail = result.Errors.First().Message,
                    Status = 400
                });

            return NoContent();
        }

    }

    


