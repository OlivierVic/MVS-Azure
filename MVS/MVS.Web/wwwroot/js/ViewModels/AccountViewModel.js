var AccountViewModel = null;

function accountViewModel(user) {
    self = this;

    self.user = ko.mapping.fromJS(user);



    //Update user notif settings
    self.updateNotifSettings = function () {
        $.ajax({
            url: '/Account?handler=UpdateNotifSettings',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                userId: self.user.Id(),
                notifAnswerQuestions: self.user.NotifAnswerQuestions(),
                notifAppointment: self.user.NotifAppointment(),
                notifFileProgress: self.user.NotifFileProgress(),
                notifOpinion: self.user.NotifOpinion()
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }



    //Update user simple property
    self.selectedProperty = {
        pronoun: ko.observable(""),
        label: ko.observable(""),
        placeholder: ko.observable(""),
        value: ko.observable(""),
        propertyName: ko.observable(""),
    }
    self.openPropertyModal = function (pronoun, label, propertyName, placeholder) {
        self.selectedProperty.pronoun(pronoun);
        self.selectedProperty.label(label);
        self.selectedProperty.propertyName(propertyName);
        self.selectedProperty.placeholder(placeholder);
        self.selectedProperty.value("");
        $("#propertyModifyModal").modal('show');
    }
    self.updateProperty = function () {
        if (self.selectedProperty.value() == "") {
            toastr.error("Veuillez remplir le champs pour continuer");
            return;
        }
        $.ajax({
            url: '/Account?handler=UpdateProperty',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                userId: self.user.Id(),
                value: self.selectedProperty.value(),
                propertyName: self.selectedProperty.propertyName(),
            },
            success: function () {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }



    //Update user identity
    self.identityData = {
        firstname: ko.observable(self.user.FirstName()),
        lastname: ko.observable(self.user.LastName())
    };
    self.updateIdentity = function () {
        if (self.identityData.firstname() == "" || self.identityData.lastname() == "") {
            toastr.error("Veuillez remplir les champs pour continuer");
            return;
        }
        $.ajax({
            url: '/Account?handler=UpdateIdentity',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                userId: self.user.Id(),
                firstname: self.identityData.firstname(),
                lastname: self.identityData.lastname()
            },
            success: function () {
                window.location.reload();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }



    //Update user email
    self.emailData = {
        oldEmail: ko.observable(""),
        newEmail: ko.observable(""),
        password: ko.observable("")
    };
    self.updateEmail = function () {
        if (self.emailData.oldEmail() == "" || self.emailData.newEmail() == "" || self.emailData.password() == "") {
            toastr.error("Veuillez remplir les champs pour continuer");
            return;
        }

        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!regex.test(self.emailData.oldEmail()) || !regex.test(self.emailData.newEmail())) {
            toastr.error("Les emails ne sont pas valides");
            return;
        }

        if (self.emailData.oldEmail() != self.user.Email()) {
            toastr.error("L'ancienne adresse email est erronée");
            return;
        }

        $.ajax({
            url: '/Account?handler=UpdateEmail',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                userId: self.user.Id(),
                email: self.emailData.newEmail(),
                password: self.emailData.password(),
            },
            success: function () {
                window.location.reload();
            },
            error: function (response) {
                if (response.status == 400) {
                    toastr.error("Le mot de passe est erroné");
                }
                else {
                    toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
                }
            }
        });
    }



    //Update user password
    self.passwordData = {
        email: ko.observable(""),
        oldPassword: ko.observable(""),
        newPassword: ko.observable(""),
        confirmPassword: ko.observable("")
    };
    self.updatePassword = function () {
        if (self.passwordData.oldPassword() == "" || self.passwordData.newPassword() == "" || self.passwordData.email() == "" || self.passwordData.confirmPassword() == "") {
            toastr.error("Veuillez remplir les champs pour continuer");
            return;
        }

        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!regex.test(self.passwordData.email()) || self.passwordData.email() != self.user.Email()) {
            toastr.error("L'email de connexion n'est pas valide ou est erronée");
            return;
        }

        if (self.passwordData.newPassword() != self.passwordData.confirmPassword()) {
            toastr.error("Les deux mots de passe ne sont pas identiques");
            return;
        }

        $.ajax({
            url: '/Account?handler=UpdatePassword',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                userId: self.user.Id(),
                oldPassword: self.passwordData.oldPassword(),
                newPassword: self.passwordData.newPassword(),
            },
            success: function () {
                window.location.reload();
            },
            error: function (response) {
                console.log(response);
                if (response.status == 400 && response.responseText == "oldPassword") {
                    toastr.error("Le mot de passe actuel est erroné");
                }
                else if (response.status == 400 && response.responseText == "newPassword") {
                    toastr.error("Le nouveau mot de passe n'est pas valide");
                    $("#password-error").removeClass("d-none");
                }
                else {
                    toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
                }
            }
        });
    }

    AccountViewModel = self;
}
