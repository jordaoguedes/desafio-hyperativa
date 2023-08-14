using DesafioHyperativa.Entities;

namespace DesafioHyperativa.Repository.Contracts;

public interface ILoteRepository : IRepository<Lote>
{
    bool VerificarLoteExiste(string nome);
}
