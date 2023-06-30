namespace DocumentService;

public class FileService
{
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
            // Directory.Delete(tempDirectoryName, true);
        }
    }

    private static string TempDirectory()
    {
        // Get the system's temporary folder path
        string tempFolderPath = Path.GetTempPath();

        // Create a unique directory name
        string tempDirectoryName = Path.Combine(tempFolderPath, Guid.NewGuid().ToString());

        // Create the temporary directory
        Directory.CreateDirectory(tempDirectoryName);

        // Switch to the temporary directory
        Directory.SetCurrentDirectory(tempDirectoryName);

        return tempDirectoryName;
    }
}