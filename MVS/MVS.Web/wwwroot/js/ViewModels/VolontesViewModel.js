var VolontesViewModel = null;

function volontesViewModel(folderId, nbElem) {
    self = this;

    self.folderId = folderId;

    self.selectedContractId = ko.observable("");
    self.selectedContractName = ko.observable("");

    self.sendingToSignature = ko.observable(false);
    
    self.openSignatureModal = function (contractId, contractName) {
        self.selectedContractId(contractId);
        self.selectedContractName(contractName);
        $("#signatureModal").modal('show');
    }
    /* --------------------------------------------------------------------------------------- */
    self.documents = ko.observable({});
    self.currentPage = ko.observable(1);
    self.nbPages = ko.observable(1);
    self.pages = ko.observable();
    self.nbElem = nbElem;

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
            url: '/Vault/Volontes?handler=Documents',
            type: 'GET',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folderId,
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
            url: '/Vault/Volontes?handler=DeleteDocument',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderId: self.folderId,
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

    self.SendToSignature = function () {
        self.sendingToSignature(true);
        $.ajax({
            url: '/Vault/Volontes?handler=SendToSignature',
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

    VolontesViewModel = self;
}
