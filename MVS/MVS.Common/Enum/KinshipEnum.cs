// <copyright file="KinshipEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Common.Enum;
public enum Kinship
{
    [Description("Conjoint / Concubin / Partenaire de PACS")]
    Spouse = 0,
    [Description("Grand-parent")]
    GrandParent = 1,
    [Description("Père / Mère")]
    FatherMother = 2,
    [Description("Enfant")]
    Child = 3,
    [Description("Petit-enfant")]
    GrandChild = 4,
    [Description("Frère / Soeur")]
    BrotherSister = 5,
    [Description("Oncle / Tante")]
    OncleAunt = 6,
    [Description("Neveu / Nièce")]
    Nephew = 7,
    [Description("Cousin / Cousine")]
    Cousin = 8,
    [Description("Ami(e)")]
    Friend = 9,
    [Description("Autre lien de famille")]
    OtherFamilyTies = 10,
    [Description("Autre")]
    Other = 11,
}