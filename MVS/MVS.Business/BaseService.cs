// <copyright file="BaseService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Infrastructure.Interfaces;
using MVS.Common.Interfaces;
using MVS.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class BaseService<T> : IBaseService<T> where T : class
{

    public readonly IConfiguration _configuration;

    public BaseService(IConfiguration configuration) => this._configuration = configuration;

    public async Task<T> Add(T elm)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        return await repo.AddAsync(elm);
    }

    public async Task<T> Get(ISpecification<T> spec)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        return await repo.FirstOrDefaultAsync(spec);
    }

    public async Task<List<T>> Search(ISpecification<T> spec)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        return await repo.ListAsync(spec);
    }

    public async Task Update(T elm)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        await repo.UpdateAsync(elm);
    }

    public async Task<bool> Delete(T elm)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        try
        {
            await repo.DeleteAsync(elm);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<int> Count(ISpecification<T> spec)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        return await repo.CountAsync(spec);
    }

    public async Task<bool> Any(ISpecification<T> spec) => await this.Count(spec) > 0;
}
