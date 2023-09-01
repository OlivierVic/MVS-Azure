var DeleteArchiveFolderModalModel = null;

function deleteArchiveFolderModalModel(folder) {
    self = this;

    self.folder = ko.mapping.fromJS(folder);
    self.returnUrl = "/Home";

    self.delete = function () {
        $.ajax({
            url: '/Vault',
            type: 'POST',
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
            },
            success: function () {
                deleteFolderInside(self.folderHealth.VaultId());

                window.location.href = self.returnUrl;
            },
            error: function () {
                toastr.error("Une erreur est survenue, veuillez réessayer plus tard");
            }
        });
    }

    DeleteArchiveFolderModalModel = self;
}

/*@section Scripts
{
<script src="~/js/ViewModels/FolderDocumentViewModel.js" asp-append-version="true"></script>

<script type="text/javascript">
    $(document).ready(function () {
        ko.applyBindings(new folderDocumentViewModel(@JsonHelper.GetJson(Model.Vault), 5), document.getElementById('documents'));
        $("#tab1").addClass("selected");
    });
</script>
}*/
