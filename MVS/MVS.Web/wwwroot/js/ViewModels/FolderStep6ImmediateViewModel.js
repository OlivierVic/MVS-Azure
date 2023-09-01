var FolderStep6ImmediateViewModel = null;

function folderStep6ImmediateViewModel(folder) {
    self = this;

    self.folder = folder;
    self.viewerUrl = ko.observable(null);

    self.selectedDepartment = ko.observable();
    self.selectedDepartment.subscribe(function (newValue) {
        if (newValue != 'null') {
            $.ajax({
                url: '/Vault/FolderStep6Immediate?handler=DoctorsListUrl',
                type: 'Get',
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: {
                    department: newValue,
                    folderId: self.folder.Id,
                },
                success: function (data) {
                    self.viewerUrl(data.url);
                },
                error: function () {
                    toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
                }
            });
        }
        else {
            self.viewerUrl(null);
        }
    });

    self.selectedDepartment(self.folder.RegistrationDepartment);

    self.goToSCM = function (scmContractId) {
        window.open("/SCM?scmContractId=" + scmContractId + "&showForm=false", '_blank');
    }

    FolderStep6ImmediateViewModel = self;
}
