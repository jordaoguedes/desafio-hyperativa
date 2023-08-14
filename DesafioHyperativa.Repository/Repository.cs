using DesafioHyperativa.Entities;
using DesafioHyperativa.Entities.Base;
using DesafioHyperativa.Repository.Context;
using DesafioHyperativa.Repository.Contracts;
using DesafioHyperativa.Repository.CrossCutting.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DesafioHyperativa.Repository;

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
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
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

            if (entity.Id == 0)
            {
                await DbSet.AddAsync(entity);
            }
            else
            {
                entity.DataUpdate = DateTime.Now;
                DbSet.Update(entity);
            }
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

            DbSet.AddRange(entity);

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

            foreach (var item in entity) 
            { 
                item.DataUpdate = DateTime.Now;
            }
            DbSet.UpdateRange(entity);

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
