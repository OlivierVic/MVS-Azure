var AddContactProImmediatViewModel = null;

function addContactProImmediatViewModel(newContact) {
    self = this;

    self.newContact = ko.observable(newContact);
    self.IsSetJudge = ko.observable("");

    self.checkAdd = function () {
        var missingData = false;
        $("input.required, select.required").each(function () {
            if ($(this).is(':radio') && $('input[name=' + $(this).attr('name') + ']:checked').length == 0) {
                $(this).parent().parent().addClass("errorHere");
                missingData = true;
            }
            else {
                $(this).parent().parent().removeClass("errorHere");
            }
        });
        if (missingData) {
            return;
        }

        //Here boucle for createEmptyDocument
        if (self.newContact().TypeMission == 2 && self.newContact().CloseNotice == true) {
            var tmpName = self.newContact().LastName + " " + self.newContact().FirstName;
            createEmptyDoc(self.newContact().VaultId, "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
        }
        else if (self.newContact().TypeMission == 2 && self.newContact().CloseNotice == false) {
            var tmpName = self.newContact().LastName + " " + self.newContact().FirstName;
            createEmptyDoc(self.newContact().VaultId, "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
        }
        else if (self.newContact().TypeMission != 2 && self.newContact().CloseNotice == true) {
            var tmpName = self.newContact().LastName + " " + self.newContact().FirstName;
            createEmptyDoc(self.newContact().VaultId, "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
        }
        //

        $("#add-contact-step2-form").submit();
    }

    AddContactProImmediatViewModel = self;
}
