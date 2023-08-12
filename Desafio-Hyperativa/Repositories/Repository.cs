﻿using DesafioHyperativa.Context;
using DesafioHyperativa.CrossCutting.Contract;
using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DesafioHyperativa.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ContextDb _context;
    protected readonly DbSet<T> DbSet;
    private string _transactionGuid = Guid.NewGuid().ToString();

    public Repository(ContextDb context, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        DbSet = _context.Set<T>();
    }

    public async Task<T> GetAsync(int id)
    {
        return await this.DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task SaveRangeAsync(List<T> entities)
    {
        var lstInsert = entities.Where(x => x.Id == 0).ToList();
        var lstUpdate = entities.Where(x => x.Id != 0).ToList();

        if (lstInsert.Any())
            await InsertRangeAsync(lstInsert);

        if (lstUpdate.Any())
            await UpdateRangeAsync(lstInsert);
    }


    public async Task<T> SaveAsync(T entity)
    {
        try
        {        
            _unitOfWork.BeginTransaction(_transactionGuid);

            if(entity.Id == 0)
                await this.DbSet.AddAsync(entity);
            else
                this.DbSet.Update(entity);

            await _context.SaveChangesAsync();

            await _unitOfWork.CommitAsync(_transactionGuid);

            return entity;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackAsync(_transactionGuid);

            if (ex.InnerException != null)
            {
               throw new Exception(ex.InnerException.Message);
            }
            
            throw;
        }      
    }


    private async Task InsertRangeAsync(IList<T> entity)
    {
        try
        {
            _unitOfWork.BeginTransaction(_transactionGuid);

            this.DbSet.AddRange(entity);

            await _context.SaveChangesAsync();

            await _unitOfWork.CommitAsync(_transactionGuid);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackAsync(_transactionGuid);
            throw;
        }
    }

    private async Task UpdateRangeAsync(IList<T> entity)
    {
        try
        {
            _unitOfWork.BeginTransaction(_transactionGuid);

            this.DbSet.UpdateRange(entity);

            await _context.SaveChangesAsync();

            await _unitOfWork.CommitAsync(_transactionGuid);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackAsync(_transactionGuid);
            throw;
        }
    }
}
