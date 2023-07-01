namespace DocumentService;

public class FileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> GetFile(string html)
    {
        var tempDirectoryName = TempDirectory();

        try
        {
            var fileName = Path.GetRandomFileName() + ".txt";
            var filePath = Path.Combine(tempDirectoryName, fileName);

            await File.WriteAllTextAsync(filePath, html);
            var result = await File.ReadAllBytesAsync(filePath);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            throw;
        }
        finally
        {
            // Remove the temporary directory and its contents
            Directory.Delete(tempDirectoryName, true);
            _logger.LogInformation("Tagit bort arbetskatalog: {Dir}", tempDirectoryName);
        }
    }

    private string TempDirectory()
    {
        // Create a unique directory name
        var tempDirectoryName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        // Create the temporary directory
        Directory.CreateDirectory(tempDirectoryName);

        _logger.LogInformation("Skapat arbetskatalog: {Dir}", tempDirectoryName);

        return tempDirectoryName;
    }
}