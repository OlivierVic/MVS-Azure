// <copyright file="FolderService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class VaultContractService : BaseService<VaultContract>, IVaultContractService
{
    public VaultContractService(IConfiguration configuration)
        : base(configuration)
    {
    }
}