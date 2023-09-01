var AnticipationMeasuresViewModel = null;

var AnticipationMeasuresFormSpecialsPossibleAsnwer = {
    OrganizedFuneral: 77,
    BuyingBurialPlot: 78,
    SubscribeFuneralAgreement: 79,
    SharedDonation: 82,
}

function anticipationMeasuresViewModel(questions, folderAnticipationMeasuresInfo, returnUrl) {
    self = this;

    self.folderAnticipationMeasuresInfo = ko.mapping.fromJS(folderAnticipationMeasuresInfo);
    self.questions = questions;
    self.questions.forEach(question => question.AnswersAnticipationMeasures.forEach(a => a.Selected = ko.observable(a.Selected)));
    self.questions.forEach(question => question.AnswersAnticipationMeasures = ko.observableArray(question.AnswersAnticipationMeasures));
    self.returnUrl = returnUrl;

    self.addNewAnswer = function (questionId) {
        var newAnswer = {
            Id: 0,
            QuestionId: questionId,
            VaultId: self.folderAnticipationMeasuresInfo.VaultId(),
            Comment: "",
            Data: "",
            Answer: null
        };
        self.questions.find(q => q.Id == questionId).AnswersAnticipationMeasures.push(newAnswer);
    }

    self.removeAnswer = function (questionId, answer) {
        self.questions.find(q => q.Id == questionId).AnswersAnticipationMeasures.remove(answer);
    }

    self.save = function () {
        var answers = [];
        self.questions.forEach(question => answers = answers.concat(question.AnswersAnticipationMeasures()))

        console.log(answers);

        $.ajax({
            url: '/Vault/Formulaires/AnticipationMeasures?handler=SaveInfos',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                folderAnticipationMeasuresInfo: self.folderAnticipationMeasuresInfo,
                answers,
                answersToDelete: self.answersToDelete
            },
            success: function () {
                //Here boucle for createEmptyDocument
                /*if (self.folderAnticipationMeasuresInfo.AlreadyDrawnUpFutureProtectionMandate()) {
                    createEmptyDoc(self.folderAnticipationMeasuresInfo.VaultId(), "Mandat de protection future", 24, "Mesures d'anticipations");
                    createEmptyContactPart(11, "Mandataire actuelle", self.folderAnticipationMeasuresInfo.VaultId());
                }

                if (self.folderAnticipationMeasuresInfo.DraftedBy() == "Par un notaire") {
                    createEmptyContactPro(18, "Notaire", self.folderAnticipationMeasuresInfo.VaultId());
                }

                answers.forEach(function (answer) {
                    console.log(answer);
                    if (answer.QuestionId == 21 && answer.Answer1 == 80) {
                        if (answer.Selected()) {
                            createEmptyDoc(self.folderAnticipationMeasuresInfo.VaultId(), "Procuration bancaire", 25, "Mesures d'anticipations");
                        }
                    }
                })*/
                //
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }
AnticipationMeasuresViewModel = self;
}
