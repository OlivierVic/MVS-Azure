// <copyright file="RelationshipFrequenciesEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Common.Enum;
public enum RelationshipFrequencies
{
    [Description("Toutes les semaines")]
    EveryWeek = 0,
    [Description("Tous les mois")]
    EveryMonth = 1,
    [Description("Plusieurs fois par an")]
    SeveralTimesAYear = 2,
    [Description("Moins d'une fois par an")]
    LessThanOnceAYear = 3,
    [Description("Null")]
    NULL = 4,
}