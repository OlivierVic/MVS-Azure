var DigitalLifeViewModel = null;

function digitalLifeViewModel(vaultDigitalLife, returnUrl, answersDigitalLife, vaultId) {
    self = this;

    self.vaultDigitalLife = ko.mapping.fromJS(vaultDigitalLife);

    self.returnUrl = returnUrl;
    self.vaultId = vaultId;
    self.answersDigitalLife = ko.observableArray(answersDigitalLife);
    self.answersDigitalLife().forEach(place => place.Order = ko.observable(place.Order));
    //self.digitalAnswer = ko.observableArray(answersDigitalLife);
    //self.digitalAnswer.forEach(digitalAnswer => digitalAnswer.AnswersHeritages = ko.observableArray(digitalAnswer.AnswersHeritages));
    self.answersToDelete = [];

    self.addNewAnswer = function (vaultId) {
        var newAnswer = {
            Id: 0,
            //VaultId: vaultId,
            VaultId: self.vaultDigitalLife.VaultId(),
            ReseauSocial: "",
            AnswerOtherReseauSocial: "",
            AnswerProfileUrl: "",
            AnswerIdentifiantProfile: "",
            AnswerLegataire: null,
            AnswerLegataireFirstLastName: "",
            AnswerProfileUrlLegataire: ""
        };

        self.answersDigitalLife.push(newAnswer);
        //self.answersDigitalLife.find(q => q.Id == vaultId).answersDigitalLife.push(newAnswer);
    }

    self.removeAnswer = function (vaultId, answer) {
        if (answer.Id != 0) {
            self.answersToDelete.push(answer);
        }
        self.answersDigitalLife.remove(answer);
    }

    self.save = function () {

        var answers = [];

        //self.answersDigitalLife().forEach(digitalAnswer => answers = answers.concat(digitalAnswer.answersDigitalLife()))
        //self.digitalAnswer.forEach(digitalAnswer => answers = answers.concat(digitalAnswer.answersDigitalLife()))

        //console.log(answers);

        $.ajax({
            url: '/Vault/Formulaires/DigitalLife?handler=SaveInfos',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultDigitalLife: self.vaultDigitalLife,
                answersDigitalLife: answers,
                answersDigitalLifeToDelete: self.answersToDelete
            },
            success: function () {
                //
                /*if (self.vaultDigitalLife.MainResidence() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Titre de propriété de la résidence principale", 28, "Patrimoine");
                }

                if (self.vaultDigitalLife.SecondResidence() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Titre de propriété de la résidence secondaire", 76, "Patrimoine");
                }

                if (self.vaultDigitalLife.OtherRealEstate() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Titre de propriété d'un autre bien immobilier", 77, "Patrimoine");
                }

                if (self.vaultDigitalLife.BankAccount() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "RIB du compte principal", 29, "Patrimoine", true);
                }

                if (self.vaultDigitalLife.LifeInsurance() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Contrat d'assurance vie " + self.vaultDigitalLife.LifeInsuranceWording(), 75, "Patrimoine", true);
                }

                if (self.vaultDigitalLife.UnitsSharesCompanies() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Statuts (pas société par action) ou compte d'actionnaire (société par action) - " + self.vaultDigitalLife.UnitsSharesCompaniesWording(), 31, 'Patrimoine', true)
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Extrait KBIS - " + self.vaultDigitalLife.UnitsSharesCompaniesWording(), 32, 'Patrimoine', true)
                }

                if (self.vaultDigitalLife.Copyright() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), self.vaultDigitalLife.CopyrightWording(), 78, 'Patrimoine', true)
                }

                if (self.vaultDigitalLife.IndustrialProperty() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), self.vaultDigitalLife.IndustrialPropertyWording(), 78, 'Patrimoine', true)
                }

                if (self.vaultDigitalLife.MotorVehicle() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), "Carte grise du véhicule \'" + self.vaultDigitalLife.MotorVehicleWording() + "\'", 33, 'Patrimoine', true)
                }

                if (self.vaultDigitalLife.ValuablePersonalProperty() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), self.vaultDigitalLife.ValuablePersonalPropertyWording(), 78, 'Patrimoine', true)
                }

                if (self.vaultDigitalLife.PortfolioOfShares() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), self.vaultDigitalLife.PortfolioOfSharesWording(), 78, 'Patrimoine', true)
                }

                if (self.vaultDigitalLife.Borrowing() == true) {
                    createEmptyDoc(self.vaultDigitalLife.VaultId(), self.vaultDigitalLife.BorrowingWording(), 78, "Patrimoine", true);
                }

                answers.forEach(function (answer) {
                    console.log(answer.Data);
                    if (answer.QuestionId == 48) {
                        //Autre Compte bancaire
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), "RIB du compte " + answer.Data, 29, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 55) {
                        //Autre Droit d'auteur 
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }
                    if (answer.QuestionId == 50) {
                        //Ajouter une assurance vie
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 75, 'Patrimoine', true)
                    }
                    if (answer.QuestionId == 51) {
                        //Ajouter des parts ou actions de société
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), "Statuts (pas société par action) ou compte d'actionnaire (société par action) - " + answer.Data, 31, 'Patrimoine', true)
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), "Extrait KBIS - " + answer.Data, 32, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 53) {
                        //Ajouter un droit de propriété industrielle
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 49) {
                        //Ajouter un véhicule automobile
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), "Carte grise du véhicule \'" + answer.Data + "\'", 33, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 54) {
                        //Ajouter un bien mobilier de valeur
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 56) {
                        //Ajouter un portefeuille d'actions
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }

                    if (answer.QuestionId == 47) {
                        //Ajouter un autre bien
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 78, 'Patrimoine', true)
                    }
                    else if (answer.QuestionId == 57) {
                        //Ajouter une autre dette
                        createEmptyDoc(self.vaultDigitalLife.VaultId(), answer.Data, 78, 'Patrimoine', true)
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

    DigitalLifeViewModel = self;
}

