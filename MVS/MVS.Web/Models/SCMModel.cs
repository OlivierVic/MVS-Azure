// <copyright file="RegisterModel.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using Smartclause.SDK.ClientModel;
using Smartclause.SDK.DTO;

namespace MVS.Web.Models;

public class Right
{
    public string Name { get; set; }

    public int Value { get; set; }

    public string Description { get; set; }
}

public class ConditionOperator
{
    public string Name { get; set; }

    public int Value { get; set; }
}

public class ContractViewModel : SmartClauseModel
{
    public string SCMUserId { get; set; }

    public List<WorkflowContractSteps> WorkflowContractStepsList { get; set; }

    public WorkflowContractStepUser UserModel { get; set; }

    public Condition ConditionModel { get; set; }

    public List<Right> RightsEnum { get; set; }

    public List<ConditionOperator> ConditionOperatorsEnum { get; set; }

    public int ContractIndex { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsCreator { get; set; }

    public string FromFolderId { get; set; }

    public string Uid { get; set; }

    public List<ContributorField> FieldsList { get; internal set; }

    public TemplateAvailableFields TemplateAvailableFields { get; set; }

    public string CancelSignError { get; set; }

    public int RightsSignature { get; set; }

    public string UserId { get; set; }

    public string Lang { get; set; }

    public string ViewerUrl { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Initials { get; set; }

    public string ReturnUrl { get; set; }

    public string CreatorEmail { get; set; }
}