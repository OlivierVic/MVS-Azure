// <copyright file="FolderFieldEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace MVS.Common.Enum;

public enum FolderStatus
{
    [Description("En attente de signature")]
    PendingSignature = 0,
    [Description("En attente de paiement")]
    PendingPayment = 1,
    [Description("Collecte")]
    Collect = 2,
    [Description("Collecte terminé")]
    CollectFinish = 3,
    [Description("Evaluation en cours")]
    EvaluationInProgress = 4,
    [Description("Rédaction en cours")]
    WritingInProgress = 5,
    [Description("Signature en cours")]
    SignatureInProgress = 6,
    [Description("Enregistrement en cours")]
    RecordingInProgress = 7,
    [Description("Envoi au juge")]
    SendingJudge = 8,
    [Description("Dossier complet")]
    FolderCompleted = 9,
    [Description("Dossier envoyé au juge")]
    SendJudge = 10,
    [Description("Dossier terminé")]
    FolderFinish = 11,
}
