// <copyright file="HeritageService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Business;
public class VaultAdministrativeLifeService : BaseService<VaultAdministrativeLife>, IVaultAdministrativeLifeService
{
    public VaultAdministrativeLifeService(IConfiguration configuration)
        : base(configuration)
    {
    }
}