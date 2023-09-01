// <copyright file="CRUDRepository.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Infrastructure.Interfaces;
using MVS.Common.Models;
using MVS.Data.Infrastructure;
using MVS.Data.Infrastructure.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MVS.Data.Repositories;

public class FolderInfosRepository : CRUDRepository<Vault>
{
    public FolderInfosRepository(IConfiguration configuration) :
        base(configuration)
    {
    }

    /*public async Task<IsFolderInfosResult> IsFolderInfos(string folderId)
    {
        List<IsFolderInfosResult> results = await this._dbContext.IsFolderInfosResult.FromSqlRaw("EXECUTE [dbo].[IsFolderInfos] {0}", folderId).ToListAsync();
        return results.FirstOrDefault();
    }*/
}

//Voir si utile