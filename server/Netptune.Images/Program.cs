using Flurl;

using Netptune.Images.Processing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddOutputCache(options =>
{
    options.SizeLimit = 12_046;
    options.UseCaseSensitivePaths = true;
    options.DefaultExpirationTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

const string basePath = "https://netptune-cloud.s3.eu-west-2.amazonaws.com/production";

app.MapGet("/{path}", async (string path, IHttpClientFactory clientFactory) =>
{
    var source = basePath.AppendPathSegment(path);
    var client = clientFactory.CreateClient();

    var response = await client.GetStreamAsync(source);
    var processed = ImageProcessor.ProcessStream(response, new ());

    if (processed is null) return Results.NotFound();

    return Results.File(processed, "image/webp");

}).CacheOutput(b => b.SetVaryByRouteValue("path"));

app.UseOutputCache();

app.Run();
