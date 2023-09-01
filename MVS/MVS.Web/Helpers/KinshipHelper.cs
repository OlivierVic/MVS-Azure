// <copyright file="KinshipHelper.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVS.Web.Helpers;
public static class KinshipHelper
{
    public static async Task<string> KinshipText(Common.Models.VaultContact contact)
    {
        string tmpKinship = "";
        switch (contact.Kinship)
        {
            case (int)Kinship.Spouse:
                tmpKinship = "conjoint / concubin / partenaire de PACS";
                break;
            case (int)Kinship.GrandParent:
                tmpKinship = "grand-parent";
                break;
            case (int)Kinship.FatherMother:
                tmpKinship = "mère / père";
                break;
            case (int)Kinship.Child:
                tmpKinship = "enfant";
                break;
            case (int)Kinship.GrandChild:
                tmpKinship = "petit-enfant";
                break;
            case (int)Kinship.BrotherSister:
                tmpKinship = "frère/soeur";
                break;
            case (int)Kinship.OncleAunt:
                tmpKinship = "oncle / tante";
                break;
            case (int)Kinship.Nephew:
                tmpKinship = "neveu / nièce";
                break;
            case (int)Kinship.Cousin:
                tmpKinship = "cousin(e)";
                break;
            case (int)Kinship.Friend:
                tmpKinship = "ami(e)";
                break;
            case (int)Kinship.OtherFamilyTies:
                tmpKinship = "membre de la famille";
                break;
            case (int)Kinship.Other:
                tmpKinship = "personne de l'entourage";
                break;
            default:
                tmpKinship = "personne de l'entourage";
                break;
        }

        return tmpKinship;
    }
}
