using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;

namespace DesafioHyperativa.Repository.Contracts;

public interface IRepository<T> where T : BaseEntity
{
    #region Persistencia
    Task<T> SaveAsync(T entity);
    Task SaveRangeAsync(List<T> entity);
    #endregion

    #region Pesquisa
    Task<T> GetAsync(int id);
    #endregion
}
