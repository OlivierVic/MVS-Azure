var PersonalSituationViewModel = null;

function personalSituationViewModel(vaultPersonalInfo, returnUrl) {
    self = this;

    self.vaultPersonalInfo = ko.mapping.fromJS(vaultPersonalInfo);

    self.returnUrl = returnUrl;

    /*self.checkData = function (questionId, answerName) {
        var question = self.questions.find(q => q.Id == questionId);
        return question.Answers[0].Answer1() == question.PossibleAnswers.find(pa => pa.Name == answerName).Id
    }*/

    self.save = function () {
        $.ajax({
            url: '/Vault/Formulaires/PersonalSitutation?handler=SaveInfos',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultPersonalInfo: self.vaultPersonalInfo,
            },
            success: function () {
                //Here boucle for createEmptyDocument
                /*if (self.vaultPersonalInfo.LivingEnvironment() == "Maison de retraite") {
                    createEmptyDoc(self.vaultPersonalInfo.VaultId(), "Contrat d'hébergement en maison de retraite", 8, "Situation Personnelle");
                }

                if (self.vaultPersonalInfo.HousingLaw() == "Propriétaire") {
                    createEmptyDoc(self.vaultPersonalInfo.VaultId(), "Avis de taxe foncière", 9, "Situation Personnelle");
                }

                if (self.vaultPersonalInfo.HousingLaw() == "Locataire") {
                    createEmptyDoc(self.vaultPersonalInfo.VaultId(), "Bail d'habitation", 10, "Situation Personnelle");
                }

                if (self.vaultPersonalInfo.ProtectiveSupervision() != null && self.vaultPersonalInfo.ProtectiveSupervision() != "Aucune") {
                    createEmptyDoc(self.vaultPersonalInfo.VaultId(), "Décision du juge", 7, "Situation Personnelle");
                }

                if (self.vaultPersonalInfo.ProtectiveSupervision() == "Habilitation familiale avec assistance") {
                    createEmptyContactPart(11, "Protecteur", self.vaultPersonalInfo.VaultId());
                }

                if (self.vaultPersonalInfo.ProtectiveSupervision() != "Habilitation familiale avec assistance" && self.vaultPersonalInfo.ProtectiveSupervision() != null && self.vaultPersonalInfo.ProtectiveSupervision() != "Aucune") {
                    createEmptyContactPro(18, "Protecteur", self.vaultPersonalInfo.VaultId());
                }

                if (self.vaultPersonalInfo.OngoingLitigation()) {
                    if (self.vaultPersonalInfo.NbLawyerFirms() != null || self.vaultPersonalInfo.NbLawyerFirms() != 0) {
                        for (i = 1; i <= self.vaultPersonalInfo.NbLawyerFirms(); i++) {
                            tmpName = "Avocat " + i
                            createEmptyContactPro(18, tmpName, self.vaultPersonalInfo.VaultId());
                        }
                    }
                }*/
                //
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    PersonalSituationViewModel = self;
}
