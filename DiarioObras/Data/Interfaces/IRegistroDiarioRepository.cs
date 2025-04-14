using DiarioObras.Models;
using System.Linq.Expressions;

namespace DiarioObras.Data.Interfaces;

public interface IRegistroDiarioRepository : IRepository<RegistroDiario>
{
    RegistroDiario? getRelatorioByObraID(int idObra, int IdRegistroDiario);
    int getTotalRelatorio(Expression<Func<RegistroDiario, bool>> predicate);
}
