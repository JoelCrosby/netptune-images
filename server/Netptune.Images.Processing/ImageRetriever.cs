using Flurl;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using Netptune.Images.Core.Services;
using Netptune.Images.Core.Types;

namespace Netptune.Images.Processing;

public class ImageRetriever : IImageRetriever
{
    private readonly HttpClient HttpClient;
    private readonly IMemoryCache Cache;
    private readonly IOptions<ImageProcessingOptions> Options;

    public ImageRetriever(HttpClient httpClient, IMemoryCache cache, IOptions<ImageProcessingOptions> options)
    {
        HttpClient = httpClient;
        Cache = cache;
        Options = options;
    }

    public async Task<Stream?> Get(string path)
    {
        var result = await Cache.GetOrCreateAsync(path, async _ =>
        {
            var request = Options.Value.BasePath.AppendPathSegment(path);
            var response = await HttpClient.GetByteArrayAsync(request);

            return new MemoryStream(response);
        });

        return result;
    }
}
