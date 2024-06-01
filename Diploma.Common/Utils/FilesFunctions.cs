using Telegram.Bot;

namespace Diploma.Common.Utils;

public class FilesFunctions
{
    public static async Task<Stream> GetFileStreamAsync(ITelegramBotClient Client, string fileId)
    {
        var file = await Client.GetFileAsync(fileId);
        var memoryStream = new MemoryStream();
        await Client.DownloadFileAsync(file.FilePath, memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public static async Task<byte[]> ConvertStreamToByteArrayAsync(Stream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}