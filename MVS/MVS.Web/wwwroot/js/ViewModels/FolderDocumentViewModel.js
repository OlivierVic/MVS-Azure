var FolderDocumentViewModel = null;

function folderDocumentViewModel(Vault, nbElem, returnUrl) {
    self = this;

    self.folder = ko.mapping.fromJS(Vault);

    self.documents = ko.observable({});
    self.currentPage = ko.observable(1);
    self.nbPages = ko.observable(1);
    self.pages = ko.observable();
    self.nbElem = nbElem;
    self.returnUrl = returnUrl;

    self.testDocument = {
        title: ko.observable(''),
    };


    self.uploadDocument = {
        title: ko.observable(''),
        type: ko.observable(''),
        typeName: ko.observable(''),
        folderDocumentId: ko.observable(''),
    };

    self.OpenUploadDocument = function (title, type, typeName, folderDocumentId) {
        self.uploadDocument.title(title);
        self.uploadDocument.type(type);
        self.uploadDocument.typeName(typeName);
        self.uploadDocument.folderDocumentId(folderDocumentId);
    };

    self.currentPage.subscribe(function () {
        self.getDocuments();
    });

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

    self.getDocuments = function () {
        $.ajax({
            url: '/Vault/Documents?handler=Documents',
            type: 'GET',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                currentPage: self.currentPage(),
                nbElem: self.nbElem,
            },
            success: function (data) {
                self.documents(data.documents);
                self.nbPages(data.nbPages);
                self.getPagesList();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }
    self.getDocuments();

    self.deleteDocuments = function (name, type, typeName, id, url) {
        $.ajax({
            url: '/Vault/Documents?handler=DeleteDocument',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                documentName: name,
                type: type,
                documentTypeName: typeName,
                documentId: id,
                documentUrl: url,
            },
            success: function (data) {
                toastr.success("Le document supprimé avec sucess");
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.save = function () {
        $.ajax({
            url: '/Vault/Documents?handler=Completed',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folder.Id(),
                completedDocument: self.folder.CompletedDocument(),
            },
            success: function (data) {
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    FolderDocumentViewModel = self;
}
