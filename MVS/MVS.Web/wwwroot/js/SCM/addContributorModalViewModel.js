function AddContributorModalViewModel(fields, rights, contractId, contractName, currentStep, stepsNumber) {
    const self = this;

    self.loader = new LoaderViewModel();
    self.contractId = ko.observable(contractId);
    self.contractName = ko.observable(contractName)
    self.currentStep = ko.observable(currentStep);
    self.fields = ko.observableArray(fields);
    self.shouldDisplaySignRight = ko.pureComputed(() => {
        // Is it the last step ?
        return self.currentStep() === stepsNumber - 1;
    });
    self.rights = ko.computed(() => {
        if (self.shouldDisplaySignRight()) {
            return rights;
        }
        const newRights = [...rights];
        newRights.pop();
        return newRights;
    })

    self.selectedField = ko.observable();
    self.selectedRight = ko.observable();
    self.email = ko.observable();
    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.isExternalField = ko.pureComputed(() => {
        const field = self.fields().find(f => f.id === self.selectedField()).title;
        return field === "Extern";
    });

    self.initials = ko.pureComputed(() => {
        return (self.firstName() ? self.firstName()[0] : '') + (self.lastName() ? self.lastName()[0] : '');
    });
    self.fullName = ko.pureComputed(() => {
        return (self.firstName() ? self.firstName() : '') + ' ' + (self.lastName() ? self.lastName() : '');
    });

    self.canSave = ko.pureComputed(() => {
        return self.firstName() !== null && self.lastName() !== null && self.email() !== null;
    });

    self.onUserChange = (e, data) => {
        self.email(data.item.data.email);
        self.lastName(data.item.data.lastName);
        self.firstName(data.item.data.firstName);

        if (data.item.data.email === "@Author") {
            const authorFieldId = self.fields().find(e => e.title === "Author").id;
            self.selectedField(authorFieldId);
        }
    }

    self.getAvailableUsers = (searchTerm, callback) => {
        $.ajax({
            dataType: "json",
            url: "/Search/SearchUsersForWorkflowStep",
            data: {
                query: searchTerm,
                field: self.fields().find(f => f.id === self.selectedField()).title,
                withAuthor: true
            },
        }).done(callback);
    }

    self.selectedField.subscribe((newValue) => {
        const authorFieldId = self.fields().find(e => e.title === "Author").id;
        if (newValue === authorFieldId) {
            self.email("@Author");
            self.firstName("Auteur");
            self.lastName("");
            return;
        }

        self.email(null);
        self.lastName(null);
        self.firstName(null);
        $("#selectedUserInput").val("");
    });

    self.onSave = () => {
        if (!self.canSave()) {
            toastr.warning(Resources?.["InformationMissing"] || "Certaines informations sont manquantes.");
            return;
        }
        self.loader.switchLoading();
        $.ajax({
            url: "/Contract/AddContributorToWorkflowStep",
            method: "POST",
            data: JSON.stringify({
                email: ko.unwrap(self.email),
                firstName: ko.unwrap(self.firstName),
                lastName: ko.unwrap(self.lastName),
                fieldId: ko.unwrap(self.selectedField),
                rights: ko.unwrap(self.selectedRight),
                contractId: ko.unwrap(self.contractId),
                contractName: ko.unwrap(self.contractName),
                currentStep: ko.unwrap(self.currentStep)
            }),
            contentType: 'application/json',
            success: (data) => {
                if (!data.wasAdded) {
                    toastr.error(Resources?.["ContributorAlreadyPresent"] || "Le contributeur est déjà présent dans l'étape courante.");
                    return;
                }
                window.location.reload();
            },
            error: (err) => {
                toastr.error(Resources?.["ErrorOccured"] || "Une erreur est survenue.");
            },
            complete: () => {
                self.loader.switchLoading();
            }
        });
    }
}
