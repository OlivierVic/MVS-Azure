// <copyright file="BaseService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Data.Repositories;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MVS.Business;

public class AccessService<T> : IAccessService<T> where T : AccessElem
{

    public readonly IConfiguration _configuration;

    public AccessService(IConfiguration configuration) => this._configuration = configuration;

    public async Task CheckAccess(string elemId, string userId, bool isAdmin = false)
    {
        using CRUDRepository<T> repo = new(this._configuration);
        Specification<T> spec = new Specification<T>(el => el.Id == elemId);
        T elem = await repo.FirstOrDefaultAsync(spec);
        if (elem == null)
        {
            throw new ArgumentException("La donnée que vous voulez récupérer n'existe pas");
        }

        if (isAdmin)
        {
            return;
        }

        PropertyInfo propInfo = elem.GetType().GetProperty("UserId");
        if (propInfo.GetValue(elem, null).ToString() == userId)
        {
            return;
        }

        if (elem.GetType() == typeof(Vault))
        {
            using CRUDRepository<VaultUser> repository = new(this._configuration);
            List<VaultUser> ListVaultUsers = repository.List(new Specification<VaultUser>(fu => fu.VaultId == elemId));
            List<string> vaultUsersId = ListVaultUsers.Select(u => u.UserId).ToList();

            if(vaultUsersId.Any(fu => fu == userId))
            {
                return;
            }
        }

        throw new UnauthorizedAccessException("Vous n'avez pas accès à la donnée que vous voulez récupérer");
    }
}
