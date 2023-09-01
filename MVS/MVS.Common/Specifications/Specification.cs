// <copyright file="Specification.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.ClassExtensions;
using MVS.Common.Infrastructure.Interfaces;
using System.Linq.Expressions;
namespace MVS.Common.Specifications;

public class Specification<T> : ISpecification<T>
{
    public Specification(Expression<Func<T, bool>> criteria) => this.Criteria = criteria;

    public Expression<Func<T, bool>> Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public Expression<Func<T, object>> GroupBy { get; private set; }

    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    public virtual void ApplyPaging(int skip, int take)
    {
        this.Skip = skip;
        this.Take = take;
        this.IsPagingEnabled = true;
    }

    public virtual void NoPaging() => this.IsPagingEnabled = false;

    public virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) => this.OrderBy = orderByExpression;

    public virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => this.OrderByDescending = orderByDescendingExpression;

    public virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) => this.GroupBy = groupByExpression;

    public virtual void AndAlso(Expression<Func<T, bool>> andAlsoExpression) => this.Criteria = this.Criteria.AndAlso(andAlsoExpression);
}
