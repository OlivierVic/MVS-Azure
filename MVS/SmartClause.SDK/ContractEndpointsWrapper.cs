using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Smartclause.SDK.DTO;
using Smartclause.SDK.Tools;
using SmartClause.SDK.DTO;
using SmartClause.SDK.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<bool> CheckContractAccess(string contractId, string enduserId, string enduseremail)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/CheckAccess?contractId={HttpUtility.UrlEncode(contractId)}", "GET");
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<bool>(sr.ReadToEnd());

        }

        public async Task<bool> MarkContractAsFinalized(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/Finalized", "PUT");
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<bool>(sr.ReadToEnd());
        }

        public async Task ArchiveContract(string contractId, string platform)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Archive/{platform}/{contractId}", "GET");
            await request.GetResponseAsync();
        }

        public async Task UnarchiveContract(string contractId, string platform)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Unarchive/{platform}/{contractId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task DisableContract(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Disable/{contractId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task<string> GetContractHasHTML(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/html/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "text/html";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<string> GetContractData(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/data/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "text/html";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<List<ContractField>> GetContractField(string contractId, string tenantId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/fields/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "text/html";
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<ContractField>>(sr.ReadToEnd());
        }

        public async Task<byte[]> GeneratePDFFromHTML(string html)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/generate/pdf");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "text/html, text/javascript, */*; q=0.01";
            request.ContentType = "text/html";
            Stream ms = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(html));
            sw.Flush();
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

        public async Task<byte[]> GeneratePDFFromHTMLV2(string html)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/generate/pdfv2");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "text/html, text/javascript, */*; q=0.01";
            request.ContentType = "text/html";
            Stream ms = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(html));
            sw.Flush();
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

        public async Task<string> GetContractAsPDF(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/pdf/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "text/plain";

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<string> GetContractAsPDFV2(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/pdfv2/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "text/plain";

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<byte[]> GetContractAsPDFAsByteArray(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/pdfv2/{contractId}/ByteArray", "GET");

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

        public async Task<byte[]> GetContractAsWordAsByteArray(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/doc/{contractId}/ByteArray", "GET");

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

        public async Task<PrepareContractResult> Copy(string templateId, string email, string data, string title,
            string folderId, string actions, string contractCopyId, string userId = null,
            List<Conversation> conversations = null)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Copy");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic dt = new ExpandoObject();

            dt.TemplateId = templateId;
            dt.Email = email;
            dt.Data = data;
            dt.UserId = userId;
            dt.Title = title;
            dt.Actions = actions;
            dt.VaultId = folderId;
            dt.ContractCopyId = contractCopyId;

            if (conversations != null)
            {
                dt.Conversations = conversations;
            }

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<PrepareContractResult>(sr.ReadToEnd());
        }

        public async Task<ContractDto> GetLightContract(string contractId, string tenantId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Light/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<ContractDto>(sr.ReadToEnd());
        }


        public async Task<ContractDto> GetContract(string contractId, string tenantId = null)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<ContractDto>(sr.ReadToEnd());
        }

        public async Task<string> SetContractFilled(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SetFilled/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<string> SetContractTransfered(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SetTransfered/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<string> SetContractValidated(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Validate/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<string> SetSentForSignatureWorkflow(string contractId, string UID)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SetSentForSignatureWorkflow");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            ContractWithUserModel model = new ContractWithUserModel()
            {
                Id = contractId,
                UID = UID
            };

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<CompareResult> CompareVersion(int version1_Id, int version2_Id)
        {
            try
            {
                string access_token = await this.Logon();

                HttpWebRequest request =
                    HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/CompareVersions/" + version1_Id + "/" +
                                              version2_Id);
                request.Headers[HttpRequestHeader.Authorization] = string.Format(
                    "Bearer {0}", access_token);
                request.Method = "GET";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";

                WebResponse response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                return JsonConvert.DeserializeObject<CompareResult>(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> ValidateContractWorkflowStepUser(ValidateContractWorkflowStepUserModel model)
        {
            string access_token = await this.Logon();

            HttpWebRequest request =
                HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/ValidateContractWorkflowStepUser");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
        }

        public async Task<string> ValidateContractWorkflowStep(ValidateContractWorkflowStepModel model, string tenantId = null)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/ValidateContractWorkflowStep");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
        }

        public async Task<string> UpdateWorkflowContractInvalidStepsAndAddHandSignStep(
            string contractId, string HandSignature, string endUserEmail, string tenantId)
        {
            List<string> body = new List<string>() { contractId, HandSignature };
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/UpdateWorkflowContractInvalidStepsAndAddHandSignStep", "PUT", bodyObject: body);
            request.Headers.Add("TenantId", tenantId);

            return await this.GetResponseAsObject<string>(request);
        }

        public async Task<WorkflowUpdateResponseDto> UpdateWorkflowContractSteps(
            List<WorkflowContractSteps> workflowContractSteps, string endUserEmail, string tenantId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/UpdateWorkflowContractSteps", "PUT");
            request.Headers.Add("TenantId", tenantId);

            Stream ms = request.GetRequestStream();
            dynamic dt = new ExpandoObject();

            dt.WorkflowContractSteps = workflowContractSteps;
            dt.EndUserEmail = endUserEmail;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();

            return await this.GetResponseAsObject<WorkflowUpdateResponseDto>(request);
        }

        public async Task<List<WorkflowContractStepUser>> GetWorkflowStepUser(string stepId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{stepId}/Workflow/party/search", "POST");
            return await this.GetResponseAsObject<List<WorkflowContractStepUser>>(request);
        }

        public async Task<string> GetVersionHtml(int version_Id)
        {
            try
            {
                string access_token = await this.Logon();

                HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/GetVersion/" + version_Id);
                request.Headers[HttpRequestHeader.Authorization] = string.Format(
                    "Bearer {0}", access_token);
                request.Method = "GET";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";

                WebResponse response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                return JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
            }
            catch
            {
                return "KO";
            }
        }

        public async Task<string> RestoreVersion(string contractId, int versionNumber)
        {
            try
            {
                string access_token = await this.Logon();
                HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl +
                                                                   "/api/Contract/RestoreVersion?contractId=" +
                                                                   contractId + "&versionNumber=" + versionNumber);
                request.Method = "GET";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";
                WebResponse response = await request.GetResponseAsync();
                return "OK";
            }
            catch (Exception e)
            {
                return "KO";
            }
        }

        public async Task<string> SetContractName(string contractId, string name)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SetName");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            ChangeNameModel model = new ChangeNameModel()
            {
                Id = contractId,
                Name = name
            };

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<List<Party>> SetContractUsers(string contractId, List<Party> parties)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SetUsers");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic dt = new ExpandoObject();

            dt.Id = contractId;
            dt.Parties = parties;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<List<Party>>(sr.ReadToEnd());
        }

        public async Task<string> SetContractParties(string contractId, string parties)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SetParties");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic dt = new ExpandoObject();

            dt.Id = contractId;
            dt.Parties = parties;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
        }

        public async Task<SignContractResult> SignContract(SignContractRequest model, string tenantId = null)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Sign");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";

            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<SignContractResult>(sr.ReadToEnd());
        }

        public async Task<SignContractResult> SignMultipleContracts(SignMultipleContractsRequest model)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SignMultiple");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<SignContractResult>(sr.ReadToEnd());
        }

        public async Task<SignContractResult> SignMultipleContractsAddAnchor(ContractsSignatureAnchor model)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/SignAddAnchor");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(model));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<SignContractResult>(sr.ReadToEnd());
        }

        public async Task VoidEnvelope(ContractVoidDocuSignEnvelope model)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/VoidDocuSignEnvelope", "POST", bodyObject: model);
            await request.GetResponseAsync();
        }

        public async Task<string> GetContractModel(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/model/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return sr.ReadToEnd();
        }

        public async Task<List<ContractDto>> GetContractList(string folderId, string tenantId = null)
        {
            LogonResult logonResult = await this.LogonExtended();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/List/" + logonResult.userId +
                                                               "/" + (folderId ?? "null"));
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", logonResult.access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<ContractDto> tmp = JsonConvert.DeserializeObject<List<ContractDto>>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<List<FolderDto>> GetContractFolderCrumbs(string folderId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Crumbs/" + folderId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<FolderDto> tmp = JsonConvert.DeserializeObject<List<FolderDto>>(sr.ReadToEnd());
            return tmp;
        }


        public async Task<List<FolderDto>> GetContractFolders(string folderId, bool hideDisabled = false, string tenantId = null)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Folders?folderId=" + folderId +
                                                               "&hideDisabled=" + hideDisabled);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<FolderDto> tmp = JsonConvert.DeserializeObject<List<FolderDto>>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<List<ContractDto>> GetAllArchivedContracts()
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest("/api/Contract/GetAllArchived", "GET");
            return await this.GetResponseAsObject<List<ContractDto>>(webRequest);
        }

        public async Task<List<FolderDto>> GetAllArchivedFolders()
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest("/api/Contract/Vault/GetAllArchived", "GET");
            return await this.GetResponseAsObject<List<FolderDto>>(webRequest);
        }

        public async Task<List<FolderDto>> GetAllContractFolders(string userId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/AllFolders?userId=" + userId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<FolderDto> tmp = JsonConvert.DeserializeObject<List<FolderDto>>(sr.ReadToEnd());
            return tmp;
        }


        public async Task<List<FolderDto>> GetContractChildFolders(string folderId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request =
                HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/ChildFolders?folderId=" + folderId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<FolderDto> tmp = JsonConvert.DeserializeObject<List<FolderDto>>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<FolderDto> CreateFolder(string userId, string folderName, string parentId,
            bool frozen = false, bool frozenContent = false, bool confidential = false, string tenantId = null)
        {
            try
            {
                string access_token = await this.Logon();

                HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Vault/Create");
                request.Headers[HttpRequestHeader.Authorization] = string.Format(
                    "Bearer {0}", access_token);
                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json";
                Stream ms = request.GetRequestStream();

                dynamic d = new ExpandoObject();
                d.FolderName = folderName;
                d.ParentId = parentId;
                d.UserId = userId;
                d.Frozen = frozen;
                d.FrozenContent = frozenContent;
                d.Confidential = confidential;
                d.TenantId = tenantId;
                StreamWriter sw = new StreamWriter(ms);
                sw.Write(JsonConvert.SerializeObject(d));
                sw.Flush();
                WebResponse response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);

                return JsonConvert.DeserializeObject<FolderDto>(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task CreateFolderHierarchy(CreateFolderHierarchyRequestDto requestDto)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest("api/Contract/Vault/CreateHierarchy", "POST", bodyObject: requestDto);
            await request.GetResponseAsync();
        }

        public async Task<ContractMultipleFiles> GetContractFromMultiple(string templateId, string data,
            int marginTop = 20, int marginBottom = 20, int marginLeft = 20, int marginRight = 20,
            bool addPageNumbers = false)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(String.Format("{0}{1}{2}/{3}/{4}/{5}/{6}/{7}", this._baseUrl,
                "/api/Template/FillMCFTemplate/", templateId, marginTop, marginBottom, marginLeft, marginRight,
                addPageNumbers));
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";

            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(data);
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();

            ContractMultipleFiles contract = new ContractMultipleFiles
            {
                Name = templateId + ".pdf",
                Content = responseStream
            };

            return contract;
        }

        public async Task<string> UpdateContractRights(string contractId, string userId,
            Dictionary<string, int> userRoles)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/UpdateRights");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic obj = new ExpandoObject();
            obj.id = contractId;
            obj.userRoles = userRoles;
            obj.userId = userId;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(obj));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            string tmp = JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<List<CommentModel>> GetContractComments(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Comments/" + contractId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<CommentModel> tmp = JsonConvert.DeserializeObject<List<CommentModel>>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<CommentModel> AddComment(CommentModel comment)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Comment");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();


            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(comment));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<CommentModel>(sr.ReadToEnd());
        }

        public async Task<string> GetUserColors(string userEmail)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Template/Colors/");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(userEmail));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            string tmp = JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<List<Party>> GetContractParties(string contractId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/" + contractId + "/Parties");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            List<Party> tmp = JsonConvert.DeserializeObject<List<Party>>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<Party> UpdateParty(Party p)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/UpdateParty");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(p));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            Party tmp = JsonConvert.DeserializeObject<Party>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<SearchContractResult> SearchContractAndFolders(string str, string userId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Search/" + userId + "/" + str);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "GET";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";


            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            return JsonConvert.DeserializeObject<SearchContractResult>(sr.ReadToEnd());
        }

        public async Task<FolderDto> UpdateFolder(string folderId, string newName, bool? frozen, bool? frozenContent, bool? confidential)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Vault/Update");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "PUT";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic d = new
            {
                VaultId = folderId,
                FolderNewName = newName,
                Frozen = frozen,
                FrozenContent = frozenContent,
                Confidential = confidential
            };

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(d));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            FolderDto tmp = JsonConvert.DeserializeObject<FolderDto>(sr.ReadToEnd());
            return tmp;
        }

        public async void DeleteVault(string folderId)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/Vault/Delete/" + folderId);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "DELETE";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
        }

        public async Task<ContactAnchorResponse> AddContactAnchorsToDocument(string contractId,
            List<UserContact> contacts)
        {
            string access_token = await this.Logon();

            HttpWebRequest request = HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/AddContacts");
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic obj = new ExpandoObject();
            obj.ContractId = contractId;
            obj.Contacts = contacts;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(obj));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            ContactAnchorResponse tmp = JsonConvert.DeserializeObject<ContactAnchorResponse>(sr.ReadToEnd());
            return tmp;
        }

        public async Task<string> UpdateFirstValideTime(string id)
        {
            string access_token = await this.Logon();

            HttpWebRequest request =
                HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/UpdateFirstValideTime?contractId=" + id);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";
            Stream ms = request.GetRequestStream();

            dynamic dt = new ExpandoObject();

            dt.contractId = id;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
        }

        public async Task<string> UpdateFinalValidatedTime(string id)
        {
            string access_token = await this.Logon();

            HttpWebRequest request =
                HttpWebRequest.CreateHttp(this._baseUrl + "/api/Contract/UpdateFinalValidatedTime?contractId=" + id);
            request.Headers[HttpRequestHeader.Authorization] = string.Format(
                "Bearer {0}", access_token);
            request.Method = "POST";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/json";

            Stream ms = request.GetRequestStream();

            dynamic dt = new ExpandoObject();

            dt.contractId = id;

            StreamWriter sw = new StreamWriter(ms);
            sw.Write(JsonConvert.SerializeObject(dt));
            sw.Flush();
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);

            return JsonConvert.DeserializeObject<string>(sr.ReadToEnd());
        }

        public async Task<string> CreateContractFromFile(
            string title,
            string field,
            string language,
            string nation,
            string desc,
            IFormFile file,
            string email,
            string uid,
            string folderId = null,
            List<Conversation> conversations = null,
            List<string> refElemsIds = null,
            string contractType = null,
            string templateId = null,
            string tenantId = null)
        {
            dynamic dt = new ExpandoObject();
            dt.Title = title;
            dt.Field = field;
            dt.Language = language;
            dt.Nation = nation;
            dt.Description = desc;
            dt.UserId = uid;
            if (folderId != null)
            {
                dt.VaultId = folderId;
            }

            if (conversations != null)
            {
                dt.Conversations = conversations;
            }

            dt.RefElemsIds = refElemsIds;
            dt.ContractType = contractType;
            dt.TemplateId = templateId;
            dt.Email = email;
            dt.TenantId = tenantId;


            MemoryStream target = new();
            await file.OpenReadStream().CopyToAsync(target);
            byte[] bytes = target.ToArray();

            Dictionary<string, object> postParameters = new();
            postParameters.Add("contractInformations", JsonConvert.SerializeObject(dt));
            postParameters.Add("file", new RequestHelper.FileParameter(bytes, file.FileName, "text/html"));

            string formDataBoundary = $"----------{Guid.NewGuid():N}";
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = RequestHelper.GetMultipartFormData(postParameters, formDataBoundary);

            HttpWebRequest request =
                await this.CreateHttpWebRequest("/api/Contract/CreateContractFromFile",
                    "POST",
                    contentType,
                    "");

            await using (Stream requestStream = request.GetRequestStream())
            {
                await requestStream.WriteAsync(formData.AsMemory(0, formData.Length));
                requestStream.Close();
            }

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<string>(request);
        }

        public async Task<ContractDto> SetContractAdditionalInformation(ContractAdditionalInformationRequest request)
        {
            HttpWebRequest webRequest =
                await this.CreateHttpWebRequest("/api/Contract/SetAdditionalInformation", "POST", bodyObject: request);
            return await this.GetResponseAsObject<ContractDto>(webRequest);
        }

        public async Task<List<List<FolderDto>>> GetContractCrumbs(string contractId, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/Crumbs", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<List<FolderDto>>>(request);
        }

        public async Task AddContractToFolder(string contractId, string folderId, string tenantId = null)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Contract/{contractId}/AddToFolder/{folderId}", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            await request.GetResponseAsync();
        }

        public async Task RemoveContractFromFolder(string contractId, string folderId, string tenantId = null)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Contract/{contractId}/RemoveFromFolder/{folderId}", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            await request.GetResponseAsync();
        }

        public async Task UpdateContractMetadata(string contractId,
            UpdateContractMetadataRequest updateContractMetadataRequest, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/UpdateMetadata", "PATCH",
                bodyObject: updateContractMetadataRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task UpdateContractEnvelopeId(string contractId, UpdateContractSignInfoRequest updateContractSignInfoRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/UpdateSignInfo", "PATCH", bodyObject: updateContractSignInfoRequest);
            await request.GetResponseAsync();
        }

        public async Task<byte[]> GetContributorContractsZip(string contributorEmail)
        {
            string encodedContributorEmail = HttpUtility.UrlEncode(contributorEmail);
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Contract/Contributor/Zip?contributorEmail={encodedContributorEmail}", "GET");
            FileContentResponse response = await this.GetResponseAsObject<FileContentResponse>(request);
            return Convert.FromBase64String(response.FileContents);
        }

        public async Task<List<ContractDto>> SearchContributorContracts(SearchContributorContractsRequest body, string tenantId = null)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest("/api/Contract/Contributor/Search", "POST", bodyObject: body);

            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<List<ContractDto>>(request);
        }

        public async Task<ContractDto> GetContractWithConditions(string contractId, string tenantId, string endUserId)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Contract/WithConditions/{contractId}?endUserId={endUserId}", "GET");
            if (!String.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<ContractDto>(request);
        }

        public async Task<FlattenHierarchyResponseDto> FlattenHierarchy(string folderId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/FlattenHierarchy/{folderId}", "GET");
            return await this.GetResponseAsObject<FlattenHierarchyResponseDto>(request);
        }

        public async Task<List<string>> GetWorkflowStepUsersEmails(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/GetWorkflowStepUsers", "GET");
            return await this.GetResponseAsObject<List<string>>(request);
        }

        public async Task<bool> AddContributorToWorkflowStep(AddContributorToWorkflowStepRequest addContributorToWorkflowStepRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Contract/AddContributorToWorkflowStep",
                "POST", bodyObject: addContributorToWorkflowStepRequest);
            return await this.GetResponseAsObject<bool>(request);
        }

        public async Task<List<CurrentNegotiationDto>> GetCurrentNegotiations(string userEmail, string tenantId)
        {
            dynamic userEmailRequest = new ExpandoObject();
            userEmailRequest.UserEmail = userEmail;
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Contract/GetCurrentNegotiations", "POST",
                bodyObject: userEmailRequest);
            if (tenantId != null)
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<List<CurrentNegotiationDto>>(request);
        }

        public async Task<string> GetSignedDocumentSasUri(string tenantId, string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/GetSignedDocumentSasUri", "GET");
            if (tenantId != null)
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<string>(request);
        }

        public async Task<ContractDto> ValidateSignedDocumentImport(string tenantId, string contractId, ValidateSignedDocumentImportRequest body)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/ValidateSignedDocumentImport", "POST", bodyObject: body);
            if (tenantId != null)
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<ContractDto>(request);
        }

        public async Task<ContractDto> ForceSignContract(string tenantId, string contractId, ForceSignContractRequest body)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/{contractId}/ForceSign", "POST", bodyObject: body);
            if (tenantId != null)
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<ContractDto>(request);
        }

        public async Task<ConversationMessageDto> GetContractLastMessage(string tenantId, string contractId, DateTime? before = null)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (before != null) query["before"] = before.Value.ToUniversalTime().ToString();
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/GetLastMessage/{contractId}?{query}", "GET");
            request.Headers.Add("TenantId", tenantId);
            return await this.GetResponseAsObject<ConversationMessageDto>(request);
        }

        public async Task<CommentModel> GetContractLastComment(string tenantId, string contractId, DateTime? before = null)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (before != null) query["before"] = before.Value.ToUniversalTime().ToString();
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/GetLastMessage/{contractId}?{query}", "GET");
            request.Headers.Add("TenantId", tenantId);
            return await this.GetResponseAsObject<CommentModel>(request);
        }
    }
}