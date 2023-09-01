// <copyright file="TestEmailTemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public class RegisterTemplate : ITemplate
{
    public string TemplateName { get; } = "RegisterTemplate";
    public string Subject { get; init; } = "Vérification Adresse Email - Plateforme Alix Accompagne";
    public RegisterTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class RegisterTemplateModel
{
    public string Name { get; set; }
    public string ValidEmailUrl { get; set; }
    public string PlateformUrl { get; set; }
}