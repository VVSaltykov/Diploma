using System.IO.Compression;
using Diploma.Common.Models;

namespace Diploma.Common.Utils;

public class ZipFileCreator
{
    public static async Task<byte[]> CreateZipFile(List<Files> filesList)
    {
        using (MemoryStream zipStream = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in filesList)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(file.FileName);
                    using (Stream entryStream = entry.Open())
                    {
                        await entryStream.WriteAsync(file.FileData, 0, file.FileData.Length);
                    }
                }
            }

            byte[] zipBytes = zipStream.ToArray();

            return zipBytes;
        }
    }
}