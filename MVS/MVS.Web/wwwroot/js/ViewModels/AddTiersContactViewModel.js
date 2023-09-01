var AddTiersContactViewModel = null;

function addTiersContactViewModel(newContact)
{
    self = this;

    self.newContact = ko.mapping.fromJS(newContact);
    if (self.newContact.Id() == null) {
        self.newContact.Ispro(null);
        self.newContact.Country();
    }

    self.newContact.Ispro.subscribe(function (newValue) {
        setTimeout(function () {
            $("#countryselector").countrySelect({
                /*defaultCountry: "fr",
                preferredCountries: ['fr']*/
            });
        }, 10);

        setTimeout(function () {
            $("#nationalityselector").nationalitySelect({
                //defaultCountry: "fr",
                //preferredCountries: ['fr']
            });
        }, 10);
    });
    

    if (self.newContact.Ispro || !self.newContact.Ispro && self.newContact.Ispro != null) {
        setTimeout(function () {
            $("#countryselector").countrySelect({
                /*defaultCountry: "fr",
                preferredCountries: ['fr']*/
            });
        }, 10);

        setTimeout(function () {
            $("#nationalityselector").nationalitySelect({
                //defaultCountry: "fr",
                //preferredCountries: ['fr']
            });
        }, 10);
    }

    self.checkAdd = function () {
        var missingData = false;
        $("input.required, select.required").each(function () {
            if (!$(this).val() || $(this).val() == null) {
                $(this).parent().addClass("errorHere");
                missingData = true;
            }
            else {
                $(this).parent().removeClass("errorHere");
            }
        });
        if (missingData) {
            return;
        }

        $("#add-contact-form").submit();
    }

    AddTiersContactViewModel = self;
}
