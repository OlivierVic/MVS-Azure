var AdminAddUserFolderViewModel = null;

function adminAddUserFolderViewModel() {
    self = this;

    self.userId = ko.observable();
    self.vaultId = ko.observable();
    /*self.folders = ko.observable({});*/
    /*self.currentPage = ko.observable(1);
    self.nbPages = ko.observable(1);
    self.pages = ko.observable();*/

    /*self.currentPage.subscribe(function () {
        self.getFolders();
    });

    self.sort = ko.observable();
    self.sort.subscribe(function () {
        self.getFolders();
    });*/

    self.addUserFolder = function () {
        $.ajax({
            url: '/Admin/AddUserFolder?handler=AddUserOnFolder',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                UserId: self.userId(),
                VaultId: self.vaultId(),
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    AdminAddUserFolderViewModel = self;
}
