using Netptune.Images.Core.Types;

namespace Netptune.Images.Core.Services;

public interface IImagePipeline
{
    Task<ProcessedImage?> Process(string uri, ProcessingOptions options);
}
