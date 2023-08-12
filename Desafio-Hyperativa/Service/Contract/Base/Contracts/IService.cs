using DesafioHyperativa.Entities.Base;

namespace DesafioHyperativa.Service.Contract.Base.Contracts;

public interface IService<T> where T : BaseEntity, new()
{
    Task<T> GetAsync(int id);
    Task SaveAsync(T entity);
    Task SaveRangeAsync(List<T> entity);
}
