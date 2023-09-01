// <copyright file="TypeMissionEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVS.Common.Enum;
public enum TypeMission
{
    [Description("Aucune")]
    Default = 0, //Valeur par default
    [Description("Protecteur (ex: habilité, curateur, tuteur)")]
    Protecteur = 1,
    [Description("Contrôleur (ex: subrogé-curateur)")]
    Controleur = 2,
    [Description("Aucune")]
    Aucune = 3,
    [Description("Observateur")]
    Observateur = 4,
    [Description("Remplaçant Contrôleur")]
    ReplaceControleur = 5,
    [Description("Remplaçant Mandataire")]
    ReplaceMandataire = 6,
}

