using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<PrepareContractResult> PrepareContract(string templateId, string email, string data,
            string title,
            string folderId, string actions, string userId = null, List<Conversation> conversations = null,
            List<string> refElems = null, string endUserId = null, string parentContractId = null, string endUserEmail = null, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/PrepareContract", "POST");

            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            var ms = request.GetRequestStream();

            dynamic dt = new ExpandoObject();

            dt.TemplateId = templateId;
            dt.Email = email;
            dt.Data = data;
            dt.UserId = userId;
            dt.Title = title;
            dt.Actions = actions;
            dt.VaultId = folderId;
            dt.RefElems = refElems;
            dt.CreatorEndUserId = endUserId;
            dt.CreatorEndUserEmail = endUserEmail;
            dt.ParentContractId = parentContractId;
            dt.TenantId = tenantId;

            if (conversations != null)
            {
                dt.Conversations = conversations;
            }

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();
            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<PrepareContractResult>(sr.ReadToEnd());
        }

        public async Task<List<Template>> GetTemplateList(string dossierId, string tenantId = null, bool getDrafts = false,
            bool listAll = false)
        {
            var access_token = await Logon();

            var endpoint = listAll ? "/api/Template/ListAll" : $"/api/Template/List?dossierId={dossierId}";

            HttpWebRequest request = HttpWebRequest.CreateHttp(_baseUrl + endpoint);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            if (!String.IsNullOrWhiteSpace(tenantId))
                request.Headers.Add("TenantId", tenantId);

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            var templates = JsonConvert.DeserializeObject<List<Template>>(sr.ReadToEnd());
            return !getDrafts ? templates.Where(t => !t.IsDraft).ToList() : templates;
        }

        public async Task<List<UserStats>> GetUserListGlobalStatsPeriod(string beginDate, string endDate)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest("/api/Template/UsersGlobalStats/" + beginDate + "/" + endDate, "GET");

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<UserStats>>(sr.ReadToEnd());
        }

        public async Task<List<TemplateStats>> GetTemplateListGlobalStatsPeriod(string beginDate, string endDate, bool byTemplateOwner = false)
        {
            var endpoint = "/api/Template/ListGlobalStats/" + beginDate +
                           "/" + endDate +
                           (byTemplateOwner ? "?byTemplateOwner=true" : "");
            HttpWebRequest request = await CreateHttpWebRequest(endpoint, "GET");

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<TemplateStats>>(sr.ReadToEnd());
        }

        public async Task<List<TemplateStats>> GetUserTemplateListStatsPeriod(string userId, string beginDate,
            string endDate)
        {
            HttpWebRequest request = await CreateHttpWebRequest(
                "/api/Template/ListStats/" + userId + "/" + beginDate + "/" +
                endDate, "GET");

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<TemplateStats>>(sr.ReadToEnd());
        }

        public async Task<List<TemplateStats>> GetTemplateListGlobalStats(bool byTemplateOwner = false)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/ListGlobalStats" +
                                                                (byTemplateOwner ? "?byTemplateOwner=true" : ""),
                "GET");

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<TemplateStats>>(sr.ReadToEnd());
        }

        public async Task<List<TemplateStats>> GetTemplateListStats(string userId, string period)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest("/api/Template/ListStats/" + userId + "/" + period, "GET");

            try
            {
                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);

                return JsonConvert.DeserializeObject<List<TemplateStats>>(sr.ReadToEnd());
            }
            catch (Exception e)
            {
            }

            return null;
        }


        public async Task<Template> GetTemplate(string templateId)
        {
            try
            {
                HttpWebRequest request = await CreateHttpWebRequest("/api/Template/" + templateId, "GET");

                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);

                return JsonConvert.DeserializeObject<Template>(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Dossier>> GetTemplateFolders(string folderId, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Folders?folderId=" + folderId, "GET");
            if (!String.IsNullOrWhiteSpace(tenantId))
                request.Headers.Add("TenantId", tenantId);
            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<Dossier>>(sr.ReadToEnd());
        }

        public async Task<List<Dossier>> GetTemplateFolderCrumbs(string folderId)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Crumbs/" + folderId, "GET");
            return await this.GetResponseAsObject<List<Dossier>>(request);
        }

        public async Task<SearchTemplateResult> SearchTemplatesAndTemplateFolders(string templateName, string tenantId = null)
        {
            string urlTemplateName = Uri.EscapeDataString(templateName);
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Template/Search?str=" + templateName, "GET");
            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<SearchTemplateResult>(sr.ReadToEnd());
        }

        /// <summary>
        /// Create a new template
        /// </summary>
        /// <returns>the guid of the created template, null if an error occured</returns>
        public async Task<string> CreateTemplate(string templateName, string language, string applicableRight,
            string domain, string description, string authors, string folderId, bool isDraft, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/CreateTemplate", "POST");

            if (!String.IsNullOrWhiteSpace(tenantId))
                request.Headers.Add("TenantId", tenantId);

            Stream requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);

            dynamic emptyTemplate = new ExpandoObject();
            emptyTemplate.Title = templateName;
            emptyTemplate.Description = description;
            emptyTemplate.Field = domain;
            emptyTemplate.Language = language;
            emptyTemplate.Nation = applicableRight;
            emptyTemplate.Authors = authors;
            emptyTemplate.VaultId = folderId;
            emptyTemplate.IsDraft = isDraft;

            await sw.WriteAsync(JsonConvert.SerializeObject(emptyTemplate));
            await sw.FlushAsync();

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<string>(streamReader.ReadToEnd());
        }

        /// <summary>
        /// Create a new template
        /// </summary>
        /// <returns>the guid of the created template, null if an error occured</returns>
        public async Task<CreateTemplateResult> CreateTemplateFromFile(string templateName, string language, string applicableRight,
            string domain, string description, string author, IFormFile file, string folderId, bool isDraft, string tenantId = null)
        {

            var token = await Logon();
            FromFileTemplate fromFileTemplate = new()
            {
                Title = templateName,
                Description = description,
                Field = domain,
                Language = language,
                Nation = applicableRight,
                Authors = author,
                VaultId = folderId,
                IsDraft = isDraft,
            };

            HttpContent templateInformations = new StringContent(JsonConvert.SerializeObject(fromFileTemplate));
            HttpContent fileStreamContent = new StreamContent(file.OpenReadStream());

            using var client = new HttpClient();
            using var formData = new MultipartFormDataContent();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            formData.Add(templateInformations, "templateInformations");
            formData.Add(fileStreamContent, file.FileName, file.FileName);

            if (!String.IsNullOrWhiteSpace(tenantId))
                client.DefaultRequestHeaders.Add("TenantId", tenantId);

            var response = await client.PostAsync($"{_baseUrl}/api/Template/CreateTemplateFromFile", formData);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<CreateTemplateResult>(await response.Content.ReadAsStringAsync());
        }

        public async Task<CreateTemplateResult> CreateTemplateByZip(string templateName, string language, string applicableRight,
            string domain, string description, string author, IFormFile file, string folderId, bool isDraft, string tenantId = null)
        {
            var token = await Logon();
            FromFileTemplate fromFileTemplate = new()
            {
                Title = templateName,
                Description = description,
                Field = domain,
                Language = language,
                Nation = applicableRight,
                Authors = author,
                VaultId = folderId,
                IsDraft = isDraft,
            };

            HttpContent templateInformations = new StringContent(JsonConvert.SerializeObject(fromFileTemplate));
            HttpContent fileStreamContent = new StreamContent(file.OpenReadStream());

            using var client = new HttpClient();
            using var formData = new MultipartFormDataContent();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            formData.Add(templateInformations, "templateInformations");
            formData.Add(fileStreamContent, file.FileName, file.FileName);

            if (!String.IsNullOrWhiteSpace(tenantId))
                client.DefaultRequestHeaders.Add("TenantId", tenantId);

            var response = await client.PostAsync($"{_baseUrl}/api/Template/CreateTemplateByZip", formData);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<CreateTemplateResult>(await response.Content.ReadAsStringAsync());
        }


        public async Task<HttpWebResponse> RenameTemplate(string templateId, string newTemplateName, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Rename", "PUT");
            if (!String.IsNullOrWhiteSpace(tenantId))
                request.Headers.Add("TenantId", tenantId);
            // Creating request object
            dynamic requestObject = new ExpandoObject();
            requestObject.TemplateId = templateId;
            requestObject.TemplateNewName = newTemplateName;

            // Add request object to request body
            var requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);
            sw.Write(JsonConvert.SerializeObject(requestObject));
            sw.Flush();

            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<HttpWebResponse> RenameDossier(string dossierId, string newFolderName, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Dossier/Rename", "PUT");
            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            // Creating request object
            dynamic requestObject = new ExpandoObject();
            requestObject.DossierId = dossierId;
            requestObject.DossierNewName = newFolderName;

            // Add request object to request body
            var requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);
            sw.Write(JsonConvert.SerializeObject(requestObject));
            sw.Flush();

            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<HttpWebResponse> DeleteTemplate(string templateId)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/Template/Delete/{templateId}", "DELETE");

            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<HttpWebResponse> DeleteTemplateFolder(string folderId, string tenantId)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/Template/Dossier/{folderId}/Delete", "DELETE");
            request.Headers.Add("TenantId", tenantId);

            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<HttpWebResponse> CreateTemplateFolder(string currentFolderId, string folderName, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Dossier/Create", "POST");
            if (!String.IsNullOrWhiteSpace(tenantId))
                request.Headers.Add("TenantId", tenantId);


            dynamic requestObject = new ExpandoObject();
            requestObject.DossierName = folderName;
            requestObject.ParentId = currentFolderId;

            // Add request object to request body
            var requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);
            sw.Write(JsonConvert.SerializeObject(requestObject));
            sw.Flush();

            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<DossierTemplate> MoveTemplate(string templateId, string folderId, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Move", "PUT");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            dynamic requestObject = new ExpandoObject();
            requestObject.TemplateId = templateId;
            requestObject.NewDossierId = folderId;

            // Add request object to request body
            var requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);
            sw.Write(JsonConvert.SerializeObject(requestObject));
            sw.Flush();

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<DossierTemplate>(streamReader.ReadToEnd());
        }

        public async Task<Dossier> MoveTemplateFolder(string choosedFolderId, string folderId, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Template/Dossier/Move", "PUT");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            dynamic requestObject = new ExpandoObject();
            requestObject.DossierId = choosedFolderId;
            requestObject.NewParentId = folderId;

            // Add request object to request body
            var requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);
            sw.Write(JsonConvert.SerializeObject(requestObject));
            sw.Flush();

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<Dossier>(streamReader.ReadToEnd());
        }

        public async Task<TemplateWorkflow> GetTemplateWorkflow(string id)
        {
            var access_token = await Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(_baseUrl + "/api/Workflow/" + id);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                       "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<TemplateWorkflow>(sr.ReadToEnd());
        }


        public async Task<TemplateWorkflow> UpdateTemplateWorkflow(TemplateWorkflow workflow)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Workflow/Update", "PUT");

            //dynamic requestObject = new ExpandoObject();
            //requestObject.workflow = workflow;

            // Add request object to request body
            var requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream);
            sw.Write(JsonConvert.SerializeObject(workflow));
            sw.Flush();

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<TemplateWorkflow>(streamReader.ReadToEnd());
        }

        public async Task<List<Template>> GetArchivedTemplates(string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/ListArchived", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<List<Template>>(request);
        }

        public async Task<TemplateAvailableFields> GetTemplateAvailableFields(string templateId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/AvailableFields/{templateId}", "GET");
            return await GetResponseAsObject<TemplateAvailableFields>(request);
        }

        public async Task<TemplateAvailableClauses> GetTemplateAvailableClauses(string templateId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/AvailableClauses/{templateId}", "GET");
            return await GetResponseAsObject<TemplateAvailableClauses>(request);
        }

        public async Task<Dossier> GetTemplateParentFolder(string templateId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/{templateId}/Vault", "GET");
            return await GetResponseAsObject<Dossier>(request);
        }


        public async Task<HttpWebResponse> ArchiveTemplate(string templateId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/Archive/{templateId}", "PUT");
            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<HttpWebResponse> UnarchiveTemplate(string templateId, string templateName)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/Unarchive/{templateId}/{templateName}", "PUT");
            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<HttpWebResponse> DisableTemplate(string templateId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/Disable/{templateId}", "PUT");
            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public async Task<TemplateReferenceCompletionsResponseDto> GetTemplateReferenceCompletions(string templateId, string tenantId = null)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/Template/{templateId}/ReferenceCompletions", "GET");
            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await GetResponseAsObject<TemplateReferenceCompletionsResponseDto>(request);
        }

        public async Task<TemplateReferenceCompletionsResponseDto> ReplaceTemplateReferenceCompletions(
            string templateId,
            ReplaceTemplateReferenceCompletionsRequestDto requestDto,
            string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Template/{templateId}/ReferenceCompletions",
                "POST", bodyObject: requestDto);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<TemplateReferenceCompletionsResponseDto>(request);
        }


        public async Task<List<ReferenceDTO>> GetTemplateLinkedReferentials(string templateId, string tenantId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Template/{templateId}/Referentials", "GET");
            request.Headers.Add("TenantId", tenantId);
            return await this.GetResponseAsObject<List<ReferenceDTO>>(request);
        }

        public async Task<byte[]> DownloadTemplateAsZip(string id)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Template/Download/{id}", "GET");

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            byte[] content;
            using (MemoryStream mss = new MemoryStream())
            {
                responseStream.CopyTo(mss);
                mss.Seek(0, SeekOrigin.Begin);
                content = mss.ToArray();
            }
            return content;
        }
    }
}