var ViewMod = null;

function Party(name) {
    var self = this;
    self.Name = ko.observable(name);
    self.canBeUpdated = false;
    self.canSign = false;
    return self;
}

function viewMod(data) {
    var self = this;

    self.contractId = data.ContractId;
    self.contractTitle = data.ContractTitle;
    self.sentForSignature = data.SentForSignature;
    self.signed = data.Signed;

    self.isAdmin = data.IsAdmin;
    self.isCreator = data.IsCreator;
    self.creatorEmail = data.CreatorEmail;

    self.rights = {};
    self.rightSignature = data.RightsSignature;
    data.RightsEnum.forEach(right => self.rights[right.Description] = right.Value);

    self.CancelSignError = data.CancelSignError;


    self.isAnnexesTabSelected = ko.observable(false);
    self.contractAnnexesViewModel = new ContractAnnexesViewModel(self.contractId, data.ViewerUrl);
    self.showAnnexesTab = function () {
        self.isAnnexesTabSelected(true);
        self.contractAnnexesViewModel.getContractAnnexes();
    }

    // Convert current data to workflow editor view model understandable data
    var workflow = {
        workflowSteps: data.WorkflowContractStepsList.map(x => changeCase(x, {
            "workflowContractStepUsers": "workflowStepUsers"
        })),
    };
    var rightsEnum = data.RightsEnum.map(changeCase);
    var conditionOperatorsEnum = data.ConditionOperatorsEnum.map(changeCase);
    var fieldsList = data.FieldsList.map(changeCase);
    var userModel = changeCase(data.UserModel);
    var conditionModel = changeCase(data.ConditionModel);
    var availableFields = data.TemplateAvailableFields.AvailableFields ? data.TemplateAvailableFields.AvailableFields.map(changeCase) : [];

    self.workflowPanelViewModel = new WorkflowPanelViewModel(self.isCreator || self.isAdmin, fieldsList, rightsEnum);
    self.workflowEditorViewModel = ko.observable();
    self.loadWorkflowEditorViewModel = function () {
        workflow.currentStep = ViewModel.workflow()[0].Step();
        self.workflowEditorViewModel(new WorkflowEditorViewModel(workflow,
            rightsEnum,
            conditionOperatorsEnum,
            fieldsList,
            userModel,
            conditionModel,
            availableFields,
            false,
            function (self) {
                self.isSaving(true);
                var workflowObj = ko.mapping.toJS(self.workflow.workflowSteps);
                // Reorder
                workflowObj.forEach((step, index) => {
                    step.stepOrder = index;
                });

                // Convert object to be understood by our api
                workflowObj.map(x => {
                    x["workflowContractStepUsers"] = x["workflowStepUsers"];
                    return x;
                });

                $.ajax({
                    url: '/Contract/SaveWorkflowContractModifications',
                    method: 'POST',
                    data: {
                        workflowContractSteps: JSON.stringify(workflowObj),
                    },
                    complete: function (d, s) {

                        if (typeof (chatHub) !== "undefined" && chatHub !== null) {
                            chatHub.server.updateContractWorkflow(self.workflow.workflowSteps()[0].contractId());
                        }
                        window.location.reload();
                        self.isSaving(false);
                    }
                });
            }));
    }

    self.contractLoaded = ko.observable(false);

    self.copiedElementStyle = ko.observable();

    self.connectedUsers = ko.observableArray([]);

    self.computedConnectedUsers = ko.computed(function () {
        let unique = [];
        self.connectedUsers().forEach(u => {
            if (unique.find(user => user.username === u.username) == null)
                unique.push(u);
        })
        return unique;
    });

    self.sendToLastParty = function () {
        if (ViewModel.fullContractParties().length > 1) {
            $.ajax({
                url: '/Reader/SendToSpecificUser',
                method: 'POST',
                data: {
                    contractId: self.contractId,
                    contractTitle: self.contractTitle,
                    email: ViewModel.fullContractParties()[ViewModel.fullContractParties().length - 1].PartyEmail(),
                    messageContent: $("#single-message-content").val()
                },
                complete: function (d, s) {
                    $("#send-to-user").modal("hide");
                }
            });
        }
    };


    self.tryValidate = function (data, event) {
        return ViewModel.tryValidate(data, event);
    };

    self.sendContract = function (data, event) {
        ViewModel.sendContractToParties();
        toastr["success"]("Le document a bien été transmis");
    };

    self.showModalSign = function (data, event) {
        $("#confirm-sign").modal("show");
    };

    self.showModalReopen = function (data, event) {
        $("#reopen-negotiation-modal").modal("show");
    };

    self.contractSigneesInternal = ko.observableArray([]);
    self.contractSignees = ko.computed(function () {
        if (self.contractLoaded())
            return self.contractSigneesInternal().concat(ViewModel.fullContractParties().map(c => { return { Name: c.PartyEmail, canBeUpdated: true, canSign: ko.observable(true) } })).reverse();
        return [];
    });

    self.addSigningParty = function (data, event) {
        self.contractSigneesInternal.unshift(ko.mapping.fromJS({ Name: ko.observable(' '), canBeUpdated: true, canSign: ko.observable(true) }));
    };
    self.removeSigningParty = function (data, event) {
        self.contractSigneesInternal.remove(data);
    };
    self.updatedSigningParties = ko.computed(function () {
        return self.contractSignees().filter(c => c.canSign()).map(c => c.Name()).join(',');
    });

    self.setUserFilter = function (data, event) {
        ViewModel.filterOnUsers($("#userFilter").val());
    }

    self.checkInputsRequired = function (inputs) {
        let error = false;
        for (let i = 0; i < inputs.length; ++i) {
            if ($(inputs[i]).val() === '') {
                $(inputs[i]).addClass('input-required');
                error = true;
            } else {
                $(inputs[i]).removeClass('input-required');
            }
        }
        return error;
    }

    self.generateWord = function () {
        window.location = '/Contract/WordByteArray?contractId=' + self.contractId;
    }

    self.generatePdf = function () {
        window.location = '/Contract/PDFByteArray?contractId=' + self.contractId;
    }

    self.saveAll = function (data, event) {
        ViewModel.saveDocument();
    };

    self.showTab = function (data, event) {
        if (typeof ViewModel == "undefined")
            return;

        ViewModel.shouldShowVersionBody(false);
        self.revisionsVisible(false);
        self.versionsVisible(false);
        self.usersVisible(false);
        self.compareVersion(false);
        $("#reader").show();

        $('.right-pane').show();
        $("#document-body").removeClass("edition");
        var tabToShow = $(event.currentTarget).data('tab');

        $(".tab-sc").removeClass("active");
        $(event.currentTarget).addClass("active");
        ViewModel.hideLeftPanes();
        ViewModel.showLeftPane(tabToShow);

        //if (tabToShow === "form") {
        //    $(".chat.public").hide();
        //    $(".chat.private").hide();
        //}
        //else {
        //    $(".chat.public").show();
        //    $(".chat.private").show();
        //}
        ViewModel.toggleIcons(false);

        froala.toolbar.hide();
        froala.edit.off()
    };

    self.checkMenuToDisplay = function () {
        if (self.stepValidatedbyUser() && ViewMod.currentWorkflowStepUser()[0].rights() === 0) {
            ViewModel.hideLeftPanes();
            $('#validated-form-message').show();
            $('#form-tab').hide();
        }
    }

    self.userConnected = ko.observable("");

    self.setUserConnected = function (data, event) {
        if (typeof ViewModel == "undefined")
            return false;

        self.userConnected(data);
    }

    self.contractIndex = ko.mapping.fromJS(data.ContractIndex);

    ////Workflow Part
    if (data.WorkflowContractStepsList) {
        self.workflowStep = ko.mapping.fromJS(data.WorkflowContractStepsList.sort(function (a, b) {
            return a.StepOrder - b.StepOrder;
        }));
        self.selectedWorkflowStep = ko.observable(self.workflowStep()[0]);
    }

    self.userModel = data.UserModel;
    self.actions = ko.mapping.fromJS(data.RightsEnum);
    self.contributorFields = data.FieldsList;
    self.editWorkflow = ko.observable(false);
    self.currentlySelectedStepIndex = ko.observable(0);

    self.currentWorkflowStep = ko.observable();
    self.currentWorkflowStepUser = ko.observable(self.workflowStep()[0].workflowStepUsers());
    self.nextWorkflowStep = ko.observable(self.workflowStep()[0]);

    self.setCurrentWorkflow = function (data, event) {
        if (typeof ViewModel == "undefined")
            return false;

        var currentStep = ViewModel.workflow()[0].Step();

        for (var i = 0; i < self.workflowStep().length; i++) {
            if (self.workflowStep()[i].stepOrder() !== currentStep) {
                continue;
            } else {
                self.currentWorkflowStep(self.workflowStep()[i]);
                self.currentWorkflowStepUser(self.workflowStep()[i].workflowStepUsers().filter(obj => {
                    return obj.email() === self.userConnected()
                }));
                return;
            }
        }
    }

    self.setNextWorkflowStep = function (data, event) {
        if (typeof ViewModel == "undefined")
            return false;

        var currentStep = ViewModel.workflow()[0].Step();
        let index = self.workflowStep().findIndex((elm) => elm.stepOrder() === currentStep + 1);
        self.nextWorkflowStep(self.workflowStep()[index]);
    }

    self.showRevisionTab = function () {
        $(".tab-sc").removeClass("active");
        $("#revisions-tab").addClass("active");
        $("#reader").show();

        ViewModel.hideLeftPanes();
        ViewModel.showLeftPane("rev");

        //$(".chat.public").show();
        //$(".chat.private").show();

        ViewModel.toggleIcons(false);

        self.showRevisions();
    }

    //self.isStepOver = function (data) {
    //    for (var i = 0; i < data.WorkflowContractStepUsers().length; i++) {
    //        if (!data.WorkflowContractStepUsers()[i].Validated()) {
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    self.showNextStepModal = function () {
        if (self.nextWorkflowStep().builtinOption() === 4 && ViewModel.revisions().length > 0) {
            toastr.warning(ContractResources.RevWarningSignature);
            return;
        }

        $("#go-to-next-workflow-step-modal").modal('show');
    };

    self.showNextStepModalWithOptionalNotValidated = function () {
        if (self.nextWorkflowStep().builtinOption() === 4 && ViewModel.revisions().length > 0) {
            toastr.warning(ContractResources.RevWarningSignature);
            return;
        }

        var nextStepContractModal = $("#go-to-next-workflow-step-modal-without-optional-validations");
        if (typeof nextStepContractModal == undefined)
            return false;

        nextStepContractModal.modal('show');
    }

    //self.addStep = function (data, event) {
    //    if (typeof ViewModel == "undefined")
    //        return false;

    //    self.workflowStep.push(ko.mapping.fromJS(Object.assign({}, self.stepModel)))
    //    self.currentlySelectedStepIndex(self.workflowStep().length - 1);
    //}


    self.validateStepUser = function (data, event) {
        self.setValidationStateForUser(true);
    }

    self.invalidateStepUser = function (data, event) {
        self.setValidationStateForUser(false);
    }

    self.setValidationStateForUser = function (state) {
        var contractWorkflowStep = self.currentWorkflowStep();
        var contractWorkStepUser = contractWorkflowStep.workflowStepUsers().filter(obj => {
            return obj.email() === self.userConnected()
        });

        var url;
        if (!state)
            url = `/Contract/InvalidateContractWorkflowStepUser?contractId=${self.contractId}`
        else
            url = `/Contract/ValidateContractWorkflowStepUser?contractId=${self.contractId}`

        $.ajax({
            url: url,
            type: 'POST',
            data: {
                ContractWorkflowStepId: contractWorkflowStep.id(),
                WorkflowContractStepUserId: contractWorkStepUser[0].id(),
                ContractIndex: self.contractIndex(),
                State: state
            },
            headers: headers,
        }).done(function (data) {
            location.reload();
        });
    }



    self.validateStep = function (data, event) {
        if (typeof self.currentWorkflowStep() == "undefined")
            return false;

        $.ajax({
            url: `/Contract/ValidateContractWorkflowStep?contractId=${self.contractId}`,
            type: 'POST',
            data: {
                ContractWorkflowStepId: self.currentWorkflowStep().id()
            },
            headers: headers,
        }).done(function (data) {
            location.reload();
        });
    }

    self.addContributor = function (data, event) {
        self.selectedWorkflowStep().workflowStepUsers.push(ko.mapping.fromJS(Object.assign({}, self.userModel)));
    }

    self.userOtherIndex = ko.observable(0);

    self.setOtherUser = function (event, data) {
        var stepUser = self.selectedWorkflowStep().WorkflowContractStepUsers()[self.userOtherIndex()];
        stepUser.Email(data.item.data.email);
        stepUser.Lastname(data.item.data.lastName);
        stepUser.Firstname(data.item.data.firstName);

        if (data.item.data.email == "@Author") {
            var authorFieldId = self.fields.find(e => e.Title == "Author").Id;
            self.selectedWorkflowStep().WorkflowContractStepUsers()[self.userOtherIndex()].FieldId(authorFieldId);
        }
    }

    self.setOtherCurrentUserStep = function (data, event) {
        self.userOtherIndex($(event.currentTarget).data("id"));
    }

    //Compare Versions Part
    self.versions = ko.pureComputed(function () { return ViewModel.versions(); });

    self.leftVersionCompared = ko.observable();
    self.rightVersionCompared = ko.observable();
    self.versionsVisible = ko.observable(false);
    self.compareVersion = ko.observable(false);
    self.versionToRestore = ko.observable('');
    self.versionNumber = ko.observable('');
    self.firstShowVersion = false;

    self.setLeftVersionCompared = function (data, event) {
        self.leftVersionCompared($("#leftCompareVersion").val());
    };

    self.setRightVersionCompared = function (data, event) {
        self.rightVersionCompared($("#rightCompareVersion").val());
    };

    self.setLeftVersionComparedAndCompareVersions = function (data, event) {
        self.leftVersionCompared($("#leftCompareVersion2").val());
        if (self.leftVersionCompared() != self.rightVersionCompared()) {
            self.compareTwoVersion();
        }
    };

    self.setRightVersionComparedAndCompareVersions = function (data, event) {
        self.rightVersionCompared($("#rightCompareVersion2").val());
        if (self.leftVersionCompared() != self.rightVersionCompared()) {
            self.compareTwoVersion();
        }
    };

    self.getLeftNameVersionCompared = function (data, event) {
        return ViewModel.versions().find(element => element.Id === self.leftVersionCompared());
    }

    self.getRightNameVersionCompared = function (data, event) {
        return ViewModel.versions().find(element => element.Id === self.rightVersionCompared());
    }

    self.showVersions = function (data, event) {
        if (typeof ViewModel == "undefined")
            return false;

        self.versionsVisible(true);
        ViewModel.displayVersionTab();
        if (!self.firstShowVersion) {
            self.firstShowVersion = true;
            // update pages
            if (verFroala && verFroala.page_a4) {
                verFroala.page_a4.update();
            }
            new PrettyScroll('.verBar', {
                container: '#reader-card',
                offsetTop: 51, // space between the sticky element and the top of the window
                offsetBottom: 0, // space between the sticky element and the bottom of the window
                wishedElemWidth: $("#lecteur"),
                shouldUseWishedWidth: true,
                condition: function () {
                    return true;
                }, // you can disable the sticky behavior by returning false, it will be executed when you scroll.
            });
            var resizeEvent = new Event('resize');
            window.dispatchEvent(resizeEvent);
        }
    };

    self.usersVisible = ko.observable(false);
    self.showUsers = function (data, event) {
        self.usersVisible(true);
        $('.right-pane').hide();
    }

    self.restoreVersion = function (data, event) {
        self.versionToRestore(ViewModel.currentVersionName());
        self.versionNumber(ViewModel.currentVersionNumber());
        $("#rv-versionName").text(ViewModel.currentVersionName());
        $("#version-restore-modal").modal('show');

        new PrettyScroll('.revBar-2', {
            container: '.card-body',
            offsetTop: 51, // space between the sticky element and the top of the window
            offsetBottom: 0, // space between the sticky element and the bottom of the window
            wishedElemWidth: $("#lecteur"),
            shouldUseWishedWidth: true,
            condition: function () {
                return true;
            }, // you can disable the sticky behavior by returning false, it will be executed when you scroll.

        });
    };

    self.shouldDisplayRestore = function () {
        if (typeof ViewModel == "undefined")
            return false;

        return ViewModel.curVersion().Number != ViewModel.currentVersionNumber();
    };

    self.proceedRestore = function (data, event) {
        ViewModel.proceedRestore();
        $("#version-restore-modal").modal('hide');
    };

    self.revisionsVisible = ko.observable(false);
    self.showRevisions = function (data, event) {
        if (typeof ViewModel == "undefined")
            return;

        if (!ViewModel.bodyLockedFromEdit() && ViewModel.userRights.Modify) {
            $("#document-body").addClass("edition");
            froala.toolbar.show();
            froala.edit.on();
            manageNewParagraphs();
            tracker.setShouldUseAliasView(true);
        }

        $("#reader").show();
        self.revisionsVisible(true);
        self.compareVersion(false);
        new PrettyScroll('.revBar', {
            container: '#reader-card',
            offsetTop: 51, // space between the sticky element and the top of the window
            offsetBottom: 0, // space between the sticky element and the bottom of the window
            wishedElemWidth: $("#lecteur"),
            shouldUseWishedWidth: true,
            condition: function () {
                return true;
            }, // you can disable the sticky behavior by returning false, it will be executed when you scroll.
        });
        var resizeEvent = new Event('resize');
        window.dispatchEvent(resizeEvent);
    };

    self.currentRevIdx = ko.computed(function () {
        if (typeof ViewModel !== 'undefined') {
            return ViewModel.revIdx();
        }
        else {
            return -1;
        }
    });

    self.prevRev = function (data, event) {
        ViewModel.showPreviousRev();
    };

    self.nextRev = function (data, event) {
        ViewModel.showNextRev();
    };
    self.validateAllRevs = function (data, event) {
        ViewModel.validateAllRevs();
        ViewModel.saveDocument();
    };

    self.addComment = function (data, event) {
        ViewModel.addComment(data, event);
    };

    self.toggleRevs = function (data, event) {
        ViewModel.showRevisionsElts(!ViewModel.showRevisionsElts());
        return true;
    };

    self.toggleComments = function (data, event) {
        ViewModel.showCommentElts(!ViewModel.showCommentElts());
        return true;
    };

    self.showVersionModal = function (data, event) {
        $("#version-creation-modal").modal('show');
    };

    self.showCompareVersionModal = function (data, event) {
        $("#compare-version-creation-modal").modal('show');
    };

    self.goBackToVersion = function (data, event) {
        if (typeof ViewModel == "undefined")
            return;

        self.compareVersion(false);
        $(".tab-sc").removeClass("active");
        $("#versions-tab").addClass("active");
        $("#reader").show();
    };

    self.compareTwoVersion = function (data, event) {
        if (self.leftVersionCompared() !== self.rightVersionCompared()) {
            $("#reader").hide();
            $("#compare-version-creation-modal").modal('hide');
            self.contractLoaded(false);
            self.compareVersion(true);
            $.ajax({
                url: '/Contract/CompareVersion/',
                type: 'GET',
                data: {
                    version1Id: self.leftVersionCompared(),
                    version2Id: self.rightVersionCompared()
                }
            }).done(function (data) {
                //Need to change css to display differences between version
                var comparedVersion = self.applyCssCompareVersion(data.compareHtml);

                self.contractLoaded(true);

                $("#comparatorRevBar").show();
                $("#leftVersionComparator").html(data.versionHtml);
                $("#rightVersionComparator").html(comparedVersion);
                $("#leftCompareVersion2").val(self.leftVersionCompared());
                $("#rightCompareVersion2").val(self.rightVersionCompared());

                $(".tab-sc").removeClass("active");
                $("#versions-tab").addClass("active");
            });
        }
    };

    self.goToEditWorkflow = function (data, event) {
        if (typeof ViewModel == "undefined")
            return;

        self.editWorkflow(true);
        self.revisionsVisible(false);
        self.versionsVisible(false);
        self.usersVisible(false);
        self.compareVersion(false);
        $("#reader").hide();
        $("#linksBar").hide();
        $(".row-links").css("height", "0px");
        $("#lecteur").toggleClass('goToEditWorkflow');
        $("#lecteur").css('transition', '0s');
        $("#left-workflow-panel").hide();
        $("#main-navbar").hide();
        $(".reader-card-content").hide();

    };

    self.stateModified = function () {
        var currentState = ko.mapping.toJS(ViewMod.workflowEditorViewModel().workflow.workflowSteps);
        var previousState = ViewMod.workflowEditorViewModel().baseModel.workflowSteps;
        return JSON.stringify(currentState) !== JSON.stringify(previousState);
    }

    self.leaveEditWorkflow = function (data, event) {
        if (typeof ViewModel == "undefined")
            return;

        if (self.stateModified()) {
            var confirmQuit = confirm("Vous n'avez pas enregistré vos modifications, êtes vous sûr de vouloir retourner sur le contrat ?");
            if (!confirmQuit) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
        }

        self.editWorkflow(false);
        self.revisionsVisible(false);
        self.versionsVisible(false);
        self.usersVisible(false);
        self.compareVersion(false);

        if (!self.isAnnexesTabSelected()) {
            $("#reader").show();
        }

        $("#linksBar").show();
        $(".row-links").css("height", "50px");
        $("#lecteur").toggleClass('goToEditWorkflow');
        $("#left-workflow-panel").show();
        $("#main-navbar").show();
        $(".reader-card-content").show();
    };

    self.applyCssCompareVersion = function (data, event) {
        data = data.replaceAll("<ins ", "<ins class='insGreen' ");
        return data.replaceAll("<del ", "<del class='delRed' ");
    }

    self.saveVersion = function (data, event) {
        if ($("#versionNameInput").val() && ViewModel.saveVersion($("#versionNameInput").val())) {
            $("#version-creation-modal").modal('hide');
            $("#versionNameInput").val('');
        }
    };

    self.getTitleFromConnectedUser = function (data) {
        if (!self.contractLoaded())
            return '';

        var email = data.username;
        var user = ko.utils.arrayFirst(ViewModel.fullContractParties(), function (item) {
            return item.PartyEmail() == email;
        });

        if (user)
            return user.PartyFirstName() + " " + user.PartyLastName() + " (" + email + ")";

        return "Pas d'information";
    };

    self.showSendToSignatureModal = function (data, event) {
        var sendToSignatureModal = $("#sendtosignature-modal");
        if (typeof nextStepContractModal == undefined)
            return false;

        self.contractAnnexesViewModel.getContractAnnexes();
        sendToSignatureModal.modal('show');
    }

    self.showCancelSignatureModal = function (data, event) {
        var cancelSignatureModal = $("#cancelsignature-modal");
        if (typeof nextStepContractModal == undefined)
            return false;

        cancelSignatureModal.modal('show');
    }

    self.sendingToSignature = ko.observable(false);
    self.chosenAnnexes = ko.observableArray();

    self.sendContractToSignature = function (data, event) {
        self.sendingToSignature(true);

        $.ajax({
            url: '/Contract/SendToSignature',
            type: 'POST',
            contentType: "application/json",
            data: ko.mapping.toJSON({
                contractId: self.contractId,
                workflowStepId: self.currentWorkflowStep().id(),
                annexesToSend: self.chosenAnnexes()
            }),
            headers: headers,
        }).done(function (data) {
            $("#sendtosignature-modal").modal("hide");
            $("#validsignature-modal").modal("show");
        });
    }

    self.cancelingSignature = ko.observable(false);
    self.cancelSignature = function (data, event) {
        if (!$("#cancelsignature-reason").val()) {
            toastr.error(self.CancelSignError);
            return;
        }

        self.cancelingSignature(true);

        $.ajax({
            url: '/Contract/VoidEnvelope',
            type: 'POST',
            data: {
                contractId: self.contractId,
                voidMessage: $("#cancelsignature-reason").val()
            },
            headers: headers,
        }).done(function (data) {
            location.reload();
        });
    }

    self.stepValidatedbyUser = ko.pureComputed(function () {
        return self.currentWorkflowStepUser()[0].validated() != null
            && self.currentWorkflowStepUser()[0].validated() == true;
    });

    //Show Button
    self.showInvalidStepButton = ko.pureComputed(function () {
        return self.currentWorkflowStepUser().length > 0
            && self.currentWorkflowStepUser()[0].validated() != null
            && self.currentWorkflowStepUser()[0].validated() == true
            && self.currentWorkflowStepUser()[0].rights() != ViewMod.rightSignature
    });

    self.showValidStepButton = ko.pureComputed(function () {
        return ViewMod.currentWorkflowStepUser().length > 0
            && (ViewMod.currentWorkflowStepUser()[0].validated() == null || ViewMod.currentWorkflowStepUser()[0].validated() == false)
            && ViewMod.currentWorkflowStepUser()[0].rights() != ViewMod.rightSignature
    });

    self.showSendSignButton = ko.pureComputed(function () {
        return ViewMod.currentWorkflowStep() != null
            && ViewMod.currentWorkflowStep().workflowStepUsers().some((stepUser) => stepUser.rights() == ViewMod.rightSignature)
            && ViewMod.workflowPanelViewModel.allowEditing
            && ViewMod.sentForSignature != true
    });

    self.showCancelSignButton = ko.pureComputed(function () {
        return ViewMod.currentWorkflowStep() != null
            && ViewMod.currentWorkflowStep().workflowStepUsers().some((stepUser) => stepUser.rights() == ViewMod.rightSignature)
            && ViewMod.workflowPanelViewModel.allowEditing && ViewMod.sentForSignature == true
    });

    self.returnToDrive = function (url) {
        ViewModel.saveDocument();
        window.location.href = url;
    }

    ViewMod = self;
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
