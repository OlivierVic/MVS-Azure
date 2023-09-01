using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public Task<List<FolderDto>> GetRootFolders(string tenantId = null)
        {
            return this.GetContractFolders(null, tenantId: tenantId);
        }

        public async Task UpdateFolderLastAccessTime(string folderId)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Contract/Vault/{folderId}/UpdateLastAccessTime", "POST");
            await request.GetResponseAsync();
        }

        public async Task ArchiveFolder(string folderId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Vault/Archive/{folderId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task UnarchiveFolder(string folderId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Vault/Unarchive/{folderId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task DisableFolder(string folderId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Vault/Disable/{folderId}", "PUT");
            await request.GetResponseAsync();
        }

        public async Task EmptyFolderTrash(string tenantId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Vault/EmptyTrash", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task<FolderDto> GetFolder(string folderId, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Vault/{folderId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<FolderDto>(request);
        }

        public async Task MoveFolder(string folderId, string newFolderId)
        {
            var body = new { VaultId = folderId, NewFolderId = newFolderId };
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Contract/Vault/Move", "PUT", bodyObject: body);
            await request.GetResponseAsync();
        }

        public async Task<List<FolderDto>> LastVisitedFolders(string tenantId = null, params string[] folders)
        {
            LastVisitedFolderRequest foldersRequest = new LastVisitedFolderRequest() { Folders = folders };
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Drive/LastVisitedFolders", "POST", bodyObject: foldersRequest);

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<List<FolderDto>>(request);
        }
    }
}