using Azure;
using Azure.Storage.Blobs.Models;

namespace ibge.Providers.interfaces;

public interface IBlobProvider
{
    Task<Response<BlobContentInfo>> UploadFile(string fileName, Stream stream);
    string GetDownloadUrl(string fileName);
}