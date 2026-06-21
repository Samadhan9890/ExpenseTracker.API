using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using NC.StorageProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC.StorageProcessor.Implementations
{
	public class AzStorageDocumentProcessor : IDocumentProcessor
	{
		private readonly BlobServiceClient _blobServiceClient;
		private readonly ILogger _logger;

		public AzStorageDocumentProcessor(BlobServiceClient blobServiceClient)
		{
			_blobServiceClient = blobServiceClient;
				
		}

		/// <summary>
		/// This method gets the blob present in the contaner and returns the binary data of file
		/// </summary>
		/// <param name="directory"> Container name</param>
		/// <param name="documentPath">Path of the blob</param>
		/// <returns></returns>
		public async Task<byte[]> GetDocument(string directory, string documentPath)
		{
			BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(directory);
			BlobClient blobClient = blobContainerClient.GetBlobClient(documentPath);
			Azure.Response<BlobDownloadInfo> response = await blobClient.DownloadAsync();

			using MemoryStream ms = new();
			await response.Value.Content.CopyToAsync(ms);
			return ms.ToArray();
		}

		/// <summary>
		/// Replace the document present in the blob matching the name and path
		/// </summary>
		/// <param name="directory">Name of container</param>
		/// <param name="documentPath">complete path of blob with name and extension</param>
		/// <param name="fileStream">Binary of the file to upload/replace</param>
		/// <returns></returns>
		public async Task<bool> ReplaceDocument(string directory, string documentPath, Stream fileStream)
		{
			try
			{
				BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(directory);
				BlobClient blobClient = blobContainerClient.GetBlobClient(documentPath);
				await blobClient.UploadAsync(fileStream, overwrite: true);
				return true;
			}
			catch (Exception)
			{
				return false;				
			}

		}

		/// <summary>
		/// This function uploads the file to storage container
		/// </summary>
		/// <param name="directory">It is container(folder) name inside the storage container</param>
		/// <param name="fileName">Name of file along with path</param>
		/// <param name="fileStream">The file to upload</param>
		/// <returns></returns>
		public async Task<string> UploadDocument(string directory, string fileName, Stream fileStream)
		{
			var blobContainerClient =  _blobServiceClient.GetBlobContainerClient(directory);
			await blobContainerClient.CreateIfNotExistsAsync();
			
			var blobClient = blobContainerClient.GetBlobClient(fileName);			
			await blobClient.UploadAsync(fileStream);

			return fileName;
		}


	}
}
