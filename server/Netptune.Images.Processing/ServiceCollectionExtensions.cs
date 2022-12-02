using Microsoft.Extensions.DependencyInjection;

using Netptune.Images.Core.Services;
using Netptune.Images.Core.Types;

namespace Netptune.Images.Processing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageProcessing(this IServiceCollection services, Action<ImageProcessingOptions> action)
    {
        services.Configure(action);

        services.AddMemoryCache();

        services.AddTransient<IImageProcessor, ImageProcessor>();
        services.AddTransient<IImageRetriever, ImageRetriever>();
        services.AddTransient<IImagePipeline, ImagePipeline>();

        return services;
    }
}
