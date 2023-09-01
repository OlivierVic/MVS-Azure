var ContactByEmailViewModel = null;

function contactByEmailViewModel() {
    self = this;

    self.questionData = {
        subject: ko.observable(""),
        folderNumber: ko.observable(""),
        question: ko.observable(""),
        acceptPolitic: ko.observable(false)
    }

    self.sendEmail = function () {
        if (self.questionData.subject() == "" || self.questionData.folderNumber() == "" || self.questionData.question() == "") {
            toastr.error("Vous devez complétez tous les champs pour continuer");
            return;
        }

        if (!self.questionData.acceptPolitic()) {
            toastr.error("Vous devez acceptez la politique de confidentialité pour continuer");
            return;
        }

        $.ajax({
            url: '/ContactByEmail?handler=SendEmail',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                subject: self.questionData.subject(),
                folderNumber: self.questionData.folderNumber(),
                question: self.questionData.question()
            },
            success: function () {
                self.questionData.subject("");
                self.questionData.folderNumber("");
                self.questionData.question("");
                self.questionData.acceptPolitic(false);
                toastr.success("Votre question a été envoyée");
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    ContactByEmailViewModel = self;
}
