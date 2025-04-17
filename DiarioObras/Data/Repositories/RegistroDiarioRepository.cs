using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Data.Repositories;
using DiarioObras.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class RegistroDiarioRepository : Repository<RegistroDiario>, IRegistroDiarioRepository
{
    public RegistroDiarioRepository(AppDbContext context) : base(context) { }

    public RegistroDiario? getRelatorioByObraID(int idObra, int idRegistroDiario)
    {
        return _context.Set<RegistroDiario>()
            .Include(o => o.Obra)
            .Include(x => x.Materiais)
            .Include(x => x.Equipe)
            .Include(f => f.Fotos)
            .FirstOrDefault(x => x.ObraId == idObra && x.Id == idRegistroDiario);
    }

    public async Task<int> getTotalRelatorioAsync(Expression<Func<RegistroDiario, bool>> predicate)
    {
        return await _context.Set<RegistroDiario>().CountAsync(predicate);
    }

    public async Task<IEnumerable<RegistroDiario>> GetAllWithObraByEmpresaAsync(int empresaId)
    {
        return await _context.Set<RegistroDiario>()
            .Include(r => r.Obra)
            .Where(r => r.Obra.EmpresaId == empresaId)
            .AsNoTracking()
            .OrderByDescending(r => r.Data)
            .ToListAsync();
    }
}
