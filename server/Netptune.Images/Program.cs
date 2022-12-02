using Netptune.Images;
using Netptune.Images.Core.Services;
using Netptune.Images.Processing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddImageProcessing(options =>
{
    options.BasePath = builder.Configuration.GetValue<string>("BasePath") ?? throw new Exception("BasePath required");
});

builder.Services.AddOutputCache(options =>
{
    options.UseCaseSensitivePaths = true;
    options.DefaultExpirationTimeSpan = TimeSpan.FromHours(1);
});

var app = builder.Build();

app.UseOutputCache();

app.MapGet("/favicon.ico", Results.NoContent);

app.MapGet("/{path}", async ([AsParameters] ImageQueryParams query, IImagePipeline pipeline) =>
{
    if (query.Path is null) return Results.BadRequest();

    var result = await pipeline.Process(query.Path, query.ToOptions());

    if (result is null) return Results.NotFound();

    return Results.File(result.Content, result.ContentType);

}).CacheOutput(b => b.SetVaryByRouteValue("path").SetVaryByQuery(ImageQueryParams.QueryKeys));

app.Run();
