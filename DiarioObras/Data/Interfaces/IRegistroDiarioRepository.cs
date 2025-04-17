using DiarioObras.Models;
using System.Linq.Expressions;

namespace DiarioObras.Data.Interfaces;

public interface IRegistroDiarioRepository : IRepository<RegistroDiario>
{
    RegistroDiario? getRelatorioByObraID(int idObra, int idRegistroDiario);
    Task<int> getTotalRelatorioAsync(Expression<Func<RegistroDiario, bool>> predicate);
    Task<IEnumerable<RegistroDiario>> GetAllWithObraByEmpresaAsync(int empresaId);
}

