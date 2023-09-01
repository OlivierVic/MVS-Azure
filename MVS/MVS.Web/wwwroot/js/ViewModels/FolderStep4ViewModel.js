var FolderStep4ViewModel = null;

function folderStep4ViewModel(folder, newMeasure) {
    self = this;

    self.folder = folder;
    self.measures = ko.observableArray();

    folder.FolderStep4Measures.forEach(element => self.measures.push(ko.mapping.fromJS(element)));

    self.newMeasure = newMeasure;

    self.addNotAdvisedMeasure = function () {
        var mesureNotAdvised = ko.mapping.fromJS(newMeasure);
        mesureNotAdvised.Advised(false);
        self.measures.push(mesureNotAdvised);
    }

    if (folder.Field == 1) {
        if (self.measures().length == 0) {
            var mesureAdvised1 = ko.mapping.fromJS(newMeasure);
            mesureAdvised1.Advised(true);
            self.measures.push(mesureAdvised1);

            var mesureAdvised2 = ko.mapping.fromJS(newMeasure);
            mesureAdvised2.Advised(true);
            self.measures.push(mesureAdvised2);
        }
    }

    self.saveMeasures = function () {
        $.ajax({
            url: '/Vault/FolderStep4?handler=SaveMeasures',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                measures: self.measures(),
                folderId: self.folder.Id,
            },
            success: function (data) {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.goToSCM = function(scmContractId) {
        window.open("/SCM?scmContractId=" + scmContractId + "&showForm=false", '_blank');
    }
    
    FolderStep4ViewModel = self;
}
