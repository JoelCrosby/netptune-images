namespace Netptune.Images.Core.Services;

public interface IImageRetriever
{
    Task<Stream?> Get(string path);
}
