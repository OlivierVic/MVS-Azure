var PutVaultViewModel = null;

const FormChoise = {
    Default: 0,
    Yes: 1,
    No: 2
};

function putVaultViewModel(putVault, vaultId) {
    self = this;

    self.putVault = ko.mapping.fromJS(putVault);
    self.returnUrl = "/Vault";
    self.vaultId = vaultId;

    /*self.newVault = ko.observable(newVault);*/

    self.ImPersonToProtect = ko.observable(FormChoise.Default);
    self.PersonToProtectIsRequesting = ko.observable(FormChoise.Default);
    self.ImRequester = ko.observable(FormChoise.Default);

    self.putVault = ko.mapping.fromJS(putVault);
    if (self.putVault.Id() == null) {
        //self.putVault.Country('France');
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

    self.save = function () {
        $.ajax({
            url: '/Vault/PutVault?handler=SaveInfos',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vault: self.putVault
            },
            success: function () {
                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    function containsSpecialChars(str) {
        const specialChars = /[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
        return specialChars.test(str);
    }

    PutVaultViewModel = self;
}
