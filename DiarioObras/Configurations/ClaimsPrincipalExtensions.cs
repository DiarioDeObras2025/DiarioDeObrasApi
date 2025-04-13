using System.Security.Claims;

namespace DiarioObras.Configurations
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetEmpresaId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst("empresaId")!.Value);
        }
    }
}
