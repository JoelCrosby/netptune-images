using System.ComponentModel.DataAnnotations;

using Flurl;

using Netptune.Images;
using Netptune.Images.Processing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddOutputCache(options =>
{
    options.UseCaseSensitivePaths = true;
    options.DefaultExpirationTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

const string basePath = "https://netptune-cloud.s3.eu-west-2.amazonaws.com/production";


app.UseOutputCache();

app.MapGet("/favicon.ico", Results.NoContent);

app.MapGet("/{path}", async ([Required] string? path, [AsParameters] ImageQueryParams query, IHttpClientFactory clientFactory) =>
{
    var source = basePath.AppendPathSegment(path);
    using var client = clientFactory.CreateClient();

    var response = await client.GetStreamAsync(source);
    var (processed, contentType) = ImageProcessor.ProcessStream(response, query.ToOptions());

    if (processed is null) return Results.NotFound();

    return Results.File(processed, contentType);

}).CacheOutput(b => b.SetVaryByRouteValue("path").SetVaryByQuery(ImageQueryParams.QueryKeys));

app.Run();
