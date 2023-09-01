var AddContactParticularFutureViewModel = null;

function addContactParticularFutureViewModel(newContact) {
    self = this;

    /*self.newContact = ko.observable(newContact);*/
    self.newContact = ko.mapping.fromJS(newContact);

    self.Property = ko.observableArray();
    self.Kinship = ko.observable("");
    self.TypeMission = ko.observable("");
    self.IsFutuAgent = ko.observable(self.newContact.IsFutuAgent);
    /*self.newContact.IsFutuAgent = ko.observable(null);*/
    self.AgentMission = ko.observable("");
    self.HelpNeeded = ko.observable("");
    self.newContact.ProtectPeople = ko.observable("");
    self.newContact.ProtectAllProperty = ko.observable("");
    self.newContact.ProtectAllProperty = ko.observable("");

    self.checkAdd = function () {
        var missingData = false;
        $("input.required").each(function () {
            if ($(this).is(':radio') && $('input[name=' + $(this).attr('name') + ']:checked').length == 0) {
                if ($(this).attr('name') == 'AgentMission' && !self.IsFutuAgent()) {
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
        if (self.IsFutuAgent() == true) {
            var tmpName = self.newContact.LastName + " " + self.newContact.FirstName;
            createEmptyDoc(self.newContact.VaultId, "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
            createEmptyDoc(self.newContact.VaultId, "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
            createEmptyDoc(self.newContact.VaultId, "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
        }
        else {
            if (self.TypeMission() != null && self.TypeMission() != 3) {
                var tmpName = self.newContact.LastName + " " + self.newContact.FirstName;
                createEmptyDoc(self.newContact.VaultId, "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
                createEmptyDoc(self.newContact.VaultId, "Extrait casier judiciaire de " + tmpName, 82, "VaultContact", true);
                createEmptyDoc(self.newContact.VaultId, "Avis d'imposition de " + tmpName, 83, "VaultContact", true);
            }
        }

        if (self.newContact.HelpNeeded == 1) {
            createEmptyDoc(self.newContact.VaultId, "Jugement de mise sous protection", 37, "VaultContact");
        }
        //

        $("#add-contact-particular-future-form").submit();
    }

    AddContactParticularFutureViewModel = self;
}
