// <copyright file="EmailMotionForProtectiveChange.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.EmailSender.Templates.Models;

public class ProtectedEmailMotionForProtectiveChange : ITemplate
{
    public string TemplateName { get; } = "ProtectedMailMotionForProtectiveChangeTemplate";
    public string Subject { get; init; } = "Requête de changement protecteur - Plateforme Alix Accompagne";
    public ProtectedEmailMotionForProtectiveChangeModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class ProtectedEmailMotionForProtectiveChangeModel
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string SendDate { get; set; }
    public string PlateformUrl { get; set; }
}