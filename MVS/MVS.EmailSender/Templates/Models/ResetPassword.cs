// <copyright file="TestEmailTemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public class ResetPasswordTemplate : ITemplate
{
    public string TemplateName { get; } = "ResetPasswordTemplate";
    public string Subject { get; init; } = "Modifier votre mot de passe - Plateforme Alix Accompagne";
    public ResetPasswordTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class ResetPasswordTemplateModel
{
    public string ResetPasswordUrl { get; set; }
    public string PlateformUrl { get; set; }
}