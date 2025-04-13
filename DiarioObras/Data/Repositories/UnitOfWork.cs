using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;

namespace DiarioObras.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IObraRepository? _obraRepository;
    private IRegistroDiarioRepository? _registroDiarioRepository;
    private IEmpresaRepository? _empresaRepository;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IObraRepository ObraRepository {  
        get
        { 
            return _obraRepository = _obraRepository ?? new ObraRepository(_context);
        } 
    }

    public IRegistroDiarioRepository RegistroDiarioRepository
    {
        get
        {
            return _registroDiarioRepository = _registroDiarioRepository ?? new RegistroDiarioRepository(_context);
        }
    }

    public IEmpresaRepository EmpresaRepository
    {
        get
        {
            return _empresaRepository = _empresaRepository ?? new EmpresaRepository(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }


    public void Dispose()
    {
        _context?.Dispose();
    }
}
