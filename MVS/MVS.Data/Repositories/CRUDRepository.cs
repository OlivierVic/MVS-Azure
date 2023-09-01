// <copyright file="CRUDRepository.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Infrastructure.Interfaces;
using MVS.Common.Models;
using MVS.Data.Infrastructure;
using MVS.Data.Infrastructure.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MVS.Data.Repositories;

public class CRUDRepository<T> : IDisposable, IAsyncRepository<T> where T : class
{
    protected readonly mvsrecetteContext _dbContext;
    public CRUDRepository(IConfiguration configuration)
    {
        this._dbContext = new mvsrecetteContext(new DbContextOptions<mvsrecetteContext>(), configuration);
    }

    public CRUDRepository(mvsrecetteContext context)
    {
        this._dbContext = context;
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await this._dbContext.Set<T>().FindAsync(id);
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        return await this._dbContext.Set<T>().FindAsync(id);
    }

    public virtual T GetById(int id)
    {
        return this._dbContext.Set<T>().Find(id);
    }

    public virtual T GetById(string id)
    {
        return this._dbContext.Set<T>().Find(id);
    }

    public async Task<List<T>> ListAllAsync()
    {
        return await this._dbContext.Set<T>().ToListAsync();
    }

    public List<T> ListAll()
    {
        return this._dbContext.Set<T>().ToList();
    }

    public async Task<List<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public List<T> List(ISpecification<T> spec)
    {
        return ApplySpecification(spec).ToList();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    public int Count(ISpecification<T> spec)
    {
        return ApplySpecification(spec).Count();
    }

    public async Task<T> AddAsync(T entity)
    {
        await this._dbContext.Set<T>().AddAsync(entity);
        await this._dbContext.SaveChangesAsync();
        return entity;
    }

    public T Add(T entity)
    {
        this._dbContext.Set<T>().Add(entity);
        this._dbContext.SaveChanges();

        return entity;
    }

    public async Task<IList<T>> AddBulkAsync(IList<T> entities)
    {
        await this._dbContext.BulkInsertAsync(entities);
        return entities;
    }

    public IList<T> AddBulk(IList<T> entities)
    {
        this._dbContext.BulkInsert(entities);
        return entities;
    }

    public async Task UpdateAsync(T entity)
    {
        this._dbContext.Entry(entity).State = EntityState.Modified;
        await this._dbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        this._dbContext.Entry(entity).State = EntityState.Modified;
        this._dbContext.SaveChanges();
    }

    public async Task UpdateBulkAsync(IList<T> entities)
    {
        await this._dbContext.BulkUpdateAsync(entities);
    }

    public void UpdateBulk(IList<T> entities)
    {
        this._dbContext.BulkUpdate(entities);
    }

    public async Task DeleteAsync(T entity)
    {
        this._dbContext.Set<T>().Remove(entity);
        await this._dbContext.SaveChangesAsync();
    }

    public void Delete(T entity)
    {
        this._dbContext.Set<T>().Remove(entity);
        this._dbContext.SaveChanges();
    }

    public async Task DeleteBulkAsync(IList<T> entities)
    {
        await this._dbContext.BulkDeleteAsync(entities);
    }

    public void DeleteBulk(IList<T> entities)
    {
        this._dbContext.BulkDelete(entities);
    }

    public async Task<T> FirstAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstAsync();
    }

    public T First(ISpecification<T> spec)
    {
        return ApplySpecification(spec).First();
    }

    public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public T FirstOrDefault(ISpecification<T> spec)
    {
        return ApplySpecification(spec).FirstOrDefault();
    }

    public async Task<T> LastOrDefaultAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).LastOrDefaultAsync();
    }

    public T LastOrDefault(ISpecification<T> spec)
    {
        return ApplySpecification(spec).LastOrDefault();

    }

    public async Task<int> BatchDeleteAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).BatchDeleteAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(this._dbContext.Set<T>().AsQueryable(), spec);
    }

    public async Task DeleteClientAsync(string clientId)
    {
        var parameter = new List<SqlParameter>();
        parameter.Add(new SqlParameter("@clientId", clientId));
        await this._dbContext.Database.ExecuteSqlRawAsync(@"SET NOCOUNT ON exec sp_DeleteClient @clientId", parameter.ToArray());
    }

    public async Task DeleteProjectAsync(string projectId)
    {
        var parameter = new List<SqlParameter>();
        parameter.Add(new SqlParameter("@projectId", projectId));
        await this._dbContext.Database.ExecuteSqlRawAsync(@"SET NOCOUNT ON exec sp_DeleteProject @projectId", parameter.ToArray());
    }

    public async Task DeleteMatrixOperationType(string operationTypeId)
    {
        var parameter = new List<SqlParameter>();
        parameter.Add(new SqlParameter("@operationTypeId", operationTypeId));
        await this._dbContext.Database.ExecuteSqlRawAsync(@"SET NOCOUNT ON exec sp_DeleteMatrixOperationType @operationTypeId", parameter.ToArray());
    }

    public void Dispose()
    {
        this._dbContext.Dispose();
    }
}
