using Microsoft.EntityFrameworkCore;

namespace DiarioObras.Data.Interfaces;

public interface IUnitOfWork
{
    IObraRepository ObraRepository { get; }
    IRegistroDiarioRepository RegistroDiarioRepository { get; }
    IEmpresaRepository EmpresaRepository { get; }
    IFotoRegistroRepository FotoRegistroRepository { get; }
    ICustoObraRepository CustoObraRepository { get; }
    void Commit();
    Task CommitAsync();

    DbContext Context { get; }
}
