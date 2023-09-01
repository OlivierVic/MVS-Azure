// <copyright file="IBaseService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Infrastructure.Interfaces;

namespace MVS.Common.Interfaces;

public interface IAccessService<T>
{
    Task CheckAccess(string elemId, string userId, bool isSuperAdmin = false);
}
