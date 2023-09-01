const genericTreeChildTypes = {
    folder: "folder",
    file: "file"
};

function GenericTreeChildViewModel(args) {
    const self = this;

    self.id = ko.observable(null);
    self.name = ko.observable("");

    ko.mapping.fromJS(args.data, {}, self);

    self.type = ko.observable(args.type);
    self.isExtended = ko.observable(false);
    self.isLoading = ko.observable(false);
    self.children = ko.observableArray();

    self.isFolder = ko.pureComputed(() => self.type() === genericTreeChildTypes.folder);
    self.isFile = ko.pureComputed(() => self.type() === genericTreeChildTypes.file);
    self.isPdf = ko.pureComputed(() => self.isFile() && self.name().endsWith(".pdf"));
}

function GenericTreeViewModel() {
    const self = this;

    self.showFiles = ko.observable(true);

    self.rootFolder = new GenericTreeChildViewModel({
        type: genericTreeChildTypes.folder
    });

    self.onAngleClick = function (folderData) {
        if (folderData.isExtended()) {
            folderData.isExtended(false);
        } else {
            self.getChildren(folderData, function () {
                if (folderData.children().length) {
                    folderData.isExtended(true);
                }
            });
        }
    }

    self.isChildSelected = function (folderData) {
        return false;
    }

    self.onFolderClick = function (folderData) {
    }

    self.onFileClick = function (fileData) {
    }

    self.getChildren = function (folderData, success) {
        const parentFolderId = folderData.id();
        folderData.isLoading(true);

        const showFiles = self.showFiles();
        $.ajax({
            method: "GET",
            url: `/Drive/GetFoldersTreeChildren?folderId=${parentFolderId || ''}&getFiles=${showFiles}`,
            dataType: "json",
            success: function (response) {
                const { folders, files } = response;
                folders.sort((lhs, rhs) => lhs.name > rhs.name);
                const tuples = [{ dataList: folders, type: genericTreeChildTypes.folder }];
                if (showFiles) {
                    files.sort((lhs, rhs) => lhs.name > rhs.name);
                    tuples.push({ dataList: files, type: genericTreeChildTypes.file })
                }

                let language = 'fr';
                const cookieMarker = '.AspNetCore.Culture=c=';
                if (document.cookie.indexOf(cookieMarker) !== -1) {
                    language = document.cookie.substring(document.cookie.indexOf(cookieMarker) + cookieMarker.length, document.cookie.indexOf('|'));
                }

                const children = tuples.flatMap(({ dataList, type }) => {
                    return dataList.map(data => {
                        if (data.name.indexOf(String.fromCharCode(164)) !== -1) {
                            const translations = data.name.split(String.fromCharCode(164));
                            data.displayName = language === 'fr' ? translations[0] : translations[1] ?? translations[0];
                        } else {
                            data.displayName = data.name;
                        }
                        return new GenericTreeChildViewModel({ data, type });
                    });
                });

                folderData.children(children);
                success?.();
            },
            complete: function () {
                folderData.isLoading(false);
            }
        })
    };

    self.refresh = function () {
        self.getChildren(self.rootFolder);
    }
}

function FoldersTreeViewModel(args) {
    const self = this;
    GenericTreeViewModel.call(self, args);

    self.showFiles(false);

    self.selectedFolder = ko.observable();

    self.isChildSelected = function (childData) {
        return childData === self.selectedFolder();
    }

    self.onFolderClick = function (folderData) {
        self.selectedFolder(folderData)
    }
}

function FilesTreeViewModel(args) {
    const self = this;
    GenericTreeViewModel.call(self, args);

    self.selectedFile = ko.observable();

    self.isChildSelected = function (childData) {
        return childData === self.selectedFile();
    }

    self.onFileClick = function (fileData) {
        self.selectedFile(fileData)
    }
}
