// <copyright file="CategoryService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class VaultCategoryService : BaseService<VaultCategory>, IVaultCategoryService
{
    public VaultCategoryService(IConfiguration configuration) : base(configuration)
    {
    }
}
