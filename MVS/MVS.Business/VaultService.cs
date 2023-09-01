// <copyright file="FolderService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class VaultService : BaseService<Vault>, IVaultService
{
    public VaultService(IConfiguration configuration)
        : base(configuration)
    {
    }

    public async Task<List<Vault>> GetVaults(string userId)
    {
        try
        {
            using FolderRepository repo = new(this._configuration);

            return await repo.GetVaults(userId);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*
    public async Task<Vault> GetCurrentFolderWithCompletedStatus(string folderId)
    {
        using FolderRepository repo = new(this._configuration);
        return await repo.GetCurrentFolderWithCompletedStatus(folderId);
    }*/

    /*public async Task<IsFolderInfosResult> IsFolderInfos(string folderId)
    {
        using FolderInfosRepository repo = new(this._configuration);
        return await repo.IsFolderInfos(folderId);
    }*/

    public async Task<bool> DeleteVault(string vaultId)
    {
        using FolderRepository repo = new(this._configuration);
        return await repo.DeleteVault(vaultId);
    }
}