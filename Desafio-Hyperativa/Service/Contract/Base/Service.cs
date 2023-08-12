﻿using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Repositories.Contracts;
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
        Validate(entity);
        await this.Repository.SaveAsync(entity);
    }

    public async Task SaveRangeAsync(List<T> entity)
    {
        ValidateRange(entity);
        await this.Repository.SaveRangeAsync(entity);
    }


    public virtual void Validate(T entity)
    {

    }

    public virtual void ValidateRange(List<T> entity)
    {

    }
}