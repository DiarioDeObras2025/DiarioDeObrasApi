using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Models;
using Microsoft.EntityFrameworkCore;
using static DiarioObras.Models.Obra;

namespace DiarioObras.Data.Repositories;

public class ObraRepository : Repository<Obra>, IObraRepository
{
    public ObraRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Obra>> GetAllByEmpresaOrderedAsync(int empresaId)
    {
        return (await _context.Obras
            .Include(o => o.RegistrosDiarios)
            .Where(o => o.EmpresaId == empresaId)
            .ToListAsync())
            .OrderBy(o =>
            {
                return o.Status switch
                {
                    StatusObra.Andamento => 0,
                    StatusObra.Planejada => 1,
                    StatusObra.Pausada => 2,
                    StatusObra.Concluida => 3,
                    StatusObra.Cancelada => 4,
                    _ => 5
                };
            })
            .ThenByDescending(o => o.DataInicio);
    }


}
