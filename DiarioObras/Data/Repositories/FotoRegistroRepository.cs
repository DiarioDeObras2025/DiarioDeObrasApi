using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Models;

namespace DiarioObras.Data.Repositories
{
    public class FotoRegistroRepository : Repository<FotoRegistro>, IFotoRegistroRepository
    {
        public FotoRegistroRepository(AppDbContext context) : base(context)
        {
        }
    }
}
