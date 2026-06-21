using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace ExpenseTracker.Services.Utilities
{
    public static class AttachmentCompressionHelper
    {
        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        // Helper method to convert byte array to FileContentResult
        public static FileContentResult ConvertToFileContentResult(byte[] fileContent, string contentType, string fileName)
        {
            return new FileContentResult(fileContent, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
