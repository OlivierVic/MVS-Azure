var FamilySituationViewModel = null;

function familySituationViewModel(familyPersonalInfo, returnUrl) {
    self = this;

    self.familyPersonalInfo = ko.mapping.fromJS(familyPersonalInfo);

    self.returnUrl = returnUrl;

    self.save = function () {
        $.ajax({
            url: '/Vault/Formulaires/FamilySituation?handler=SaveInfos',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderFamilyInfo: self.familyPersonalInfo,
            },
            success: function () {
                //Here boucle for createEmptyDocument
                /*if (self.familyPersonalInfo.FamilialSituation() == "En couple") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Pièce identité du conjoint", 11, "Situation Familiale");
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Extrait acte de naissance du conjoint", 13, "Situation Familiale");
                }

                if (self.familyPersonalInfo.FamilialSituation() == "Seule - veuve") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Acte de décès du conjoint", 14, "Situation Familiale");
                }

                if (self.familyPersonalInfo.FamilialSituation() == "Seule - divorcée") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Jugement de divorce", 15, "Situation Familiale");
                }

                if (self.familyPersonalInfo.CoupleSituation() == "Marié") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Extrait d'acte de mariage", 16, "Situation Familiale");
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Livret de famille", 19, "Situation Familiale");
                    createEmptyContactPart(10, "Conjoint", self.familyPersonalInfo.VaultId());
                }
                if (self.familyPersonalInfo.CoupleSituation() == "Pacsé") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Extrait de contrat de Pacs", 17, "Situation Familiale");
                    createEmptyContactPart(10, "Partenaire", self.familyPersonalInfo.VaultId());
                }

                if (self.familyPersonalInfo.CoupleSituation() == "En concubinage") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Attestation sur l'honneur de vie en concubinage", 18, "Situation Familiale");
                    createEmptyContactPart(10, "Concubin", self.familyPersonalInfo.VaultId());
                }

                if (self.familyPersonalInfo.MatrimonialSituation() == "séparation de bien" || self.familyPersonalInfo.MatrimonialSituation() == "communauté universelle") {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Extrait contrat de mariage", 74, "Situation Familiale");
                }

                if (self.familyPersonalInfo.LivingDonation()) {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Acte de donation au dernier vivant", 50, "Situation Familiale");
                }

                if (self.familyPersonalInfo.Children()) {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Livret de famille ", 19, "Situation Familiale");
                    if (self.familyPersonalInfo.NbChildren() != null || self.familyPersonalInfo.NbChildren() != 0) {
                        for (i = 1; i <= self.familyPersonalInfo.NbChildren(); i++) {
                            tmpName = "Enfant " + i
                            createEmptyContactPart(11, tmpName, self.familyPersonalInfo.VaultId());
                        }
                    }
                }

                if (self.familyPersonalInfo.BlendedFamily()) {
                    createEmptyDoc(self.familyPersonalInfo.VaultId(), "Livret de famille du conjoint ", 12, "Situation Familiale");
                }*/
                //
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    FamilySituationViewModel = self;
}
