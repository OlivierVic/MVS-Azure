// <copyright file="IAsyncRepository.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Infrastructure.Interfaces;

namespace MVS.Data.Infrastructure.Interfaces;

public interface IAsyncRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> ListAllAsync();
    Task<List<T>> ListAsync(ISpecification<T> spec);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> CountAsync(ISpecification<T> spec);
    Task<T> FirstAsync(ISpecification<T> spec);
    Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
}