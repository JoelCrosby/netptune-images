using Netptune.Images.Core.Types;

using NetVips;

namespace Netptune.Images.Processing;

public class ImageProcessor
{
    public static Stream? ProcessStream(Stream stream, ProcessingOptions options)
    {
        try
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }


            var image = Image.ThumbnailStream(stream, 200, null, 200);

            var result = new MemoryStream();

            image.WebpsaveStream(result);

            result.Seek(0, SeekOrigin.Begin);

            return result;
        }
        catch
        {
            return null;
        }
    }
}
