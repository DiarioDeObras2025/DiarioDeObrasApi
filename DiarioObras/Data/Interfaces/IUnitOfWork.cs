namespace DiarioObras.Data.Interfaces;

public interface IUnitOfWork
{
    IObraRepository ObraRepository { get; }
    IRegistroDiarioRepository RegistroDiarioRepository { get; }
    IEmpresaRepository EmpresaRepository { get; }
    IFotoRegistroRepository FotoRegistroRepository { get; }
    void Commit();
    Task CommitAsync();
}
