// <copyright file="MissionAgentEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Common.Enum;
public enum MissionAgent
{
    [Description("Aucune")]
    Default = 0, //Valeur par default
    [Description("Protection de la personne")]
    ProtectPeople = 1,
    [Description("Protection de tous les biens")]
    ProtectAllProperty = 2,
    [Description("Protection de certains biens")]
    ProtectOfCertainGoods = 3,
    [Description("Protection de la  personne et de certains biens")]
    ProtectOfCertainGoodsPeople = 4,
    [Description("Protection de la  personne et de tous les biens")]
    ProtectAllPropertyPeople = 5,
}