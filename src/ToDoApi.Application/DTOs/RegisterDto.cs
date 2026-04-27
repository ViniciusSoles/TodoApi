using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Application.DTOs;

    public class RegisterDto
    {

       
        [Required(ErrorMessage = "O Nome é Obrigatório.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres .")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O Email é Obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")] 
        public string Email { get; set; }
        
        [Required(ErrorMessage = "A Senha é Obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve conter no mínimo 8 caracteres.")]    
        public string Password { get; set; }
    
 
        public string Role { get; set; } = "User";  


}

