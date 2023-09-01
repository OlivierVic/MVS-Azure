// <copyright file="FolderRepository.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Data.Repositories;
public class FolderRepository : CRUDRepository<Vault>
{
    public FolderRepository(IConfiguration configuration)
        : base(configuration) { }

    public async Task<List<Vault>> GetVaults(string userId)
    {
        return await this._dbContext.Vaults.FromSqlRaw("EXECUTE [dbo].[GetVaults] {0}", userId).ToListAsync();
    }
    /*
    public async Task<Vault> GetCurrentFolderWithCompletedStatus(string folderId)
    {
        List<Vault> result = await this._dbContext.Vaults.FromSqlRaw("EXECUTE [dbo].[GetCurrentFolderWithCompletedStatus] {0}", folderId).ToListAsync();
        return result.FirstOrDefault();
    }*/

    public async Task<bool> DeleteVault(string folderId)
    {
        string commandText = "EXEC [dbo].[DeleteVault] @vaultId";
        SqlParameter idParameter = new SqlParameter("@vaultId", folderId);
        this._dbContext.Database.ExecuteSqlRaw(commandText, idParameter);

        return true;
    }
}
