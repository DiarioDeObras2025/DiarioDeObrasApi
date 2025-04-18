﻿using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.EmpresaDTOs
{
    public class CreateEmpresaDTO
    {
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da empresa deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Telefone da empresa é obrigatório.")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "O Responsável da empresa é obrigatório.")]
        public string Responsavel { get; set; }

    }
}
