// <copyright file="TestEmailTemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public class DoctorRequestTemplate : ITemplate
{
    public string TemplateName { get; } = "DoctorRequestTemplate";
    public string Subject { get; init; } = "Demande";
    public DoctorRequestTemplateModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class DoctorRequestTemplateModel
{
    public string Measure { get; set; }
    public string DoctorName { get; set; }
    public string FullName { get; set; }
    public string PlateformUrl { get; set; }
}