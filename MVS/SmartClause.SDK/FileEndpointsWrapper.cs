using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<FileDto> CreateFile(CreateFileRequest request, string tenantId = null)
        {
            var webRequest = await this.CreateHttpWebRequest("/api/File/Create", "POST", bodyObject: request);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                webRequest.Headers.Add("TenantId", tenantId);
            }
            else if (request.File != null && !string.IsNullOrWhiteSpace(request.File.TenantId))
            {
                webRequest.Headers.Add("TenantId", request.File.TenantId);
            }
            return await this.GetResponseAsObject<FileDto>(webRequest);
        }

        public async Task<List<FileDto>> GetFiles(string folderId = null, string tenantId = null)
        {
            var webRequest = await this.CreateHttpWebRequest($"/api/File/FindAllInFolder?folderId={folderId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                webRequest.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<FileDto>>(webRequest);
        }

        public async Task<List<FileDto>> GetAllArchivedFiles()
        {
            var webRequest = await this.CreateHttpWebRequest("/api/File/GetAllArchived", "GET");
            return await this.GetResponseAsObject<List<FileDto>>(webRequest);
        }

        public async Task<FileDto> GetFile(string fileId, string tenantId = null)
        {
            var webRequest = await this.CreateHttpWebRequest($"/api/File/{fileId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                webRequest.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<FileDto>(webRequest);
        }

        public async Task DeleteFile(string fileId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/Delete/{fileId}", "DELETE");
            await request.GetResponseAsync();
        }

        public async Task RenameFile(string fileId, RenameFileRequest renameFileRequest)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/File/Rename/{fileId}", "PATCH", bodyObject: renameFileRequest);
            await request.GetResponseAsync();
        }

        public async Task ArchiveFile(string fileId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/Archive/{fileId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task UnarchiveFile(string fileId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/Unarchive/{fileId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task DisableFile(string fileId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/Disable/{fileId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task<string> GetImportedFileTermSheetHtml(string fileId, string endUserId, string lang, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/TermSheet/Html/Get/{lang}/{fileId}/{endUserId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            using var streamReader = new StreamReader(responseStream);
            var responseString = await streamReader.ReadToEndAsync();
            return responseString;
        }
        public async Task<List<List<FolderDto>>> GetFileCrumbs(string fileId, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/{fileId}/Crumbs", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<List<FolderDto>>>(request);
        }

        public async Task AddFileToFolder(string fileId, string folderId, string tenantId = null)
        {

            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/{fileId}/AddToFolder/{folderId}", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            await request.GetResponseAsync();
        }

        public async Task RemoveFileFromFolder(string fileId, string folderId, string tenantId = null)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/File/{fileId}/RemoveFromFolder/{folderId}", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task UpdateFileMetadata(string fileId, UpdateFileMetadataRequest updateFileMetadataRequest, string tenantId = null)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/File/{fileId}/UpdateMetadata", "PATCH",
                    bodyObject: updateFileMetadataRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task<FileDto> ForceSignFile(string tenantId, string fileId, ForceSignFileRequest body)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/File/{fileId}/ForceSign", "POST", bodyObject: body);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<FileDto>(request);
        }
    }
}