using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Application.DTOs;


public class UpdateTodoDto
{
    public string Title { get; set; }
    public string? Description { get; set; }

}