var HeritageViewModel = null;

function heritageViewModel(vaultHeritage, folderField, questions, returnUrl, folderId) {
    self = this;

    self.vaultHeritage = ko.mapping.fromJS(vaultHeritage);

    self.returnUrl = "/Vault/Informations?vaultId=" + returnUrl;
    self.folderId = folderId;
    self.questions = questions;
    //self.questions.forEach(question => question.AnswersHeritages = ko.observableArray(question.AnswersHeritages));
    self.folderField = folderField;
    self.answersToDelete = [];

    self.addNewAnswer = function (questionId) {
        var newAnswer = {
            Id: 0,
            QuestionId: questionId,
            VaultId: self.vaultHeritage.VaultId(),
            Comment: "",
            Data: "",
            Answer1: null
        };

        self.questions.find(q => q.Id == questionId).AnswersHeritages.push(newAnswer);
    }

    self.removeAnswer = function (questionId, answer) {
        if (answer.Id != 0) {
            self.answersToDelete.push(answer);
        }
        self.questions.find(q => q.Id == questionId).AnswersHeritages.remove(answer);
    }

    self.save = function () {

        var answers = [];

        //self.questions.forEach(question => answers = answers.concat(question.AnswersHeritages()))

        //console.log(answers);

        $.ajax({
            url: '/Vault/Formulaires/Heritage?handler=SaveInfos',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultHeritage: self.vaultHeritage,
                answersHeritages: answers,
                answersHeritagesToDelete: self.answersToDelete
            },
            success: function () {
                //
                /*if (self.folderHeritage.MainResidence() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "Titre de propriété de la résidence principale", 28, "Patrimoine");
                }

                if (self.folderHeritage.SecondResidence() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "Titre de propriété de la résidence secondaire", 76, "Patrimoine");
                }

                if (self.folderHeritage.OtherRealEstate() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "Titre de propriété d'un autre bien immobilier", 77, "Patrimoine");
                }

                if (self.folderHeritage.BankAccount() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "RIB du compte principal", 29, "Patrimoine", true);
                }

                if (self.folderHeritage.LifeInsurance() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "Contrat d'assurance vie " + self.folderHeritage.LifeInsuranceWording(), 75, "Patrimoine", true);
                }

                if (self.folderHeritage.UnitsSharesCompanies() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "Statuts (pas société par action) ou compte d'actionnaire (société par action) - " + self.folderHeritage.UnitsSharesCompaniesWording(), 31, 'Patrimoine', true)
                    createEmptyDoc(self.folderHeritage.VaultId(), "Extrait KBIS - " + self.folderHeritage.UnitsSharesCompaniesWording(), 32, 'Patrimoine', true)
                }

                if (self.folderHeritage.Copyright() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), self.folderHeritage.CopyrightWording(), 78, 'Patrimoine', true)
                }

                if (self.folderHeritage.IndustrialProperty() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), self.folderHeritage.IndustrialPropertyWording(), 78, 'Patrimoine', true)
                }

                if (self.folderHeritage.MotorVehicle() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), "Carte grise du véhicule \'" + self.folderHeritage.MotorVehicleWording() + "\'", 33, 'Patrimoine', true)
                }

                if (self.folderHeritage.ValuablePersonalProperty() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), self.folderHeritage.ValuablePersonalPropertyWording(), 78, 'Patrimoine', true)
                }

                if (self.folderHeritage.PortfolioOfShares() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), self.folderHeritage.PortfolioOfSharesWording(), 78, 'Patrimoine', true)
                }

                if (self.folderHeritage.Borrowing() == true) {
                    createEmptyDoc(self.folderHeritage.VaultId(), self.folderHeritage.BorrowingWording(), 78, "Patrimoine", true);
                }

                answers.forEach(function (answer) {
                    console.log(answer.Data);
                    if (answer.QuestionId == 48) {
                        //Autre Compte bancaire
                        createEmptyDoc(self.folderHeritage.VaultId(), "RIB du compte " + answer.Data, 29, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 55) {
                        //Autre Droit d'auteur 
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }
                    if (answer.QuestionId == 50) {
                        //Ajouter une assurance vie
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 75, 'Patrimoine', true)
                    }
                    if (answer.QuestionId == 51) {
                        //Ajouter des parts ou actions de société
                        createEmptyDoc(self.folderHeritage.VaultId(), "Statuts (pas société par action) ou compte d'actionnaire (société par action) - " + answer.Data, 31, 'Patrimoine', true)
                        createEmptyDoc(self.folderHeritage.VaultId(), "Extrait KBIS - " + answer.Data, 32, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 53) {
                        //Ajouter un droit de propriété industrielle
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 49) {
                        //Ajouter un véhicule automobile
                        createEmptyDoc(self.folderHeritage.VaultId(), "Carte grise du véhicule \'" + answer.Data + "\'", 33, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 54) {
                        //Ajouter un bien mobilier de valeur
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 56) {
                        //Ajouter un portefeuille d'actions
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 47) {
                        //Ajouter un autre bien
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }
                    else if (answer.QuestionId == 57) {
                        //Ajouter une autre dette
                        createEmptyDoc(self.folderHeritage.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }
                })*/
                //console.log(answers);
                //
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    HeritageViewModel = self;
}

