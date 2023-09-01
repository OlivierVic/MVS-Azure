var AddressBookViewModel = null;

function addressBookViewModel(folder, returnUrl) {
    self = this;

    self.folder = ko.mapping.fromJS(folder);
    self.job = ko.observable("");
    self.otherJob = ko.observable("");
    self.kinship = ko.observable("");
    self.other = ko.observable("");

    self.returnUrl = returnUrl;

    self.save = function () {
        $.ajax({
            url: '/Vault/AddressBook?handler=Completed',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultId: self.folder.Id(),
                completedContactProfessional: self.folder.CompletedContactProfessional(),
                completedContactProche: self.folder.CompletedContactProche(),
            },
            success: function (data) {
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.createEmptyContactPro = function () {
        $.ajax({
            url: '/Vault/AddressBook?handler=CreateEmptyContactPro',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultId: self.folder.Id(),
                jobId: self.job,
                otherJob: self.otherJob,
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.createEmptyContactParticular = function () {
        $.ajax({
            url: '/Vault/AddressBook?handler=CreateEmptyContactParticular',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultId: self.folder.Id(),
                kinship: self.kinship,
                other: self.other,
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.sendAdvice = function (contactId) {
        $.ajax({
            url: '/Vault/AddressBook?handler=SendAdvice',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                contactId
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    AddressBookViewModel = self;
}
