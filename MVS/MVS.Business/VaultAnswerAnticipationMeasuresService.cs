// <copyright file="FolderService.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Interfaces;
using MVS.Common.Models;
using Microsoft.Extensions.Configuration;

namespace MVS.Business;

public class VaultAnswerAnticipationMeasuresService : BaseService<VaultAnswersAnticipationMeasure>, IVaultAnswerAnticipationMeasuresService
{
    public VaultAnswerAnticipationMeasuresService(IConfiguration configuration)
        : base(configuration)
    {
    }
}