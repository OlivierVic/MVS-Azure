// <copyright file="ITemplate.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.EmailSender.Templates.Models;

public interface ITemplate
{
    public string TemplateName { get; }
    public string Subject { get; init; }
    public object GetModel();
}
