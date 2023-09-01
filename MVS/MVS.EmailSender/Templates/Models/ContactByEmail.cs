// <copyright file="TestEmailTemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public class ContactByEmailTemplate : ITemplate
{
    public string TemplateName { get; } = "ContactByEmailTemplate";
    public string Subject { get; init; }
    public ContactByEmailTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class ContactByEmailTemplateModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string FolderNumber { get; set; }
    public string Question { get; set; }
    public string PlateformUrl { get; set; }
}