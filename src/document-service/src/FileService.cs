using System.IO.Compression;

namespace DocumentService;

public class FileService
{
    public const string StaticFilePath = "./static";

    public async Task<byte[]?> GetStaticFile(string fileName)
    {
        var filePath = Path.Combine(StaticFilePath, fileName);

        if (!File.Exists(filePath)) return null;

        return await File.ReadAllBytesAsync(filePath);
    }

    public static string GetRandomFilenameWithoutExtension()
    {
        return Path.ChangeExtension(Path.GetRandomFileName(), null);
    }

    public static bool DeleteDirectory(string directory)
    {
        Directory.Delete(directory, true);

        return !Directory.Exists(directory);
    }

    public static byte[] ZipDirectory(string directoryPath)
    {
        using var ms = new MemoryStream();
        using var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true);

        var directoryInfo = new DirectoryInfo(directoryPath);
        var basePath = directoryInfo.Parent?.FullName ?? throw new ArgumentNullException();

        foreach (var file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
        {
            var entryName = file.FullName[(basePath.Length + 1)..];
            zipArchive.CreateEntryFromFile(file.FullName, entryName);
        }

        return ms.ToArray();
    }
}