var FolderStep2ViewModel = null;

function folderStep2ViewModel(folderId) {
    self = this;

    self.folderId = folderId;

    self.selectedContractId = ko.observable("");
    self.selectedContractName = ko.observable("");

    self.iban = ko.observable("");
    self.bic = ko.observable("");
    self.city = ko.observable("");

    self.sendingToSignature = ko.observable(false);

    self.PayedByBankTransfert = function () {
        $.ajax({
            url: '/Vault/FolderStep2?handler=PaymentBankTransfert',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folderId,
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.PayedByDebits = function () {
        $.ajax({
            url: '/Vault/FolderStep2?handler=PaymentDebits',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folderId,
                IBAN: self.iban(),
                BIC: self.bic(),
                City: self.city()
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.openSignatureModal = function (contractId, contractName) {
        self.selectedContractId(contractId);
        self.selectedContractName(contractName);
        $("#signatureModal").modal('show');
    }

    self.SendToSignature = function () {
        self.sendingToSignature(true);
        $.ajax({
            url: '/Vault/FolderStep2?handler=SendToSignature',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folderId,
                contractId: self.selectedContractId(),
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                self.sendingToSignature(false);
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    FolderStep2ViewModel = self;
}
