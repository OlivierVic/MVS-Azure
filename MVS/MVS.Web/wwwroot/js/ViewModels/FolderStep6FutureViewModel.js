var FolderStep6FutureViewModel = null;

function folderStep6FutureViewModel(folder) {
    self = this;

    self.folder = folder;

    FolderStep6FutureViewModel = self;

    self.sendEmailTier = function () {
        $.ajax({
            url: '/Vault/FolderStep6Future?handler=SendEmailTier',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id,
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }
}
