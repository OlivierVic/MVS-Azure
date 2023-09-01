// <copyright file="RegisterUserAdmin.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.EmailSender.Templates.Models;

public class RegisterUserAdminProcheTemplate : ITemplate
{
    public string TemplateName { get; } = "RegisterUserAdminProcheTemplate";
    public string Subject { get; init; } = "Création de votre compte Alix accompagne pour un dossier d’un de vos proches - Plateforme Alix Accompagne";
    public RegisterUserAdminProcheTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class RegisterUserAdminProcheTemplateModel
{
    public string RGPDPageUrl { get; set; }
    public string DefinePasswordUrl { get; set; }
    public string PlateformUrl { get; set; }
    public string MoreInfoPageUrl { get; set; }
}