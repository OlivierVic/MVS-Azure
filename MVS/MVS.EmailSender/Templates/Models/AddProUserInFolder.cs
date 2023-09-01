// <copyright file="RegisterUserAdmin.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.EmailSender.Templates.Models;

public class AddProUserInFolderTemplate : ITemplate
{
    public string TemplateName { get; } = "AddProUserInFolderTemplate";
    public string Subject { get; init; } = "Désignation comme administrateur d’un dossier de protection d’un de vos bénéficiaires - Plateforme Alix Accompagne";
    public AddProUserInFolderTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class AddProUserInFolderTemplateModel
{
    public string ConnectionPageUrl { get; set; }
    public string RGPDPageUrl { get; set; }
    public string PlateformUrl { get; set; }
}