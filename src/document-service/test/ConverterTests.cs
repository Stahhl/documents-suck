using System.Text.Json;

namespace DocumentService.Test;

public class ConverterTests
{
    [Fact]
    public async Task ConvertHtmlToLatexTest()
    {
        const string html = """<div><h1>hello</h1></div>""";

        var service = new LatexService();

        var result = await service.HtmlToLatexToPdf(html);
        
        Assert.True(result.Length > 0);
    }

    [Fact]
    public void foo()
    {
        const string bar =
            """
            \hypertarget{hello-world}{%
            \section{Hello World}\label{hello-world}}

            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer erat
            enim, ultrices sed rhoncus quis, egestas ac mauris. Proin massa elit,
            ultrices blandit libero quis, venenatis commodo nunc. Nullam
            sollicitudin finibus nisl. Fusce vel lacinia tellus, eu bibendum ligula.
            Ut scelerisque diam felis, sit amet volutpat tellus ornare eu. Sed
            molestie tortor in mauris laoreet dapibus. Vivamus euismod imperdiet
            ornare. Nulla ut tempor nisi. In placerat et lorem vel condimentum.
            Integer viverra lorem ac turpis porta tincidunt.
            """;

        var foobar = new { data = bar };

        var json = JsonSerializer.Serialize(foobar, new JsonSerializerOptions() { WriteIndented = true });
    }
}