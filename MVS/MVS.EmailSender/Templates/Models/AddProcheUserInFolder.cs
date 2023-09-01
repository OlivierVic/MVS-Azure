// <copyright file="RegisterUserAdmin.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.EmailSender.Templates.Models;

public class AddProcheUserInFolderTemplate : ITemplate
{
    public string TemplateName { get; } = "AddProcheUserInFolderTemplate";
    public string Subject { get; init; } = "Désignation comme administrateur d’un dossier de protection d’un de vos proches - Plateforme Alix Accompagne";
    public AddProcheUserInFolderTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class AddProcheUserInFolderTemplateModel
{
    public string ConnectionPageUrl { get; set; }
    public string RGPDPageUrl { get; set; }
    public string PlateformUrl { get; set; }
}