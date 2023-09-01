var FolderStep7ViewModel = null;

function folderStep7ViewModel(folder) {
    self = this;

    self.folder = ko.mapping.fromJS(folder);
    self.folderSendDate = new Date(self.folder.SendDate());
    self.folderDateOfHearing = new Date(self.folder.DateOfHearing());
    self.folderTimeOfHearing = new Date('0000-01-01T' + self.folder.TimeOfHearing());

    /*console.log(self.folderTimeOfHearing);*/

    self.updateFolderDocumentDownload = function () {
        var DocumentDownloadPress = true;
        $.ajax({
            url: '/Vault/FolderStep7?handler=UpdateFolderDocumentDownload',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                documentDownload: DocumentDownloadPress,
            },
            success: function (data) {
                window.open("/Vault/FolderStep7?handler=DownloadAllFolderDocument&folderId=" + self.folder.Id(), '_blank');
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.updateFolderSendDate = function () {
        $.ajax({
            url: '/Vault/FolderStep7?handler=UpdateFolderSendDate',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                sendDate: self.folder.SendDate(),
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.updateFolderInfoTiers = function () {
        var InfoTiersPress = true;
        $.ajax({
            url: '/Vault/FolderStep7?handler=UpdateFolderInfoTiers',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                infoTiers: InfoTiersPress,
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.updateFolderDateOfHearing = function () {
        $.ajax({
            url: '/Vault/FolderStep7?handler=UpdateFolderDateOfHearing',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                dateOfHearing: self.folder.DateOfHearing(),
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.updateFolderTimeOfHearing = function () {
        $.ajax({
            url: '/Vault/FolderStep7?handler=UpdateFolderTimeOfHearing',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                timeOfHearing: self.folder.TimeOfHearing(),
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    FolderStep7ViewModel = self;
}
