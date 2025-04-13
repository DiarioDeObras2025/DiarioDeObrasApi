using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Data.Repositories;
using DiarioObras.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class RegistroDiarioRepository : Repository<RegistroDiario>, IRegistroDiarioRepository
{
    public RegistroDiarioRepository(AppDbContext context) : base(context) { }

    public RegistroDiario? getRelatorioByObraID(int id)
    {
        return _context.Set<RegistroDiario>().Include(o => o.Obra).Include(x => x.Materiais).FirstOrDefault(x => x.ObraId == id);
    }
}
