using Netptune.Images.Core.Services;
using Netptune.Images.Core.Types;

namespace Netptune.Images.Processing;

public sealed class ImagePipeline : IImagePipeline
{
    private readonly IImageRetriever Retriever;
    private readonly IImageProcessor Processor;

    public ImagePipeline(IImageRetriever retriever, IImageProcessor processor)
    {
        Retriever = retriever;
        Processor = processor;
    }

    public async Task<ProcessedImage?> Process(string uri, ProcessingOptions options)
    {
        var response = await Retriever.Get(uri);

        if (response is null) return null;

        var result = Processor.ProcessStream(response, options);

        return result;
    }
}
