using DiarioObras.Models;

namespace DiarioObras.Data.Interfaces;

public interface IRegistroDiarioRepository : IRepository<RegistroDiario>
{
    RegistroDiario? getRelatorioByObraID(int idObra, int IdRegistroDiario);
}
