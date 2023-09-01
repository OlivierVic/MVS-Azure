// <copyright file="TestEmailTemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public class ContactAdviceTemplate : ITemplate
{
    public string TemplateName { get; } = "ContactAdviceTemplate";
    public string Subject { get; init; } = "Avis entourage";
    public ContactAdviceTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class ContactAdviceTemplateModel
{
    public string FullName { get; set; }
    public string FullNameRequester { get; set; }
    public string Identity { get; set; }
    public string Situation { get; set; }
    public string FormUrl { get; set; }
    public string PlateformUrl { get; set; }
}