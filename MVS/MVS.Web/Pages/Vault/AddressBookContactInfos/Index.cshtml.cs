using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Aspose.Words;
using Aspose.Words.Fields;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault.AddressBookContactInfos;

public class IndexModel : PageModel
{
    private readonly IVaultContactService _contactService;
    private readonly IJobProfessionelService _jobProfessionelService;
    private readonly IJobParticularService _jobParticularService;
    private readonly IVaultService _folderService;
    private readonly IVaultCategoryService _categoryService;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public Common.Models.VaultContact _contact { get; set; }
    public Common.Models.Vault _folder { get; set; }
    public string _folderId { get; set; }
    public string _contactId { get; set; }
    public string _contactName { get; set; }
    public bool _isImmediateProtection { get; set; }
    public string _confidence { get; set; }
    public List<JobProfessionel> _job { get; set; }
    public List<JobParticular> _jobParticular { get; set; }
    public List<VaultCategory> _categories { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public Dictionary<string, string> _folderInfoHeader { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public IndexModel(IVaultContactService contactService, IConfiguration configuration, IVaultService folderService, IVaultCategoryService categoryService,
                                            IJobProfessionelService jobService, IJobParticularService jobParticularService, UserManager<ApplicationUser> userManager)
    {
        this._contactService = contactService;
        this._configuration = configuration;
        this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._jobProfessionelService = jobService;
        this._jobParticularService = jobParticularService;
        this._folderService = folderService;
        this._userManager = userManager;
        this._categoryService = categoryService;
    }

    public async Task OnGet(string contactId)
    {
        this._contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
        if (this._contact == null)
        {
            throw new ArgumentException("La donnée que vous voulez récupérer n'existe pas");
        }

        this._folderId = this._contact.VaultId;
        this._contactId = this._contact.Id;
        Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == this._folderId));

        await this._accessService.CheckAccess(folder.Id, this._userId, this.User.IsInRole("SuperAdmin"));


        List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._contactName = this._contact.FirstName + ' ' + this._contact.LastName;

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Vault", "Mes dossiers");
        this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { folderId = folder.Id }), $"Dossier N°{folder.Title}");
        this._breadcrumb.Add(this.Url.Page("/Vault/AddressBook", new { folderId = folder.Id }), "Carnet d'adresses");
        this._breadcrumb.Add("/Vault/AddressBookContactInfos", $"{this._contactName}");

        this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(folder);

        

        this._job = await this._jobProfessionelService.Search(new Specification<JobProfessionel>(j => true));
        this._jobParticular = await this._jobParticularService.Search(new Specification<JobParticular>(jp => true));
        this._categories = await this._categoryService.Search(new Specification<VaultCategory>(c => true));
    }

    public async Task<IActionResult> OnPostUpdateProperty(string contactId, int sex, string firstName, string lastName)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.Sex = sex;
        contact.FirstName = firstName;
        contact.LastName = lastName;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateIdentity(string contactId, int kinship, DateTime dateOfBirth, string placeOfBirth, string nationality,
                                        bool unknownMother, string firstLastNameMother, bool unknownFather, string firstLastNameFather, bool adoptedPerson)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.Kinship = kinship;
        contact.DateOfBirth = dateOfBirth;
        contact.PlaceOfBirth = placeOfBirth;
        contact.Nationality = nationality;
        contact.UnknownMother = unknownMother;
        contact.FirstLastNameMother = firstLastNameMother;
        contact.UnknownFather = unknownFather;
        contact.FirstLastNameFather = firstLastNameFather;
        contact.AdoptedPerson = adoptedPerson;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateContact(string contactId, string addres, string zipCodeAndCity, string country, string phoneNumber, string email)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.Addres = addres;
        contact.ZipCodeAndCity = zipCodeAndCity;
        contact.Country = country;
        contact.PhoneNumber = phoneNumber;
        contact.Email = email;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateActivity(string contactId, int jobId, string otherJob, string company, int accompaniment)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.Job = jobId;
        contact.OtherJob = otherJob;
        contact.Company = company;
        contact.Accompaniment = accompaniment;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateRelationship(string contactId, int relationshipQuality, int relationshipFrequencies)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.RelationshipQuality = relationshipQuality;
        contact.RelationshipFrequencies = relationshipFrequencies;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateRoleImmediateProtection(string contactId, bool isFolderAdmin, bool isSetAskProtection, bool isSetJudge, int adviceStatus, bool Confidence, int TypeMission)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.IsFolderAdmin = isFolderAdmin;
        contact.IsSetAskProtection = isSetAskProtection;
        contact.IsSetJudge = isSetJudge;
        contact.AdviceStatus = adviceStatus;
        contact.Confidence = Confidence;
        contact.TypeMission = TypeMission;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateRoleFutureProtection(string contactId, bool isFolderAdmin, bool isFutuAgent, bool protectPeople, bool protectAllProperty, bool protectOfCertainGoods, string propertyDetails, bool Confidence, bool infoPro, int typeMission)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.IsFolderAdmin = isFolderAdmin;
        contact.IsFutuAgent = isFutuAgent;
        /*contact.AgentMission = agentMission;*/
        contact.ProtectPeople = protectPeople;
        contact.ProtectAllProperty = protectAllProperty;
        contact.ProtectOfCertainGoods = protectOfCertainGoods;
        contact.PropertyDetails = propertyDetails;
        contact.Confidence = Confidence;
        contact.InfoPro = infoPro;
        contact.TypeMission = typeMission;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateDetails(string contactId, string details)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));

        contact.ContactDetails = details;

        await this._contactService.Update(contact);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostDeleteContact(string contactId, string password)
    {
        ApplicationUser user = this._userManager.Users.FirstOrDefault(u => u.Id == this._userId);
        if (!await this._userManager.CheckPasswordAsync(user, password))
        {
            return this.StatusCode((int)HttpStatusCode.BadRequest, null);
        }

        Specification<Common.Models.VaultContact> spec = new(c => c.Id == contactId);

        Common.Models.VaultContact contact = await this._contactService.Get(spec);
        await this._contactService.Delete(contact);

        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<FileContentResult> OnGetDownloadContactPdf(string folderId, string contactId)
    {
        Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == folderId));
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
        this._job = await this._jobProfessionelService.Search(new Specification<JobProfessionel>(j => true));
        this._jobParticular = await this._jobParticularService.Search(new Specification<JobParticular>(jp => true));
        this._categories = await this._categoryService.Search(new Specification<VaultCategory>(c => true));
        string tmpAccompaniment = "";
        string tmpSex = null;
        string DateOfBirth = contact.DateOfBirth == null ? "Non renseigné" : contact.DateOfBirth?.ToString("dd/MM/yyyy");
        string tmpMother = contact.UnknownMother ?? false ? "Inconnu" : contact.FirstLastNameMother;
        string tmpFather = contact.UnknownFather ?? false ? "Inconnu" : contact.FirstLastNameFather;
        string tmpAdopted = contact.AdoptedPerson ?? false ? "Oui" : "Non";
        string tmpProfession = null;
        string tmpProfessionProfesionnel = null;
        if (contact.Accompaniment != null)
        {
            tmpAccompaniment = contact.Accompaniment == 0 ? "Null" : this._categories.FirstOrDefault(c => c.Id == contact.Accompaniment).CategoryName;
        }
        string tmpQualiteRelation = contact.RelationshipQuality == null ? string.Empty : EnumHelper.GetDescription((RelationshipQuality)contact.RelationshipQuality);
        string tmpFrequenceRelations = contact.RelationshipFrequencies == null ? string.Empty : EnumHelper.GetDescription((RelationshipFrequencies)contact.RelationshipFrequencies);
        string tmpAdmin = contact.IsFolderAdmin ?? false ? "Oui" : "Non";
        string tmpAskProtect = contact.IsSetAskProtection ?? false ? "Oui" : "Non";
        string tmpSetJudge = contact.IsSetJudge ?? false ? "Oui" : "Non";
        string tmpAdviceStatus = contact.AdviceStatus == (int)ContactAdviceStatus.NotRequested ? "Non" : "Oui";
        string tmpOpinionStatus = contact.OpinionPro ?? false ? "Oui" : "Non";
        string tmpRequestCopy = contact.RequestCopy ?? false ? "Oui" : "Non";
        string tmpConfidence = contact.Confidence ?? false ? "Oui" : "Non";
        string tmpTrustedPerson = contact.Confidence ?? false ? "Oui" : "Non";
        string tmpInfoPro = contact.InfoPro ?? false ? "Oui" : "Non";
        string tmpIsFutuAgent = contact.IsFutuAgent ?? false ? "Oui" : "Non";
        string tmpContactDetails = contact.ContactDetails == "" ? "Non renseigné" : contact.ContactDetails;
        string tmpTypeMission;

        switch (contact.TypeMission)
        {
            case 2:
                tmpTypeMission = "Contrôleur";
                break;

            case 4:
                tmpTypeMission = "Observateur";
                break;

            case 5:
                tmpTypeMission = "Remplaçant Contrôleur";
                break;

            case 6:
                tmpTypeMission = "Remplaçant Mandataire";
                break;

            default:
                tmpTypeMission = "Aucune";
                break;
        }

        if (contact.Ispro)
        {
            if (this._job.FirstOrDefault(jp => jp.Id == contact.Job).Job == "Autre")
            {
                tmpProfessionProfesionnel = contact.OtherJob;
            }
            else
            {
                tmpProfessionProfesionnel = this._job.FirstOrDefault(jp => jp.Id == contact.Job).Job;
            }
        }
        else
        {

            if (this._jobParticular.FirstOrDefault(jp => jp.Id == contact.Job).Job == "Autre")
            {
                if (contact.OtherJob != null)
                {
                    tmpProfession = contact.OtherJob;
                }
                else
                {
                    tmpProfession = "Non renseigné";
                }
            }
            else
            {
                tmpProfession = this._jobParticular.FirstOrDefault(jp => jp.Id == contact.Job).Job;
            }
        }

        if (contact.Sex == (int)Gender.Man)
        {
            tmpSex = "M";
        }
        else if (contact.Sex == (int)Gender.Woman)
        {
            tmpSex = "Mme";
        }

        Aspose.Words.Document doc = new();
        DocumentBuilder builder = new(doc);

        //Create Title
        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        builder.Font.Size = 20;
        builder.Writeln($"Dossier n°{folder.Title} - {folder.FirstName} {folder.LastName}");

        //Create Name
        builder.Font.Size = 16;
        if (contact.Sex == (int)Gender.NonBinary)
        {
            builder.Writeln($"VaultContact {contact.FirstName} {contact.LastName}");
        }
        else
        {
            builder.Writeln($"VaultContact {tmpSex} {contact.FirstName} {contact.LastName}");
        }

        builder.Write("\n\n");

        //WriteListcontact
        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        builder.Font.Size = 12;
        /*    if (contact.Ispro)
            {
                builder.Writeln("Type de contact : Professionel");
                if (contact.AdviceStatus == (int)ContactAdviceStatus.NotRequested)
                {
                    builder.Writeln("Avis du proche : Avis non demandé");
                }
                else if (contact.AdviceStatus == (int)ContactAdviceStatus.Given)
                {
                    builder.Writeln("Avis du proche : Avis transmis");
                }
                else
                {
                    builder.Writeln("Avis du proche : Avis en attente de transmission");
                }

                builder.Write("\n");
                builder.Writeln($"À propos de ce profesionnel :");
                builder.Write("\n");
                builder.Writeln($"      Activité :");
                builder.Writeln($"          Profession : {tmpProfessionProfesionnel}");
                builder.Writeln($"          Organisation / Entreprise : {contact.Company}");
                builder.Writeln($"          Accompagnement : {tmpAccompaniment}");
                builder.Write("\n");
                builder.Writeln($"      VaultContact :");
                builder.Writeln($"          Adresse : {contact.Addres}");
                builder.Writeln($"          Téléphone : {contact.PhoneNumber}");
                builder.Writeln($"          Email : {contact.Email}");
                builder.Write("\n");

                builder.Write("\n");
                builder.Writeln($"Rôle au sein du dossier :");
                builder.Write("\n");
                builder.Writeln($"      Administrateur : {tmpAdmin}");
                //builder.Writeln($"      Demandeur à la protection : {tmpAskProtect} A SUPPRIMER");
                builder.Writeln($"      Désigné par le juge : {tmpSetJudge}");
                if (tmpSetJudge == "Oui")
                {
                    if (contact.TypeMission == 1)
                    {
                        builder.Writeln($"      Type de mission : Protecteur");
                    }
                    else if (contact.TypeMission == 2)
                    {
                        builder.Writeln($"      Type de mission : Contrôleur");
                    }
                    else
                    {
                        builder.Writeln($"      Type de mission : Non renseigné");
                    }
                }
                builder.Writeln($"      Avis demandé : {tmpAdviceStatus}");
                builder.Writeln($"      Personne de confiance : {tmpConfidence}");
                builder.Write("\n");

                builder.Write("\n");
                builder.Writeln($"Précisions sur le contact :");
                builder.Write("\n");
                builder.Writeln($"{tmpContactDetails}");
            }
            else
            {
                builder.Writeln("Type de contact : Particulier");
                if (contact.AdviceStatus == (int)ContactAdviceStatus.NotRequested)
                {
                    builder.Writeln("Avis du proche : Avis non demandé");
                }
                else if (contact.AdviceStatus == (int)ContactAdviceStatus.Given)
                {
                    builder.Writeln("Avis du proche : Avis transmis");
                }
                else
                {
                    builder.Writeln("Avis du proche : Avis en attente de transmission");
                }

                builder.Write("\n");
                builder.Writeln($"À propos de ce proche :");
                builder.Write("\n");
                builder.Writeln($"      Identité :");
                builder.Writeln($"          Lien de parenté : {EnumHelper.GetDescription((Kinship)contact.Kinship)}");
                builder.Writeln($"          Date de naissance : {DateOfBirth}");
                builder.Writeln($"          Lieu de naissance : {contact.PlaceOfBirth}");
                builder.Writeln($"          Nationalité : {contact.Nationality}");
                builder.Write("\n");
                builder.Writeln($"      VaultContact :");
                builder.Writeln($"          Adresse : {contact.Addres}");
                builder.Writeln($"          Téléphone : {contact.PhoneNumber}");
                builder.Writeln($"          Email : {contact.Email}");
                builder.Write("\n");
                builder.Writeln($"      Activité :");
                builder.Writeln($"          Profession : {tmpProfession}");
                builder.Write("\n");
                builder.Writeln($"      Relations :");
                builder.Writeln($"          Qualité des relations : {tmpQualiteRelation}");
                builder.Writeln($"          Fréquence des relations : {tmpFrequenceRelations}");
                builder.Write("\n");

                builder.Write("\n");
                builder.Writeln($"Rôle du contact :");
                builder.Write("\n");
                builder.Writeln($"      Administrateur : {tmpAdmin}");
                builder.Writeln($"      Demandeur à la protection : {tmpAskProtect}");
                builder.Writeln($"      Envoyer la copie de la demande : {tmpRequestCopy}");
                builder.Writeln($"      Désigné par le juge : {tmpSetJudge}");
                if (tmpSetJudge == "Oui")
                {
                    if (contact.TypeMission == 1)
                    {
                        builder.Writeln($"      Type de mission : Protecteur");
                    }
                    else if (contact.TypeMission == 2)
                    {
                        builder.Writeln($"      Type de mission : Contrôleur");
                    }
                    else
                    {
                        builder.Writeln($"      Type de mission : Non renseigné");
                    }
                    builder.Writeln($"          Nom et prénom de la mère : {tmpMother}");
                    builder.Writeln($"          Nom et prénom du père : {tmpFather}");
                    builder.Writeln($"          Personne adoptée : {tmpAdopted}");
                }
                builder.Writeln($"      Avis demandé : {tmpOpinionStatus}");
                builder.Writeln($"      Personne de confiance : {tmpConfidence}");
                builder.Write("\n");

                builder.Write("\n");
                builder.Writeln($"Précisions sur le contact :");
                builder.Write("\n");
                builder.Writeln($"{tmpContactDetails}");
            }
        }*/

        if (contact.Ispro)
        {
            builder.Writeln("Type de contact : Professionel");
            builder.Write("\n");
            builder.Writeln($"À propos de ce profesionnel :");
            builder.Write("\n");
            builder.Writeln($"      Activité :");
            builder.Writeln($"          Profession : {tmpProfessionProfesionnel}");
            builder.Writeln($"          Organisation / Entreprise : {contact.Company}");
            builder.Writeln($"          Accompagnement : {tmpAccompaniment}");
            builder.Write("\n");
            builder.Writeln($"      VaultContact :");
            builder.Writeln($"          Adresse : {contact.Addres}");
            builder.Writeln($"          Téléphone : {contact.PhoneNumber}");
            builder.Writeln($"          Email : {contact.Email}");
            builder.Write("\n");

            builder.Write("\n");
            builder.Writeln($"Rôle au sein du dossier :");
            builder.Write("\n");
            builder.Writeln($"      Administrateur : {tmpAdmin}");
            builder.Writeln($"      Mandataire futur(e) : {tmpIsFutuAgent}");
            if (tmpIsFutuAgent == "Oui")
            {
                builder.Writeln($"      Mission mandataire :");
                if (contact.ProtectPeople == true)
                {
                    builder.Writeln($"          Protection de la personne : Oui");
                }
                else
                {
                    builder.Writeln($"          Protection de la personne : Non");
                }
                if (contact.ProtectAllProperty == true)
                {
                    builder.Writeln($"          Protection de tous les biens : Oui");
                }
                else
                {
                    builder.Writeln($"          Protection de tous les biens : Non");
                }
                if (contact.ProtectOfCertainGoods == true)
                {
                    builder.Writeln($"          Protection de certains biens : Oui");
                }
                else
                {
                    builder.Writeln($"          Protection de certains biens : Non");
                }
                if (contact.ProtectOfCertainGoods == true)
                {
                    builder.Writeln($"          Type de biens : {contact.PropertyDetails}");
                }

                if (contact.TypeMission == 2)
                {
                    builder.Writeln($"          Autre mission : Contrôleur");
                }
                else if (contact.TypeMission == 4)
                {
                    builder.Writeln($"          Autre mission : Observateur");
                }
                else if (contact.TypeMission == 5)
                {
                    builder.Writeln($"          Autre mission : Remplaçant Contrôleur");
                }
                else if (contact.TypeMission == 6)
                {
                    builder.Writeln($"          Autre mission : Remplaçant Mandataire");
                }
                else if (contact.TypeMission == 3)
                {
                    builder.Writeln($"          Autre mission : Aucune");
                }

            }
            else if (tmpIsFutuAgent == "Non")
            {
                if (contact.TypeMission == 2)
                {
                    builder.Writeln($"      Type de mission : Contrôleur");
                }
                else if (contact.TypeMission == 4)
                {
                    builder.Writeln($"      Type de mission : Observateur");
                }
                else if (contact.TypeMission == 5)
                {
                    builder.Writeln($"      Type de mission : Remplaçant Contrôleur");
                }
                else if (contact.TypeMission == 6)
                {
                    builder.Writeln($"      Type de mission : Remplaçant Mandataire");
                }
                else if (contact.TypeMission == 3)
                {
                    builder.Writeln($"      Type de mission : Aucune");
                }
            }

            //builder.Writeln($"      Type Mission : {tmpTypeMission}");
            builder.Writeln($"      Personne de confiance : {tmpTrustedPerson}");
            builder.Writeln($"      Informer le professionnel de la rédaction du mandat : {tmpInfoPro}");
            builder.Write("\n");

            builder.Write("\n");
            builder.Writeln($"Précisions sur le contact :");
            builder.Write("\n");
            builder.Writeln($"{tmpContactDetails}");
        }
        else
        {
            builder.Writeln("Type de contact : Particulier");
            builder.Write("\n");
            builder.Writeln($"À propos de ce proche :");
            builder.Write("\n");
            builder.Writeln($"      Identité :");
            builder.Writeln($"          Lien de parenté : {EnumHelper.GetDescription((Kinship)contact.Kinship)}");
            builder.Writeln($"          Date de naissance : {DateOfBirth}");
            builder.Writeln($"          Lieu de naissance : {contact.PlaceOfBirth}");
            builder.Writeln($"          Nationalité : {contact.Nationality}");
            builder.Writeln($"          Nom et prénom de la mère : {tmpMother}");
            builder.Writeln($"          Nom et prénom du père : {tmpFather}");
            builder.Writeln($"          Personne adoptée : {tmpAdopted}");
            builder.Write("\n");
            builder.Writeln($"      VaultContact :");
            builder.Writeln($"          Adresse : {contact.Addres}");
            builder.Writeln($"          Téléphone : {contact.PhoneNumber}");
            builder.Writeln($"          Email : {contact.Email}");
            builder.Write("\n");
            builder.Writeln($"      Activité :");
            builder.Writeln($"          Profession : {tmpProfession}");
            builder.Write("\n");
            builder.Writeln($"      Relations :");
            builder.Writeln($"          Qualité des relations : {tmpQualiteRelation}");
            builder.Writeln($"          Fréquence des relations : {tmpFrequenceRelations}");
            builder.Write("\n");

            builder.Write("\n");
            builder.Writeln($"Rôle au sein du dossier :");
            builder.Write("\n");
            builder.Writeln($"      Administrateur : {tmpAdmin}");
            builder.Writeln($"      Mandataire futur(e) : {tmpIsFutuAgent}");
            if (tmpIsFutuAgent == "Oui")
            {
                builder.Writeln($"      Mission mandataire :");
                if (contact.ProtectPeople == true)
                {
                    builder.Writeln($"          Protection de la personne : Oui");
                }
                else
                {
                    builder.Writeln($"          Protection de la personne : Non");
                }
                if (contact.ProtectAllProperty == true)
                {
                    builder.Writeln($"          Protection de tous les biens : Oui");
                }
                else
                {
                    builder.Writeln($"          Protection de tous les biens : Non");
                }
                if (contact.ProtectOfCertainGoods == true)
                {
                    builder.Writeln($"          Protection de certains biens : Oui");
                }
                else
                {
                    builder.Writeln($"          Protection de certains biens : Non");
                }
                if (contact.ProtectOfCertainGoods == true)
                {
                    builder.Writeln($"          Type de biens : {contact.PropertyDetails}");
                }

                if (contact.TypeMission == 2)
                {
                    builder.Writeln($"          Autre mission : Contrôleur");
                }
                else if (contact.TypeMission == 4)
                {
                    builder.Writeln($"          Autre mission : Observateur");
                }
                else if (contact.TypeMission == 5)
                {
                    builder.Writeln($"          Autre mission : Remplaçant Contrôleur");
                }
                else if (contact.TypeMission == 6)
                {
                    builder.Writeln($"          Autre mission : Remplaçant Mandataire");
                }
                else if (contact.TypeMission == 3)
                {
                    builder.Writeln($"          Autre mission : Aucune");
                }

                builder.Writeln($"          Nom et prénom de la mère : {tmpMother}");
                builder.Writeln($"          Nom et prénom du père : {tmpFather}");
                builder.Writeln($"          Personne adoptée : {tmpAdopted}");
            }
            else if (tmpIsFutuAgent == "Non")
            {
                if (contact.TypeMission == 2)
                {
                    builder.Writeln($"      Type de mission : Contrôleur");
                }
                else if (contact.TypeMission == 4)
                {
                    builder.Writeln($"      Type de mission : Observateur");
                }
                else if (contact.TypeMission == 5)
                {
                    builder.Writeln($"      Type de mission : Remplaçant Contrôleur");
                }
                else if (contact.TypeMission == 6)
                {
                    builder.Writeln($"      Type de mission : Remplaçant Mandataire");
                }
                else if (contact.TypeMission == 3)
                {
                    builder.Writeln($"      Type de mission : Aucune");
                }
            }

            builder.Writeln($"      Personne de confiance : {tmpTrustedPerson}");
            builder.Writeln($"      Informer le professionnel de la rédaction du mandat : {tmpInfoPro}");
            builder.Write("\n");

            builder.Write("\n");
            builder.Writeln($"Précisions sur le contact :");
            builder.Write("\n");
            builder.Writeln($"{tmpContactDetails}");
        }


        MemoryStream stream = new();
        doc.Save(stream, SaveFormat.Pdf);

        return this.File(stream.ToArray(), "application/pdf", $"Information VaultContact du dossier n°{folder.Title} {folder.FirstName} {folder.LastName} - {contact.FirstName} {contact.LastName}.pdf");
    }
}
