using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC.StorageProcessor.Interfaces
{
	public interface IDocumentProcessor
	{
		Task<string> UploadDocument(string directory, string fileName, Stream fileStream);
		Task<byte[]> GetDocument(string directory, string documentPath);

		Task<bool> ReplaceDocument(string directory, string documentPath, Stream fileStream);
	}
}
