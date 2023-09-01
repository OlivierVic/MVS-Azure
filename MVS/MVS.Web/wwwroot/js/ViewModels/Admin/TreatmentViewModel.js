var TreatmentViewModel = null;

function treatmentViewModel() {
    self = this;

    self.folders = ko.observable({});
    self.currentPage = ko.observable(1);
    self.nbPages = ko.observable(1);
    self.pages = ko.observable();

    self.currentPage.subscribe(function () {
        self.getFolders();
    });

    self.sort = ko.observable();
    self.sort.subscribe(function () {
        self.getFolders();
    });

    self.selectedFolder = ko.observable({ id: "" });

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

    self.getFolders = function () {
        $.ajax({
            url: '/Admin/Treatment?handler=Folders',
            type: 'GET',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                currentPage: self.currentPage(),
                sort: self.sort(),
            },
            success: function (data) {
                data.folders.forEach(f => f.creationDate = new Date(f.creationDate));
                self.folders(data.folders);
                self.nbPages(data.nbPages);
                self.getPagesList();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.updateFolderArchiveStatus = function (archive) {
        $.ajax({
            url: '/Vault?handler=UpdateFolderArchiveStatus',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.selectedFolder().id,
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

    self.deleteFolder = function (archive) {
        $.ajax({
            url: '/Admin/Treatment?handler=DeleteVault',
            type: 'Post',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.selectedFolder().id
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.getFolders();

    TreatmentViewModel = self;
}
