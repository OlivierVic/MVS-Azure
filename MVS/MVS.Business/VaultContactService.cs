// <copyright file="ContactService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class VaultContactService : BaseService<VaultContact>, IVaultContactService
{
    public VaultContactService(IConfiguration configuration) : base(configuration)
    {
    }
}
