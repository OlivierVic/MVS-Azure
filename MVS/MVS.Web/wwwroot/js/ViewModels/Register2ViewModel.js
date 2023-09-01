var Register2ViewModel = null;

const RegisterCase = {
    CaregiverPro: 0,
    caregiverParticular: 1,
    Beneficiary: 2,
};

const RegisterMutac = {
    Non: 0,
    Oui: 1,
};

function register2ViewModel() {
    self = this;

    self.selectedRegisterCase = ko.observable(null);
    self.adhemutac = ko.observable(null);

    Register2ViewModel = self;
}
