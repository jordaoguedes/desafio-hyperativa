using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Service.Contract.Base.Contracts;

namespace DesafioHyperativa.Service.Contract.Base;

public class Service<T> : IService<T> where T : BaseEntity, new()
{
    protected readonly IRepository<T> Repository;

    public Service(IRepository<T> repository)
    {
        Repository = repository;
    }

    public async Task<T> GetAsync(int id)
    {
        return await this.Repository.GetAsync(id);
    }

    public async Task<T> GetAll(int id)
    {
        return await this.Repository.GetAsync(id);
    }

    public async Task SaveAsync(T entity)
    {
        await Validate(entity);
        await this.Repository.SaveAsync(entity);
    }

    public async Task SaveRangeAsync(List<T> lstEntities)
    {
        await ValidateRange(lstEntities);
        await this.Repository.SaveRangeAsync(lstEntities);
    }


    public virtual async Task Validate(T entity)
    {
        await Task.CompletedTask;
    }

    public virtual async Task ValidateRange(List<T> lstEntities)
    {
        await Task.CompletedTask;
    }
}