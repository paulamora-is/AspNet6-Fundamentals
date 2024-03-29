﻿using System.ComponentModel.DataAnnotations;

namespace AspNet_Core6.Fundamentals.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O 'Nome' é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O 'E-mail' é obrigatório")]
        [EmailAddress(ErrorMessage = "O 'Email' é inválido")]
        public string Email { get; set; }
    }
}
