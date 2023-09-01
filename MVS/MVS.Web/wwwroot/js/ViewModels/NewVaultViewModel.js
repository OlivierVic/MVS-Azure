var NewVaultViewModel = null;

const FormChoise = {
    Default: 0,
    Yes: 1,
    No: 2
};

function newVaultViewModel(newVault) {
    self = this;

    self.newVault = ko.observable(newVault);

    self.ImPersonToProtect = ko.observable(FormChoise.Default);
    self.PersonToProtectIsRequesting = ko.observable(FormChoise.Default);
    self.ImRequester = ko.observable(FormChoise.Default);

    self.newVault = ko.mapping.fromJS(newVault);
    if (self.newVault.Id() == null) {
        //self.newVault.Country('France');
        //self.newVault.Nationality('Française');
    }

    setTimeout(function () {
        $("#countryselector").countrySelect({
            //defaultCountry: "fr",
            //preferredCountries: ['fr']
        });
    }, 10);

    setTimeout(function () {
        $("#nationalityselector").nationalitySelect({
            //defaultCountry: "fr",
            //preferredCountries: ['fr']
        });
    }, 10);
    

    self.checkAdd = function () {
        var missingData = false;
        var caractereLastNameData = false;
        var caractereFirstNameData = false;
        var caractereBirthNameData = false;

        if (self.newVault.Sex == "null") {
            missingData = true;
        }

        if (self.newVault.Country == "null") {
            missingData = true;
        }

        if (self.newVault.HaveBirthName() == null) {
            missingData = true;
        }

        if (self.newVault.HaveBirthName() && self.newVault.BirthName() != "null" && containsSpecialChars(self.newVault.BirthName())) {
            caractereBirthNameData = true;
        }

        if (self.newVault.LastName() != "null" && containsSpecialChars(self.newVault.LastName())) {
            caractereLastNameData = true;
        }

        if (self.newVault.FirstName() != "null" && containsSpecialChars(self.newVault.FirstName())) {
            caractereFirstNameData = true;
        }

        if (self.newVault.AcceptedCondition() == null || self.newVault.AcceptedCondition() == false) {
            TrueLastLine = true;
        }

        $("input.required").each(function () {
            if (!$(this).val()) {
                missingData = true;
            }
        });

        if (missingData) {
            toastr.error("Vous devez compléter tous les champs");
            return;
        }

        if (caractereBirthNameData) {
            toastr.error("Vous ne devez pas mettre de caratère spéciaux dans le nom de naissance");
            return;
        }

        if (caractereLastNameData) {
            toastr.error("Vous ne devez pas mettre de caratère spéciaux dans le nom");
            return;
        }

        if (caractereFirstNameData) {
            toastr.error("Vous ne devez pas mettre de caratère spéciaux dans le prénom");
            return;
        }

        $("#add-folder-form").submit();
    }

    function containsSpecialChars(str) {
        const specialChars = /[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
        return specialChars.test(str);
    }

    NewVaultViewModel = self;
}
