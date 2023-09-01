// <copyright file="IBaseService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Infrastructure.Interfaces;

namespace MVS.Common.Interfaces;

public interface IBaseService<T>
{
    Task<T> Add(T elm);
    Task<T> Get(ISpecification<T> spec);
    Task Update(T elm);
    Task<List<T>> Search(ISpecification<T> spec);
    Task<bool> Delete(T elm);
    Task<int> Count(ISpecification<T> spec);
    Task<bool> Any(ISpecification<T> spec);
}
