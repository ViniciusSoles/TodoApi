using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.DTOs.TodoDtos;
using ToDoApi.Application.Interfaces;
using ToDoApi.Domain.Shared;

namespace ToDoApi.API.Controllers;

[Authorize]
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
            return NotFound(new ProblemDetails
            {
                Title = "Recurso não encontrado.",
                Detail = result.Errors.First().Message,
                Status = 404
            });     


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
    
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<TodoResponseDto>>> GetPagination(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var pagination = new PaginationParams();
        pagination.Page = page;
        pagination.PageSize = pageSize;


        
        
       

        var result = await _service.GetPaginationAsync(pagination);
        
        return Ok(result.Value);
    }













}

    


