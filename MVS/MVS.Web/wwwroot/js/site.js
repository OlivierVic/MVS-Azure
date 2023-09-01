// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function displayDate(date) {
    var dd = date.getDate();
    var mm = date.getMonth() + 1;
    var yyyy = date.getFullYear();

    dd = dd < 10 ? '0' + dd : dd;
    mm = mm < 10 ? '0' + mm : mm;

    return dd + '/' + mm + '/' + yyyy;
}

function displayTimeHM(time) {
    var hours = time.getHours();
    var minutes = time.getMinutes();

    hours = hours < 10 ? '0' + hours : hours;
    minutes = minutes < 10 ? '0' + minutes : minutes;

    return hours + ':' + minutes;
}

/*function createEmptyDocWithEvent(event, folderId, name, type, typeName, canBeMultiple) {
    $.ajax({
        url: '/Vault/Documents?handler=CreateEmptyDocument',
        type: 'POST',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            folderId,
            name,
            type,
            typeName,
            canBeMultiple,
        },
        success: function () {
            if (event) {
                $(event.target).text("Document créé");
                $(event.target).attr("disabled", true);
            }
        },
        error: function () {
            toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
        }
    });
}*/

/*function createEmptyDoc(folderId, name, type, typeName, canBeMultiple) {
    $.ajax({
        url: '/Vault/Documents?handler=CreateEmptyDocument',
        type: 'POST',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            folderId,
            name,
            type,
            typeName,
            canBeMultiple,
        },
        error: function () {
            toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
        }
    });
}*/

function createEmptyContactProWithEvent(event, jobId, otherJob, folderId, typeContact) {
    $.ajax({
        url: '/Vault/AddressBook?handler=CreateEmptyContactPro',
        type: 'POST',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            jobId,
            otherJob,
            folderId,
            typeContact,
        },
        success: function () {
            if (event) {
                $(event.target).text("VaultContact créé");
                $(event.target).attr("disabled", true);
            }
        },
        error: function () {
            toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
        }
    });
}

/*function createEmptyContactPro(jobId, otherJob, folderId) {
    $.ajax({
        url: '/Vault/AddressBook?handler=CreateEmptyContactProRequired',
        type: 'POST',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            jobId,
            otherJob,
            folderId,
        },
        error: function () {
            toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
        }
    });
}*/

/*function createEmptyContactPart(kinship, other, folderId) {
    $.ajax({
        url: '/Vault/AddressBook?handler=CreateEmptyContactParticularRequired',
        type: 'POST',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            kinship,
            other,
            folderId,
        },
        error: function () {
            toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
        }
    });
}*/
