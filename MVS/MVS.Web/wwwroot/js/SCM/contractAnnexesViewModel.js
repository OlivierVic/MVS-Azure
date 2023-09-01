function ContractAnnexesViewModel(contractId, viewerUrl) {
    const self = this;

    self.contractId = contractId;
    self.viewerUrl = viewerUrl;
    self.selectedAnnex = ko.observable();
    self.annexes = ko.observableArray();

    self.isAnnexesLoading = ko.observable(false);
    self.isAnnexesModalLoading = ko.observable(false);

    self.importAnnexFromDriveModalViewModel = new ImportAnnexFromDriveModalViewModel({
        data: { contractId: self.contractId },
        onCreated: function () {
            self.getContractAnnexes()
        }
    });

    self.getContractAnnexes = function () {
        self.isAnnexesLoading(true);
        $.ajax({
            url: '/Contract/GetContractAnnexes',
            method: 'Get',
            data: {
                contractId: self.contractId
            },
            complete: function (d, s) {

                $(".sortable-annex").sortable({
                    items: '> .sort-annex',
                    update: function (e, ui) {
                        self.reorderAnnexes();
                    },
                });

                self.annexes(d.responseJSON);
                self.isAnnexesLoading(false);
            },
        });
    }

    self.selecteAnnex = function (id) {
        self.selectedAnnex(ko.toJS(self.annexes().find(annex => annex.id == id)));
    }

    // ImportAnnex Functions
    function isPdf(files) {
        var fileType = files[0].type;
        var validMimeTypes = ["application/pdf"];
        if (!validMimeTypes.includes(fileType)) {
            toastr.info(IMPORT_ANNEX_RESOURCES.PleaseImportAPdf);
            return false;
        }
        return true;
    }

    self.changeFileImportAnnex = function () {
        const target = '#importannex-file';
        if (!isPdf($(target)[0].files)) {
            return false;
        }

        var path = $(target).val();
        var pathSplit = path.split("\\");
        var filename = pathSplit[pathSplit.length - 1];
        $("#importannex-input").val(filename);
        $("#importannex-name").val(filename);
        return true;
    }

    self.checkImportAnnex = function () {
        self.isAnnexesModalLoading(true);

        if ($('#importannex-file').get(0).files.length === 0) {
            toastr.error(IMPORT_ANNEX_RESOURCES.MustChooseAnnex);
            self.isAnnexesModalLoading(false);
            return;
        }

        var name = $("#importannex-name").val();
        if (!name) {
            toastr.error(IMPORT_ANNEX_RESOURCES.MustGiveNameToAnnex);
            self.isAnnexesModalLoading(false);
            return;
        }

        $.ajax({
            url: '/Contract/CheckIfAnnexExist',
            method: 'Get',
            data: {
                name: name,
                contractId: self.contractId
            },
            complete: function (d, s) {
                if (d.responseJSON) {
                    toastr.error(IMPORT_ANNEX_RESOURCES.SameAnnexName);
                    self.isAnnexesModalLoading(false);
                } else {
                    self.importAnnex();
                }
            },
        });
    }

    self.importAnnex = function () {
        self.isAnnexesModalLoading(true);

        var form = document.querySelector("#importannex-form");
        var formdata = new FormData(form);

        $.ajax({
            url: '/Contract/ImportAnnex',
            method: 'Post',
            data: formdata,
            processData: false,
            contentType: false,
            complete: function (d, s) {
                $("#importannex-form").trigger("reset");
                $("#importannex-contractid").val(self.contractId);

                self.getContractAnnexes();
                $("#importannex-modal").modal("hide");
                self.isAnnexesModalLoading(false);
            },
        });
    }

    //DeleteAnnex Function
    self.DeleteAnnex = function () {
        self.isAnnexesModalLoading(true);
        $.ajax({
            url: '/Contract/DeleteAnnex',
            method: 'Post',
            data: {
                id: self.selectedAnnex().id,
            },
            complete: function (d, s) {
                self.getContractAnnexes();
                $("#removeannex-modal").modal("hide");
                self.isAnnexesModalLoading(false);
            },
        });
    }

    //ReorderAnnexes Function
    self.reorderAnnexes = function () {
        $(".sort-annex").each(function (i, elm) {
            var annexId = $(elm).attr("data-id");
            var annex = self.annexes().find(elem => elem.id == annexId);
            annex.order = i;
        });

        $.ajax({
            url: '/Contract/UpdateAnnexes',
            method: 'Post',
            data: {
                annexes: self.annexes(),
            }
        });
    }

    self.onAddAnnexeClick = function () {
        $("#createannex-modal").modal("show");
    }

    self.onImportAnnexeFromComputerClick = function (files) {
        if (files) {
            $("#importannex-file")[0].files = files;
            var canImportFile = self.changeFileImportAnnex();
            if (!canImportFile)
                return;
        }

        $("#importannex-modal").modal("show");
    }

    self.openImportAnnexFromDriveModal = function () {
        const viewModel = self.importAnnexFromDriveModalViewModel;
        viewModel.show();
        viewModel.refresh();
    }
}

function ImportAnnexFromDriveModalViewModel(args) {
    const self = this;

    self.modalElement = ko.observable();
    self.contractId = args.data.contractId;
    self.isLoading = ko.observable(false);
    self.filesTreeViewModel = new FilesTreeViewModel();
    self.annexName = ko.observable("");
    self.searchMethod = ko.observable("");
    self.searchMethods = {
        inTree: "searchInTree",
        byName: "searchByName",
    };
    self.searchName = ko.observable("");
    self.throttledSearchName = ko.pureComputed(() => self.searchName()).extend({ throttle: 500 });
    self.throttledSearchName.subscribe(() => self.refreshFileSearches());
    self.searchedFiles = ko.observableArray();
    self.selectedSearchedFile = ko.observable();
    self.isSearchingFiles = ko.observable(false);

    self.file = ko.pureComputed(() => {
        switch (self.searchMethod()) {
            case (self.searchMethods.inTree): {
                return self.filesTreeViewModel.selectedFile();
            }
            case (self.searchMethods.byName): {
                return self.selectedSearchedFile();
            }
        }
        return null;
    });

    self.fileId = ko.pureComputed(() => self.file()?.id?.());
    self.fileName = ko.pureComputed(() => self.file()?.name?.());
    self.fileName.subscribe(fileName => self.annexName(fileName));

    self.isValidOrToast = function (callback) {
        if (!self.fileId()) {
            toastr.error(IMPORT_ANNEX_RESOURCES.MustChooseAnnex);
            return;
        }

        if (!self.annexName()) {
            toastr.error(IMPORT_ANNEX_RESOURCES.MustGiveNameToAnnex);
            return;
        }

        self.isLoading(true);
        $.ajax({
            url: '/Contract/CheckIfAnnexExist',
            method: 'GET',
            data: {
                name: self.annexName,
                contractId: self.contractId,
            },
            complete: function (d, s) {
                self.isLoading(false);
                if (d.responseJSON) {
                    toastr.error(IMPORT_ANNEX_RESOURCES.SameAnnexName);
                } else {
                    callback();
                }
            },
        });
    }

    self.onCreateClick = function () {
        self.isValidOrToast(() => {
            self.isLoading(true);
            $.ajax({
                url: '/Contract/ImportAnnexFromDrive',
                method: 'POST',
                data: JSON.stringify({
                    contractId: ko.unwrap(self.contractId),
                    fileId: ko.unwrap(self.fileId),
                    name: ko.unwrap(self.annexName),
                }),
                contentType: 'application/json',
                success: function (data) {
                    self.hide();
                    args.onCreated();
                },
                error: function (request, _textStatus, errorThrown) {
                    let hasShownError = false;
                    if (errorThrown === "Bad Request") {
                        const errors = Object.values(request.responseJSON).flat();
                        errors.forEach(toastr.error);
                        hasShownError = errors.length > 0;
                    }
                    if (!hasShownError) {
                        toastr.error(IMPORT_ANNEX_RESOURCES.ErrorServer);
                    }
                },
                complete: function () {
                    self.isLoading(false);
                }
            });
        });
    }

    self.refreshFileSearches = function () {
        self.selectedSearchedFile(null);
        self.isSearchingFiles(true);
        $.ajax({
            url: '/Contract/SearchFilesForAnnex',
            method: 'POST',
            contentType: 'application/json',
            data: ko.mapping.toJSON({ searchName: self.searchName }),
            success: function (response) {
                ko.mapping.fromJS(response.files, {}, self.searchedFiles);
            },
            error: function () {
                self.searchedFiles([]);
            },
            complete: function () {
                self.isSearchingFiles(false);
            }
        })
    }

    self.reset = function () {
        self.annexName("");
    }

    self.show = function () {
        self.reset();
        $(self.modalElement()).modal('show');
    }

    self.hide = function () {
        $(self.modalElement()).modal('hide');
    }

    self.refresh = function () {
        self.filesTreeViewModel.refresh();
    }
}
