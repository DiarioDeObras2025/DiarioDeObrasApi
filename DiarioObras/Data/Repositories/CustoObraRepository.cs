using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Models;
using Microsoft.EntityFrameworkCore;

namespace DiarioObras.Data.Repositories;

public class CustoObraRepository : Repository<CustoObra>, ICustoObraRepository
{
    public CustoObraRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<CustoObra>> ListarPorObraAsync(int obraId)
    {
        return _context.Set<CustoObra>()
            .Where(c => c.ObraId == obraId)
            .OrderByDescending(c => c.Data)
            .ToListAsync();
    }

    public List<CustoObra>? getRelatorioByObraID(int idObra)
    {
        return _context.Set<CustoObra>()
            .Include(o => o.Obra)
            .Where(x => x.ObraId == idObra).ToList();
    }
}
