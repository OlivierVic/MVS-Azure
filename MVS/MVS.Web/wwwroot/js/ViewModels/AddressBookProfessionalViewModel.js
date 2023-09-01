var AddressBookProfessionalViewModel = null;

const RegisterCase = {
    CaregiverPro: 0,
    caregiverParticular: 1,
    Beneficiary: 2,
};

function addressBookProfessionalViewModel(vaultId, jobs) {
    self = this;

    self.vaultId = vaultId;
    self.jobs = jobs;

    self.contacts = ko.observable({});
    self.currentPage = ko.observable(1);
    self.nbPages = ko.observable(1);
    self.pages = ko.observable();

    self.currentPage.subscribe(function () {
        self.getContacts();
    });

    self.getPagesList = function () {
        const boundaryCount = 2;
        const siblingCount = 1;

        const range = (start, end) => {
            const length = end - start + 1;
            return Array.from({ length }, (_, i) => start + i);
        };
        const startPages = range(1, Math.min(boundaryCount, self.nbPages()));
        const endPages = range(Math.max(self.nbPages() - boundaryCount + 1, boundaryCount + 1), self.nbPages());
        const siblingsStart = Math.max(
            Math.min(
                self.currentPage() - siblingCount,
                self.nbPages() - boundaryCount - siblingCount,
            ),
            boundaryCount + 2,
        );
        const siblingsEnd = Math.min(
            Math.max(
                self.currentPage() + siblingCount,
                boundaryCount + siblingCount,
            ),
            endPages[0] - 2,
        );
        const itemList = [
            ...startPages,
            ...(siblingsStart > boundaryCount + 2
                ? ['...']
                : boundaryCount + 1 < self.nbPages() - boundaryCount
                    ? [boundaryCount + 1]
                    : []),
            ...range(siblingsStart, siblingsEnd),
            ...(siblingsEnd < self.nbPages() - boundaryCount - 1
                ? ['...']
                : self.nbPages() - boundaryCount > boundaryCount
                    ? [self.nbPages() - boundaryCount]
                    : []),
            ...endPages,
        ];

        self.pages(itemList);
    }

    self.contactInfo = function (id) {
        if (id != null) {
            window.location.href = '/Vault/AddressBookContactInfos/Index?contactId=' + id;
        }
        else {
            window.location.reload();
        }
    }

    self.getContacts = function () {
        $.ajax({
            url: '/Vault/AddressBookProfessional?handler=ContactPro',
            type: 'GET',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                vaultId: self.vaultId,
                currentPage: self.currentPage()
            },
            success: function (data) {
                self.contacts(data.contacts);
                self.nbPages(data.nbPages);
                self.getPagesList();
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }
    self.getContacts();

    AddressBookProfessionalViewModel = self;
}
