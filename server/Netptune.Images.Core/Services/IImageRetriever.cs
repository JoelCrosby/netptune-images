namespace Netptune.Images.Core.Services;

public interface IImageRetriever
{
    ValueTask<Stream?> Get(string path);
}
