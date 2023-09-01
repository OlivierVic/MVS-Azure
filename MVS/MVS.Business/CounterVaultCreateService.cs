// <copyright file="CounterFolderCreateService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class CounterVaultCreateService : BaseService<CounterVaultCreate>, ICounterVaultCreateService
{
    public CounterVaultCreateService(IConfiguration configuration)
        : base(configuration)
    {
    }
}