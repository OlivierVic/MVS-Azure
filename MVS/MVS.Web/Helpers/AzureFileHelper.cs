// <copyright file="AzureFileHelper.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using Azure.Storage.Blobs;

namespace MVS.Web.Helpers;

public class AzureFileHelper
{
    private readonly IConfiguration _configuration;
    private readonly BlobServiceClient _blobServiceClient;

    public AzureFileHelper(IConfiguration configuration)
    {
        this._configuration = configuration;
        this._blobServiceClient = new BlobServiceClient(configuration.GetValue<string>("ConnectionStrings:Storage"));
    }

    public async Task<string> UploadFolderDocument(string folderId, string documentName, Stream stream, int type)
    {
        BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("documents");
        if (!await containerClient.ExistsAsync())
        {
            await containerClient.CreateAsync();
        }

        string blobName = Path.Combine(folderId, type.ToString(), documentName).Replace("\\", "/");
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(stream, true);
        return blobClient.Uri.ToString();
    }

    public async Task<Azure.Response<bool>> RemoveFolderDocument(string folderId, string documentName, int type)
    {
        BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("documents");
        if (!await containerClient.ExistsAsync())
        {
            await containerClient.CreateAsync();
        }

        string blobName = Path.Combine(folderId, type.ToString(), documentName).Replace("\\", "/");
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<byte[]> GetUserFolderDocument(string folderId, string documentName, int type)
    {
        BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("documents");

        string blobName = Path.Combine(folderId, type.ToString(), documentName).Replace("\\", "/");

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            MemoryStream ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }

        return null;
    }

    public async Task<bool> CheckDoctorsListExist(string department)
    {
        BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("doctors");
        if (!await containerClient.ExistsAsync())
        {
            return false;
        }

        string blobName = $"{department}.pdf";

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.ExistsAsync();
    }

    public async Task<byte[]> GetDoctorsList(string department)
    {
        BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("doctors");

        string blobName = $"{department}.pdf";

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            MemoryStream ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }

        return null;
    }

    public async Task<string> UploadDoctorsList(string department, Stream stream)
    {
        BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient("doctors");
        if (!await containerClient.ExistsAsync())
        {
            await containerClient.CreateAsync();
        }

        string blobName = $"{department}.pdf";
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(stream, true);
        return blobClient.Uri.ToString();
    }
}
