using Netptune.Images.Core.Types;

namespace Netptune.Images.Core.Services;

public interface IImageProcessor
{
    ProcessedImage? ProcessStream(Stream stream, ProcessingOptions? options);
}
