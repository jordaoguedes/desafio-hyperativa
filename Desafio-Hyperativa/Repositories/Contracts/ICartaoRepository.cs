using DesafioHyperativa.Entities;

namespace DesafioHyperativa.Repositories.Contracts;

public interface ICartaoRepository : IRepository<Cartao>
{
    IList<Cartao> GetCartoesJaInseridos(List<string> lst);
}
