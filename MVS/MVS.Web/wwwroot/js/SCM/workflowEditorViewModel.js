function WorkflowEditorViewModel(workflow, rights, conditionOperators, fields, userModel, conditionModel, availableFields, isTemplate, saveModificationsFunction) {
    var self = this;

    self.displayInfo = isTemplate || false;


    self.workflow = ko.mapping.fromJS(workflow);
    // Reorder based on step order
    self.workflow.workflowSteps.sort((x, y) => x.stepOrder() - y.stepOrder());
    self.baseModel = ko.mapping.toJS(self.workflow);
    self.currentlySelectedStepIndex = ko.observable(0);
    self.actionList = rights;
    self.isSaving = ko.observable(false);
    self.actions = ko.pureComputed(function () {
        if (self.currentlySelectedStepIndex() == self.workflow.workflowSteps().length - 1) {
            return self.actionList;
        }
        else {
            var newArray = [...self.actionList];
            //newArray.pop();
            return newArray.slice(0, 4);
        }
    });
    //self.actions = rights;


    self.conditionOperators = conditionOperators;
    self.fields = fields;
    self.templateAvailableFields = availableFields ?? [];
    self.templateAvailableFields.unshift({
        label: CONTRIBUTOR_RESOURCES.ChooseOne,
        write: null,
    })
    self.stepModel = {
        builtinOption: null,
        children: null,
        conditionRootId: null,
        contractId: null,
        description: null,
        endDateTime: null,
        id: null,
        name: null,
        optional: true,
        parent: null,
        parentId: null,
        startDateTime: null,
        stepOrder: null,
        workflowStepUsers: [],
        workflowTemplateId: null,
    };
    self.userModel = userModel;

    /**
     * Array of object
     * {
     *     stepId: the step where external need to be filled
     *     stepUser: the user option (in workflowStepUsers)
     * }
     */
    self.externalContributorsToFill = ko.observableArray([]);
    self.shouldDisplayExternalFilling = function (stepUserData) {
        var currentStepId = self.selectedStep().id();
        var needFill = self.externalContributorsToFill().find(x => x.stepId === currentStepId && x.stepUser === stepUserData);
        return needFill !== undefined && needFill !== null;
    }

    /**
     * New external user related
     */
    self.externalContributorFirstname = ko.observable("");
    self.externalContributorLastname = ko.observable("");
    self.externalContributorEmail = ko.observable("");
    self.currentExternalContributorFilling = ko.observable();
    self.fillExternalInformationClick = function (stepUserData) {
        var currentStepId = self.selectedStep().id();
        var currentExternalContributor = self.externalContributorsToFill().find(x => x.stepId === currentStepId && x.stepUser === stepUserData);
        self.currentExternalContributorFilling(currentExternalContributor)
        $("#fillexternalcontributor-modal").modal("toggle");
    }
    self.onAddExternalContributorClick = function () {
        var selectedStepId = self.selectedStep().id();
        var workflowStepUser = self.currentExternalContributorFilling().stepUser;

        workflowStepUser.firstname(self.externalContributorFirstname());
        workflowStepUser.lastname(self.externalContributorLastname());
        workflowStepUser.email(self.externalContributorEmail());
        workflowStepUser.fieldId(self.fields.find(e => e.title === 'Extern').id);

        self.externalContributorsToFill.remove(x => x.stepId === selectedStepId && x.stepUser === workflowStepUser);

        self.externalContributorFirstname("");
        self.externalContributorLastname("");
        self.externalContributorEmail("");
        $("#fillexternalcontributor-modal").modal("toggle");
    }

    self.canMoveStep = function (stepOrder) {
        // Case of contract
        if (self.workflow.currentStep) {
            return stepOrder >= self.workflow.currentStep() && self.workflow.workflowSteps()[stepOrder] && !self.workflow.workflowSteps()[stepOrder].workflowStepUsers().find(c => c.rights() === 3);
        }
        // Case of template
        return stepOrder !== 0 && !self.workflow.workflowSteps()[stepOrder].workflowStepUsers().find(c => c.rights() === 3);
    }

    self.canEditStep = function (step) {
        // Case of contract
        if (self.workflow.currentStep) {
            return step.stepOrder() >= self.workflow.currentStep();
        }
        // Case of template
        return step.builtinOption() !== 0;
    }

    self.canEditMandatory = function () {
        return isTemplate;
    }

    self.canEditMandatoryContributor = function () {
        return isTemplate;
    }

    self.isAuthor = function (str) {
        if (isTemplate) {
            return true;
        }
        else {
            return workflowPanelViewModel.workflowSteps()[0].workflowContractStepUsers[0].email == str;
        }
    }

    self.getUserInitials = function (data) {
        return (data.firstname() ?? ' ')[0] + ' ' + (data.lastname() && data.lastname().length > 1 ? data.lastname()[0] : ' ');
    }

    self.getUserDisplayName = function (data, event) {
        return (data.firstname() ?? '') + ' ' + (data.lastname() ?? '');
    }

    self.userInitials = function (data) {
        var users = '';
        var userList = data();
        for (var i = 0; i < userList.length; i++) {
            if (i > 0) {
                users += ', ';
            }
            var tmp = (userList[i].firstname() ?? '') + ' ' + (userList[i].lastname() ?? '');
            if (tmp.trim())
                users += tmp.trim();
            else {
                var fieldId = ko.unwrap(userList[i].fieldId);
                if (!fieldId) {
                    fieldId = self.fields[0].id;
                }
                var fieldTitle = self.fields.find(e => e.id === fieldId).title;
                users += fieldTitle;
            }
        }
        var sp = users.split(' ');
        if (sp.length > 1) {
            return sp[0][0] + sp[1][0];
        }
        else {
            return sp[0][0];
        }
    }

    self.userListWrapper = function (data) {
        var users = '';
        var userList = data();
        for (var i = 0; i < userList.length; i++) {
            if (i > 0) {
                users += ', ';
            }
            var tmp = (userList[i].firstname() ?? '') + ' ' + (userList[i].lastname() ?? '');
            if (tmp.trim())
                users += tmp.trim();
            else {
                var fieldId = ko.unwrap(userList[i].fieldId);
                if (!fieldId) {
                    fieldId = self.fields[0].id;
                }
                var fieldTitle = self.fields.find(e => e.id === fieldId).title;
                users += fieldTitle;
            }
        }

        return users;
    }

    self.getAvailableUsers = function (searchTerm, callback) {
        $.ajax({
            dataType: "json",
            url: "/Search/SearchUsersForWorkflowStep",
            data: {
                query: searchTerm,
                // field: self.fields.find(f => f.id === this.fieldId()).title,
                withAuthor: true
            },
        }).done(callback);
    }

    self.userIndex = ko.observable(0);
    self.userOtherIndex = ko.observable(0);

    self.contributorType = ko.observable();

    // Add the selected user to mandatory user list
    self.setUser = function (event, data) {
        // should display add new user popup
        if (data.item.data.email == "@Add") {
            $("#selectcontributortype-modal").modal('toggle');
            self.contributorType('mandatory');
        }
        else {
            var stepUser = self.selectedStep().workflowStepUsers()[self.userIndex()];
            stepUser.email(data.item.data.email);
            stepUser.lastname(data.item.data.lastName);
            stepUser.firstname(data.item.data.firstName);

            if (data.item.data.email === "@Author") {
                var authorFieldId = self.fields.find(e => e.title === "Author").id;
                self.selectedStep().workflowStepUsers()[self.userIndex()].fieldId(authorFieldId);
            }
        }
    }

    // Add the selected user to the optional user list
    self.setOtherUser = function (event, data) {
        var stepUser = self.selectedStep().workflowStepUsers()[self.userOtherIndex()];

        // should display add new user popup
        if (data.item.data.email == "@Add") {
            $("#selectcontributortype-modal").modal('toggle');
            self.contributorType('other');
        }
        else {

            stepUser.email(data.item.data.email);
            stepUser.lastname(data.item.data.lastName);
            stepUser.firstname(data.item.data.firstName);

            if (data.item.data.email === "@Author") {
                var authorFieldId = self.fields.find(e => e.title === "Author").id;
                self.selectedStep().workflowStepUsers()[self.userOtherIndex()].fieldId(authorFieldId);
            }
        }
    }


    self.addInternalUser = function (data, event) {
        toastr.info("L'ajout d'un utilisateur interne se fait directement dans l'administration");
    }

    self.addExternalUser = function (data, event) {
        $("#selectcontributortype-modal").modal('toggle');
        if (self.contributorType() == 'other') {
            var selectedStep = self.selectedStep();
            var workflowStepUser = selectedStep.workflowStepUsers()[self.userOtherIndex()];
            self.currentExternalContributorFilling({
                stepId: selectedStep.id(),
                stepUser: workflowStepUser
            });
        }
        $("#fillexternalcontributor-modal").modal("toggle");
    }

    self.setCurrentUserStep = function (data, event) {
        self.userIndex($(event.currentTarget).data("id"));
    }

    self.setOtherCurrentUserStep = function (data, event) {
        self.userOtherIndex($(event.currentTarget).data("id"));
    }

    self.isTemplate = isTemplate;

    self.canAddStep = function (data, event) {

        for (var i = 0; i < self.workflow.workflowSteps().length; i++) {
            for (var j = 0; j < self.workflow.workflowSteps()[i].workflowStepUsers().length; j++) {
                if (self.workflow.workflowSteps()[i].workflowStepUsers()[j].rights() == 3)
                    return false;
            }
        }
        return true;
    }

    self.addStep = function (data, event) {
        if (self.canAddStep(data, event)) {
            var stepModel = Object.assign({}, self.stepModel);
            stepModel.stepOrder = self.workflow.workflowSteps().length - 1;
            console.log(stepModel);
            self.workflow.workflowSteps.push(ko.mapping.fromJS(stepModel));
            self.currentlySelectedStepIndex(self.workflow.workflowSteps().length - 1);
            $("#sortable").sortable("refresh");
        }
        else {
            var stepModel = Object.assign({}, self.stepModel);
            stepModel.stepOrder = self.workflow.workflowSteps().length - 1;
            console.log(stepModel);
            self.workflow.workflowSteps()[self.workflow.workflowSteps().length - 1].stepOrder(self.workflow.workflowSteps()[self.workflow.workflowSteps().length - 1].stepOrder() + 1);
            self.workflow.workflowSteps.splice(self.workflow.workflowSteps().length - 1, 0, ko.mapping.fromJS(stepModel));
            self.currentlySelectedStepIndex(self.workflow.workflowSteps().length - 2);
            $("#sortable").sortable("refresh");
        }
    }


    self.canDeleteStep = function (data, event) {

        $("#removeworkflow-modal").modal('show');
        var index = self.workflow.workflowSteps.indexOf(data);
        $("#removeworkflow-modal").attr('data-workflow-step', data.id() ?? ("new_" + index));

    }

    self.deleteStep = function (data, event) {

        $("#removeworkflow-modal").modal('hide');
        var stepId = $("#removeworkflow-modal").attr('data-workflow-step');
        if (stepId.indexOf("new_") > -1) {
            var index = stepId.split("new_")[1];
            data = self.workflow.workflowSteps()[index];
        }
        else {
            data = self.workflow.workflowSteps().filter(e => e.id() === stepId)[0];
        }


        var index = self.workflow.workflowSteps.indexOf(data);
        // self.workflow.workflowSteps.remove(data);
        $(".ui-sortable-handle.active").remove();
        self.workflow.workflowSteps.splice(index, 1);
        if (index < self.workflow.workflowSteps().length) {
            self.currentlySelectedStepIndex(index);
        }
        else {
            self.currentlySelectedStepIndex(index - 1);
        }
    }

    self.selectedStep = ko.computed(function () {
        return self.workflow.workflowSteps()[self.currentlySelectedStepIndex()];
    })

    self.isSelectedStepMandatory = ko.computed({
        read: function () {
            var isOptional = self.selectedStep()?.optional();
            return isOptional === null ? false : !isOptional;
        },
        write: function (newValue) {
            self.selectedStep().optional(!newValue);
        }
    });


    self.selectStep = function (data, event) {
        var index = self.workflow.workflowSteps.indexOf(data);
        self.currentlySelectedStepIndex(index);
    }
    self.addContributor = function (data, event) {
        var newUser = ko.mapping.fromJS(Object.assign({}, self.userModel));
        newUser.optional(false);
        newUser.email("");
        self.selectedStep().workflowStepUsers.push(newUser);
    }
    self.addOtherContributor = function (data, event) {
        var newUser = ko.mapping.fromJS(Object.assign({}, self.userModel));
        newUser.optional(true);
        newUser.email("");
        self.selectedStep().workflowStepUsers.push(newUser);
    }

    self.deleteUserFromStep = function (data, event) {

        $("#removecontributor-modal #contributorFirstName").text(ko.unwrap(data.firstname));
        $("#removecontributor-modal #contributorLastName").text(ko.unwrap(data.lastname));
        $("#removecontributor-modal").modal('show');

        var ev = $._data(this, 'events');

        if (typeof ev === "undefined" || !ev.click) {
            $("#remove-contributor-button").click(function () {
                self.selectedStep().workflowStepUsers.remove(data);
                var selectedStepId = self.selectedStep().id();
                // Remove from list of external indexes
                self.externalContributorsToFill.remove(x => x.stepId === selectedStepId && x.stepUser === data);
                $("#removecontributor-modal").modal('hide');
            });
        }
    }


    self.changeField = function (data, event) {
        var index = $(event.target).attr("data-id");
        if (index) {
            var fieldId = data.fieldId();
            var fieldTitle = self.fields.find(e => e.id === fieldId).title;
            var selectedStep = self.selectedStep();
            var workflowStepUser = selectedStep.workflowStepUsers()[index];
            if (fieldTitle === "Author" || fieldTitle === "Auteur") {
                workflowStepUser.firstname("Auteur");
                workflowStepUser.lastname("");
                workflowStepUser.email("@Author");
                self.externalContributorsToFill.remove(x => x.stepId === selectedStep.id() && x.stepUser === workflowStepUser);
            } else if (["Extern"].includes(fieldTitle)) {
                self.externalContributorsToFill.push({
                    stepId: selectedStep.id(),
                    stepUser: workflowStepUser
                });
            } else {
                workflowStepUser.firstname("");
                workflowStepUser.lastname("");
                workflowStepUser.email("");
                self.externalContributorsToFill.remove(x => x.stepId === selectedStep.id() && x.stepUser === workflowStepUser);
            }
        }
    }

    /*** User Conditions ****/

    self.isEditingUserCondition = ko.observable();
    self.conditionModel = conditionModel;
    self.conditionModel.value = "";
    self.oldCondition = null;

    self.afterRenderCondition = function (elms) {
        $(elms).find('#selectFields').combobox();
    }

    self.editUserCondition = function (data, editDisable) {
        if (!editDisable) {
            if (self.isEditingUserCondition() === data)
                self.isEditingUserCondition(null);
            else {
                self.isEditingUserCondition(data);
                self.oldCondition = ko.toJS(data.conditions());
                if (data.conditions().length === 0)
                    data.conditions.push(ko.mapping.fromJS(Object.assign({}, self.conditionModel)))
            }
        }
    }

    self.closeConditions = function (data) {
        ko.mapping.fromJS(self.oldCondition, {}, self.isEditingUserCondition().conditions)
        self.isEditingUserCondition(null);
    }

    self.saveUserCondition = function (data) {
        if (self.isEditingUserCondition().conditions.any(p => !p.variable() || p.variable() === ""))
            toastr.error(CONTRIBUTOR_RESOURCES.InvalidConditionMessage);
        else {
            $("#addConditionModal").modal("toggle");
            self.isEditingUserCondition(null);

        }
    }

    self.addUserCondition = function (data) {
        data.conditions.push(ko.mapping.fromJS(Object.assign({}, self.conditionModel)))
    }

    self.removeUserCondition = function (data, parent) {
        parent.conditions.remove(data)
    }

    self.getVariableLabelFromWrite = function (write) {
        return self.templateAvailableFields.find(p => p.write === write).label
    }

    self.getOperatorNameFromValue = function (value) {
        return self.conditionOperators.find(p => p.value === value).name
    }

    /**
     * Drag and drop functions
     */
    self.dragStartIndex = ko.observable();

    $(function () {
        $("#sortable").sortable({
            revert: true,
            items: "li:not(.move-disabled)",
            update: function (e, ui) {
                // Find the index in order to set the active workflow step
                var selectedItemOldIndex = self.dragStartIndex();
                var selectedItemNewIndex = selectedItemOldIndex;
                $("#sortable li").each(function (i, elm) {
                    var currentElementOldIndex = $(elm).attr("index");
                    if (currentElementOldIndex === selectedItemOldIndex) {
                        selectedItemNewIndex = i;
                        return;
                    }
                });
                self.currentlySelectedStepIndex(selectedItemNewIndex);

                // Swap all workflow step to rearrange orders
                $("#sortable li").each(function (i, elm) {
                    var currentElementOldIndex = $(elm).attr("index");
                    if (currentElementOldIndex !== i) {
                        self.swap(currentElementOldIndex, i);
                    }
                });
            },
            stop: function (e, ui) {
                // Display link between steps, user stop dragging
                $(`li[index]`).removeClass("hidden-link");
            },
            start: function (e, ui) {
                // Keep track of the old index of the element being dragged
                var index = $(ui.item).attr("index");
                self.dragStartIndex(index);
                // Hide link between steps
                $(`li[index]`).addClass("hidden-link");
            }
        });
        $("#sortable").disableSelection();
    });

    self.swap = function (from, to) {
        if (to > self.workflow.workflowSteps().length - 1 || to < 0) return;

        var fromObj = self.workflow.workflowSteps()[from];
        var toObj = self.workflow.workflowSteps()[to];
        self.workflow.workflowSteps()[to] = fromObj;
        self.workflow.workflowSteps()[from] = toObj;
        self.workflow.workflowSteps.valueHasMutated();
    }

    self.canSaveModifications = function () {
        var dicoStepError = {};

        for (var i = 0; i < self.workflow.workflowSteps().length; ++i) {
            var step = self.workflow.workflowSteps()[i];

            var hasUnfilledExternal = self.externalContributorsToFill().find(x => x.stepId === step.id());
            if (hasUnfilledExternal) {
                dicoStepError[step.name()] = { Message: "User need to be designated" };
            }

            var hasUnfilledStepName = step.name() === null || step.name() === undefined;
            if (hasUnfilledStepName) {
                toastr.error("Une étape n'a pas de nom.");
                return false;
            }
        }

        if (Object.keys(dicoStepError).length === 0) {
            return true;
        }
        return false;
    }


    self.saveModifications = function () {
        if (self.canSaveModifications()) {
            self.isSaving(true);
            saveModificationsFunction(self);
        }
    }
}
