using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.LoginDTOs;

public class LoginModelDTO
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório")]
    public string? Password { get; set; }
}
