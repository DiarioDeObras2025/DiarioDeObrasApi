using DiarioObras.Models;
namespace DiarioObras.Data.Interfaces;
public interface IObraRepository : IRepository<Obra>
{
    Task<IEnumerable<Obra>> GetAllByEmpresaOrderedAsync(int empresaId);
}
