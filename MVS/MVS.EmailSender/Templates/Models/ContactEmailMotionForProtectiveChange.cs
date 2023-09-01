// <copyright file="EmailMotionForProtectiveChange.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.EmailSender.Templates.Models;

public class ContactEmailMotionForProtectiveChange : ITemplate
{
    public string TemplateName { get; } = "ContactMailMotionForProtectiveChangeTemplate";
    public string Subject { get; init; } = "Requête de changement protecteur - Plateforme Alix Accompagne";
    public ContactEmailMotionForProtectiveChangeModel Model { get; init; }
    public object GetModel() => this.Model;
}

public class ContactEmailMotionForProtectiveChangeModel
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string SendDate { get; set; }
    public string NameFolder { get; set; }
    public string PlateformUrl { get; set; }
}