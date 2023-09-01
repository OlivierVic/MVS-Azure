const workflowStepStatus = {
    TRUE: "true",
    FALSE: "false",
    PARTIALLY: "partially"
};

/**
 * Workflow Panel View Model
 * @constructor allowEditing (optional): tells if we want to display user step validation, default is false
 * @constructor fields (optional): fields available for a contract
 * @constructor rights (optional): rights available for a contract
 */
function WorkflowPanelViewModel(allowEditing, fields, rights) {
    const self = this;

    /**
     * Attributes
     */
    self.loader = new LoaderViewModel();
    self.contractId = ko.observable(null);
    self.contractName = ko.observable(null);
    self.contractLoaded = ko.observable(false);
    self.workflowSteps = ko.observable([]);
    self.currentWorkflowStep = ko.observable(null);
    self.allowEditing = allowEditing || false;
    self.fields = fields || [];
    self.rights = rights || [];
    self.contractWorkflow = ko.observable(null);
    self.nextWorkflowStepName = ko.observable(null);
    self.nextWorkFlowStep = ko.observable(null);
    self.addContributorModalViewModel = ko.observable();

    /**
     * Methods
     */
    self.loadData = function (scmContractId) {
        self.loader.switchLoading();
        $.ajax({
            url: `/SCM?handler=GetContractJson`,
            method: 'GET',
            data: {
                scmContractId
            },
            success: function (data) {
                self.contractId(data.id);
                self.contractName(data.title);
                var workflowContractSteps = data.workflowContractSteps;
                self.workflowSteps(workflowContractSteps.sort(function (x, y) {
                    return x.stepOrder - y.stepOrder;
                }));

                if (data.contractWorkflows.length === 0) {
                    self.currentWorkflowStep(0);
                    self.nextWorkflowStepName('END');
                    self.nextWorkFlowStep({ workflowContractStepUsers: ko.observableArray([]) });
                } else {
                    self.currentWorkflowStep(data.contractWorkflows[0].step);
                    if (workflowContractSteps.length > data.contractWorkflows[0].step + 1) {
                        self.nextWorkflowStepName(self.workflowSteps()[data.contractWorkflows[0].step + 1].name);
                        self.nextWorkFlowStep(ko.mapping.fromJS(self.workflowSteps()[data.contractWorkflows[0].step + 1]));
                    } else {
                        self.nextWorkflowStepName('END');
                        self.nextWorkFlowStep({ workflowContractStepUsers: ko.observableArray([]) });
                    }
                }
            },
            error: function (err) {
                console.error(err);
                toastr.error("Une erreur est survenue lors du chargement du workflow");
            },
            complete: function () {
                self.loader.switchLoading();
                self.contractLoaded(true);
            }
        })
    }

    self.isStepOver = function (data) {
        var status = workflowStepStatus.TRUE;
        for (var i = 0; i < data.workflowContractStepUsers.length; i++) {
            if (!data.workflowContractStepUsers[i].validated) {
                if (!data.workflowContractStepUsers[i].optional) {
                    status = workflowStepStatus.FALSE;
                } else if (status !== workflowStepStatus.FALSE) {
                    status = workflowStepStatus.PARTIALLY;
                }
            }

        }
        return status;
    }

    self.showNextStepModal = function () {
        ViewMod.showNextStepModal();
    };

    self.showNextStepModalWithOptionalNotValidated = function () {
        ViewMod.showNextStepModalWithOptionalNotValidated();
    };

    self.showAddContributorModal = () => {
        if (self.allowEditing) {
            self.addContributorModalViewModel(new AddContributorModalViewModel(
                self.fields,
                self.rights,
                self.contractId(),
                self.contractName(),
                self.currentWorkflowStep(),
                self.workflowSteps().length
            )
            );
        }
    }

    self.showNextStepBtn = ko.pureComputed(function () {
        return self.allowEditing
            && ViewMod.currentWorkflowStep() != null
            && !ViewMod.currentWorkflowStep().workflowStepUsers().some((stepUser) => stepUser.rights() === ViewMod.rightSignature)
    });

    workflowPanelViewModel = self;
}
