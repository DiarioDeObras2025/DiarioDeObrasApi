using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.Models;

namespace DiarioObras.Data.Repositories;

public class ObraRepository : Repository<Obra>, IObraRepository
{
    public ObraRepository(AppDbContext context) : base(context)
    {
    }
}
