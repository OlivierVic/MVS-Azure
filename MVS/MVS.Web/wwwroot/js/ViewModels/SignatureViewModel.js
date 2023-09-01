var SignatureViewModel = null;

function signatureViewModel(folderId) {
    self = this;

    self.folderId = folderId;

    self.sendToSignature = function (contractId) {
        $.ajax({
            url: '/Vault/FolderStep2?handler=SendTosignature',
            type: 'GET',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folderId,
                contractId
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    SignatureViewModel = self;
}
