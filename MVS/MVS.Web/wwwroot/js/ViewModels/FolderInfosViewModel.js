var FolderInfosViewModel = null;

function folderInfosViewModel(folder) {
    self = this;

    self.folder = ko.mapping.fromJS(folder);
    //self.folder.DateOfBirth(self.folder.DateOfBirth().substring(0, 10));

    //Update contact property
    self.updateProperty = function () {
        var data = {
            folderId: self.folder.Id(),
            sex: self.folder.Sex(),
            firstName: self.folder.FirstName(),
            lastName: self.folder.LastName(),
            birthName: self.folder.BirthName(),
        };

        self.ajaxUpdate("UpdateProperty", data);
    }

    //Update contact identity property
    self.updateFolderIdentity = function () {
        var data = {
            folderId: self.folder.Id(),
            dateOfBirth: self.folder.BirthDate(),
            placeOfBirth: self.folder.BirthLocation(),
        };

        self.ajaxUpdate("UpdateIdentity", data);
    }

    //Update contact contact property
    self.updateFolderProperty = function () {
        var data = {
            folderId: self.folder.Id(),
            address: self.folder.Address(),
            zipCodeAndCity: self.folder.ZipceCodeAndCity(),
            country: self.folder.Country(),
            phoneNumber: self.folder.PhoneNumber(),
            email: self.folder.Email(),
        };

        self.ajaxUpdate("UpdateContact", data);
    }

    self.ajaxUpdate = function (handler, data) {
        $.ajax({
            url: '/Vault/UpdateFolder?handler=' + handler,
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: data,
            success: function () {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    FolderInfosViewModel = self;
}
