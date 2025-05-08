// Interface
using DiarioObras.Models;

namespace DiarioObras.Data.Interfaces;

public interface ICustoObraRepository : IRepository<CustoObra>
{
    Task<List<CustoObra>> ListarPorObraAsync(int obraId);
    List<CustoObra>? getRelatorioByObraID(int idObra);
}