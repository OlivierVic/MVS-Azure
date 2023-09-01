// <copyright file="JsonHelper.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common.Enum;
using MVS.Common.Enum.FolderInfosForms;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using Aspose.Words;
using System.Linq.Expressions;

namespace MVS.Web.Helpers;

public static class VaultInfosHelper
{
    public static string MakeQuestionLineFromString(string questionTitle, string response) => $"{questionTitle} : {response} \n";

    public static byte[] BuildFolderInfosPdf(Vault folder, string questions, string title)
    {
        Aspose.Words.Document doc = new();
        DocumentBuilder builder = new(doc);

        //Create Title
        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        builder.Font.Size = 20;
        builder.Writeln($"{title}");

        //Create Name
        builder.Font.Size = 16;
        builder.Writeln($"{folder.FirstName} {folder.LastName}");

        builder.Write("\n\n");

        //WriteQuestion
        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        builder.Font.Size = 12;
        builder.Writeln(questions);

        MemoryStream stream = new();
        doc.Save(stream, SaveFormat.Pdf);

        return stream.ToArray();
    }

    public static Dictionary<string, string> GetFolderInfoHeader(Vault vault)
    {
        Dictionary<string, string> folderInfoHeader = new Dictionary<string, string>();

        folderInfoHeader.Add("Title", $"Coffre-fort n° {vault.Title}"); // mettre "Dossier n°" apres avoir modifier la création de dossier (Title)
        folderInfoHeader.Add("Name", $"{vault.FirstName + ' ' + vault.LastName}");
        folderInfoHeader.Add("CreationDate", $"{vault.CreationDate.ToString("dd/MM/yyyy")}");
        folderInfoHeader.Add("Id", $"{vault.Id}");

        return folderInfoHeader;
    }
}
