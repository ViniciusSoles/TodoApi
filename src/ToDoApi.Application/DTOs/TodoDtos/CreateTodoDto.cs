using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Application.DTOs.TodoDtos;

    public class CreateTodoDto
    {

    [Required(ErrorMessage = "Título é obrigatório.")]
    [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Title { get; set; }

    [MaxLength(500, ErrorMessage = "Máximo 500 caracteres.")]
    public string? Description { get; set; }


    }       

