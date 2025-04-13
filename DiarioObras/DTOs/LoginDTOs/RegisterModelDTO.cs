using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.LoginDTOs;

public class RegisterModelDTO
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório")]
    public string? Password { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email é obrigatório")]
    public string? Email { get; set; }

    [Phone]
    [Required(ErrorMessage = "Telefone é obrigatório")]
    public string? PhoneNumber { get; set; }
}
