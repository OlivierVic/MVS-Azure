var IndexFolderViewModel = null;

function indexFolderViewModel() {
    self = this;

    self.vaults = ko.observable({});
    self.currentPage = ko.observable(1);
    self.nbPages = ko.observable(1);
    self.pages = ko.observable();

    self.currentPage.subscribe(function () {
        self.getVaults();
    });

    self.sort = ko.observable();
    self.sort.subscribe(function () {
        self.getVaults();
    });

    self.selectedVault = ko.observable({id: ""});

    self.getPagesList = function () {
        const boundaryCount = 2;
        const siblingCount = 1;

        const range = (start, end) => {
            const length = end - start + 1;
            return Array.from({ length }, (_, i) => start + i);
        };
        const startPages = range(1, Math.min(boundaryCount, self.nbPages()));
        const endPages = range(Math.max(self.nbPages() - boundaryCount + 1, boundaryCount + 1), self.nbPages());
        const siblingsStart = Math.max(
            Math.min(
                self.currentPage() - siblingCount,
                self.nbPages() - boundaryCount - siblingCount,
            ),
            boundaryCount + 2,
        );
        const siblingsEnd = Math.min(
            Math.max(
                self.currentPage() + siblingCount,
                boundaryCount + siblingCount,
            ),
            endPages[0] - 2,
        );
        const itemList = [
            ...startPages,
            ...(siblingsStart > boundaryCount + 2
                ? ['...']
                : boundaryCount + 1 < self.nbPages() - boundaryCount
                    ? [boundaryCount + 1]
                    : []),
            ...range(siblingsStart, siblingsEnd),
            ...(siblingsEnd < self.nbPages() - boundaryCount - 1
                ? ['...']
                : self.nbPages() - boundaryCount > boundaryCount
                    ? [self.nbPages() - boundaryCount]
                    : []),
            ...endPages,
        ];

        self.pages(itemList);
    }

    self.getVaults = function()
    {
        $.ajax({
            url: '/Vault?handler=Vaults',
            type: 'GET',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                currentPage: self.currentPage(),
                sort: self.sort(),
            },
            success: function (data) {
                data.vaults.forEach(f => f.creationDate = new Date(f.creationDate));
                self.vaults(data.vaults);
                self.nbPages(data.nbPages);
                self.getPagesList();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.updateVaultArchiveStatus = function (archive) {
        $.ajax({
            url: '/Vault?handler=UpdateVaultArchiveStatus',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.selectedVault().id,
                archive
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.deleteVault = function (archive) {
        $.ajax({
            url: '/Vault?handler=DeleteVault',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.selectedVault().id
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.launchEval = function (folderId)
    {
        $.ajax({
            url: '/Vault?handler=LaunchEval',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId
            },
            success: function (data) {
                window.location.href = "/Vault/FolderStep4?folderId=" + folderId;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.getVaults();

    IndexFolderViewModel = self;
}
