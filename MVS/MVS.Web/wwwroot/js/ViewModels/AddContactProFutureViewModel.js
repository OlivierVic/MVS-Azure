var AddContactProFutureViewModel = null;

function addContactProFutureViewModel(newContact) {
    self = this;
    //self.newContactPro = ko.observable(newContact);
    self.newContact = ko.mapping.fromJS(newContact);
    self.newContact.IsFutuAgent(null);
    self.Property = ko.observableArray();
    self.newContact.ProtectPeople = ko.observable("");
    self.newContact.ProtectAllProperty = ko.observable("");
    self.newContact.ProtectAllProperty = ko.observable("");

    self.checkAdd = function () {
        var missingData = false;
        $("input.required").each(function () {
            console.log($(this).attr('name'));
            if ($(this).is(':radio') && $('input[name=' + $(this).attr('name') + ']:checked').length == 0) {
                if ($(this).attr('name') == 'AgentMission' && !self.newContact.IsFutuAgent()) {
                    return;
                }
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
        if (self.newContact.IsFutuAgent() == true) {
            var tmpName = self.newContact.LastName() + " " + self.newContact.FirstName();
            createEmptyDoc(self.newContact.VaultId(), "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
            createEmptyDoc(self.newContact.VaultId(), "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
            createEmptyDoc(self.newContact.VaultId(), "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
        }
        else {
            if (self.newContact.TypeMission() != null && self.newContact.TypeMission() != 3) {
                var tmpName = self.newContact.LastName() + " " + self.newContact.FirstName();
                createEmptyDoc(self.newContact.VaultId(), "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
                createEmptyDoc(self.newContact.VaultId(), "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
                createEmptyDoc(self.newContact.VaultId(), "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
            }
        }
        //

        $("#add-contact-step2-form").submit();
    }

    AddContactProFutureViewModel = self;
}
