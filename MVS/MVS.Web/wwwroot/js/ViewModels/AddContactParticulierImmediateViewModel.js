var AddContactParticulierImmediateViewModel = null;

const FormChoise = {
    Default: 0,
    Yes: 1
};

function addContactParticulierImmediateViewModel(newContact) {
    self = this;

    self.newContact = ko.observable(newContact);

    self.Kinship = ko.observable("");
    self.IsSetJudge = ko.observable("");
    self.HelpNeeded = ko.observable("");

    self.checkAdd = function () {
        var missingData = false;
        $("input.required").each(function () {
            console.log($(this).attr('name'));
            if ($(this).is(':radio') && $('input[name=' + $(this).attr('name') + ']:checked').length == 0) {
                console.log("non");
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
        if (self.newContact().TypeMission == 1) {
            createEmptyDoc(self.newContact().VaultId, "Extrait casier judiciaire du ou des protecteurs proposé (s)", 82, "VaultContact", true);
            createEmptyDoc(self.newContact().VaultId, "Avis d'imposition du ou des protecteurs proposé (s)", 83, "VaultContact", true);
        }

        if (self.newContact().IsSetAskProtection == true || self.newContact().OpinionPro == true || self.newContact().TypeMission == 1) {
            var tmpName = self.newContact().LastName + " " + self.newContact().FirstName;
            createEmptyDoc(self.newContact().VaultId, "Copie pièce identité de " + tmpName, 81, "VaultContact", true);
        }

        if (self.newContact().HelpNeeded == 1) {
            createEmptyDoc(self.newContact().VaultId, "Jugement de mise sous protection", 37, "VaultContact");
        }
        //

        $("#add-contact-particulier-Immediate-form").submit();
    }

    AddContactParticulierImmediateViewModel = self;
}
