var ContactInfosViewModel = null;

function contactInfosViewModel(contact) {
    self = this;

    self.contact = ko.mapping.fromJS(contact);
    if (!self.contact.Ispro()) {
        if (self.contact.DateOfBirth() != null) {
            self.contact.DateOfBirth(self.contact.DateOfBirth().substring(0, 10));
        }
    }

    self.password = ko.observable('');

    //Update contact property
    self.updateProperty = function () {
        var data = {
            contactId: self.contact.Id(),
            sex: self.contact.Sex(),
            firstName: self.contact.FirstName(),
            lastName: self.contact.LastName(),
        };

        self.ajaxUpdate("UpdateProperty", data);
    }

    //Update contact identity property
    self.updateContactIdentity = function () {
        var data = {
            contactId: self.contact.Id(),
            kinship: self.contact.Kinship(),
            dateOfBirth: self.contact.DateOfBirth(),
            placeOfBirth: self.contact.PlaceOfBirth(),
            nationality: self.contact.Nationality(),
            unknownMother: self.contact.UnknownMother(),
            firstLastNameMother: self.contact.FirstLastNameMother(),
            unknownFather: self.contact.UnknownFather(),
            firstLastNameFather: self.contact.FirstLastNameFather(),
            adoptedPerson: self.contact.AdoptedPerson(),
        };

        self.ajaxUpdate("UpdateIdentity", data);
    }

    //Update contact contact property
    self.updateContactProperty = function () {
        var data = {
            contactId: self.contact.Id(),
            addres: self.contact.Addres(),
            zipCodeAndCity: self.contact.ZipCodeAndCity(),
            country: self.contact.Country(),
            phoneNumber: self.contact.PhoneNumber(),
            email: self.contact.Email(),
        };

        self.ajaxUpdate("UpdateContact", data);
    }

    //Update contact activity property
    self.updateContactActivity = function () {
        var data = {
            contactId: self.contact.Id(),
            jobId: self.contact.JobProfessionel(),
            otherJob: self.contact.OtherJob(),
            company: self.contact.Company(),
            accompaniment: self.contact.Accompaniment(),
        };

        self.ajaxUpdate("UpdateActivity", data);
    }

    //Update contact relationship property
    self.updateContactRelationship = function () {
        var data = {
            contactId: self.contact.Id(),
            relationshipQuality: self.contact.RelationshipQuality(),
            relationshipFrequencies: self.contact.RelationshipFrequencies(),
        };

        self.ajaxUpdate("UpdateRelationship", data);
    }

    //Update contact immediate protection property
    self.updateRoleImmediateProtection = function () {
        var data = {
            contactId: self.contact.Id(),
            isFolderAdmin: self.contact.IsFolderAdmin(),
            isSetAskProtection: self.contact.IsSetAskProtection(),
            isSetJudge: self.contact.IsSetJudge(),
            adviceStatus: self.contact.AdviceStatus(),
            Confidence: self.contact.Confidence(),
            TypeMission: self.contact.TypeMission(),
        };

        self.ajaxUpdate("UpdateRoleImmediateProtection", data);
    }

    //Update contact immediate protection property
    self.updateRoleFutureProtection = function () {
        var data = {
            contactId: self.contact.Id(),
            isFolderAdmin: self.contact.IsFolderAdmin(),
            isFutuAgent: self.contact.IsFutuAgent(),
            /*agentMission: self.contact.AgentMission(),*/
            protectPeople: self.contact.ProtectPeople(),
            protectAllProperty: self.contact.ProtectAllProperty(),
            protectOfCertainGoods: self.contact.ProtectOfCertainGoods(),
            propertyDetails: self.contact.PropertyDetails(),
            Confidence: self.contact.Confidence(),
            infoPro: self.contact.InfoPro(),
            typeMission: self.contact.TypeMission(),
        };

        self.ajaxUpdate("UpdateRoleFutureProtection", data);
    }

    //Update contact details property
    self.updateContactDetails = function () {
        var data = {
            contactId: self.contact.Id(),
            details: self.contact.ContactDetails(),
        };

        self.ajaxUpdate("UpdateDetails", data);
    }

    self.ajaxUpdate = function (handler, data) {
        $.ajax({
            url: '/Vault/AddressBookContactInfos?handler=' + handler,
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: data,
            success: function () {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    self.deleteContact = function () {
        if (self.password() == '') {
            $("#password-div").addClass('errorHere');
            $("#password-div > .msgError").text('Ce champs est obligatoire');

            return;
        }

        $.ajax({
            url: '/Vault/AddressBookContactInfos?handler=DeleteContact',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                contactId: self.contact.Id(),
                password: self.password(),
            },
            success: function () {
                window.location.href = '/Vault/AddressBook?folderId=' + self.contact.VaultId();
            },
            error: function () {
                $("#password-div").addClass('errorHere');
                $("#password-div > .msgError").text('Le mot de passe n\'est pas bon');
            }
        });
    }

    ContactInfosViewModel = self;
}
