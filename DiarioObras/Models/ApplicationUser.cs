using Microsoft.AspNetCore.Identity;

namespace DiarioObras.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }
    public string? Nome { get; set; }
}
