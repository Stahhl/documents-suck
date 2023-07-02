using System.Text.Json;
using CliWrap;

namespace DocumentService;

public class LatexService
{
    private readonly string _workDir = TempDirectory();

    private const string InputHtml = "input.html";
    private const string InputTex = "input.tex";
    private const string OutputTex = "output.tex";
    private const string OutputPdf = "output.pdf";
    private const string Payload = "payload.json";
    private const string TemplateTex = "template.tex";
    private const string LogoImage = "logo.jpg";

    public async Task<(byte[] pdfBytes, string workDir)> HtmlToLatexToPdf(string html)
    {
        try
        {
            var inputLatexFilePath = await ConvertHtmlToLatex(html);
            var outputLatexFilePath = await InsertLatexInTemplate(inputLatexFilePath);
            var pdfBytes = await LatexToPdf(outputLatexFilePath);

            return new(pdfBytes, _workDir);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<byte[]> LatexToPdf(string inputLatexFilePath)
    {
        File.Copy(Path.Combine(FileService.StaticFilePath, LogoImage), Path.Combine(_workDir, LogoImage));

        await Cli.Wrap("pdflatex")
            .WithArguments(args =>
            {
                args.Add(inputLatexFilePath);
            })
            .WithWorkingDirectory(_workDir)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
            .ExecuteAsync();

        var result = await File.ReadAllBytesAsync(Path.Combine(_workDir, OutputPdf));

        return result;
    }

    private async Task<string> InsertLatexInTemplate(string inputLatexFilePath)
    {
        var inputLatex = await File.ReadAllTextAsync(inputLatexFilePath);
        var json = JsonSerializer.Serialize(new { data = inputLatex });

        var templateFilePath = Path.Combine(FileService.StaticFilePath, TemplateTex);
        var inputFilePath = Path.Combine(_workDir, Payload);
        var outputFilePath = Path.Combine(_workDir, OutputTex);

        await File.WriteAllTextAsync(inputFilePath, json);

        await Cli.Wrap("jinja")
            .WithArguments(args =>
            {
                args.Add("-d")
                    .Add(inputFilePath)
                    .Add("-o")
                    .Add(outputFilePath)
                    .Add(templateFilePath);
            })
            .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
            .ExecuteAsync();

        await File.ReadAllTextAsync(outputFilePath);

        return outputFilePath;
    }

    private async Task<string> ConvertHtmlToLatex(string html)
    {
        var inputFilePath = Path.Combine(_workDir, InputHtml);
        var outputFilePath = Path.Combine(_workDir, InputTex);

        await File.WriteAllTextAsync(inputFilePath, html);

        await Cli.Wrap("docker")
            .WithArguments(args =>
            {
                args.Add("run")
                    .Add("--rm")
                    .Add("--volume")
                    .Add($"{_workDir}:/data")
                    .Add("pandoc/latex")
                    .Add(InputHtml)
                    .Add("-f")
                    .Add("html")
                    .Add("-t")
                    .Add("latex")
                    .Add("--output")
                    .Add(InputTex);
            })
            .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
            .ExecuteAsync();

        await File.ReadAllTextAsync(outputFilePath);

        return outputFilePath;
    }

    private static string TempDirectory()
    {
        // Create a unique directory name
        var tempDirectoryName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        // Create the temporary directory
        Directory.CreateDirectory(tempDirectoryName);

        return tempDirectoryName;
    }
}