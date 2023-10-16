using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ibge.Providers.interfaces;

namespace ibge.Providers;

public class BlobProvider : IBlobProvider
{
	private readonly BlobServiceClient _blobServiceClient;
	private const string Container = "bulks";

	public BlobProvider(BlobServiceClient blobServiceClient)
	{
		_blobServiceClient = blobServiceClient;
	}
	
	public async Task<Response<BlobContentInfo>> UploadFile(string fileName, Stream stream)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(Container);
		var blobClient = containerClient.GetBlobClient(fileName);
		return await blobClient.UploadAsync(stream);
	}
	
	public static string GetBlobServiceClientSas(string accountName, string sasToken)
	{
		string blobServiceUri = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={sasToken};EndpointSuffix=core.windows.net";
		return blobServiceUri;
	}
	
	public string GetDownloadUrl(string filename)
	{
		return $"https://bulks.blob.core.windows.net/bulks/{filename}";
	}
}