// <copyright file="TestEmailTemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public class MailTierTemplate : ITemplate
{
    public string TemplateName { get; } = "MailTierTemplate";
    public string Subject { get; init; } = "Information de la rédaction du mandat de protection juridique";
    public MailTierTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class MailTierTemplateModel
{
    public string FullName { get; set; }
    public string PlateformUrl { get; set; }
}