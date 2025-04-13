using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Models;

namespace DiarioObras.Data.Repositories
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(AppDbContext context) : base(context)
        {
        }
    }
}
