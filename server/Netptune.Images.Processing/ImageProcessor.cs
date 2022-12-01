using System.Drawing;

using Netptune.Images.Core.Types;

using NetVips;

namespace Netptune.Images.Processing;

public class ImageProcessor
{
    public static (Stream?, string?) ProcessStream(Stream stream, ProcessingOptions? options)
    {
        options ??= new ProcessingOptions();

        try
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var image = Image.NewFromStream(stream);

            var width = options.Width ?? image.Width;
            var height = options.Height ?? image.Height;

            Enums.Size? size = options.Scale switch
            {
                ImageScale.Ignore => null,
                ImageScale.Fill => Enums.Size.Force,
                ImageScale.Noup => Enums.Size.Down,
                _ => null,
            };

            if (options.Thumbnail.HasValue)
            {
                image = image.ThumbnailImage(options.Thumbnail.Value, size: size);
            }
            else
            {
                image = image.ThumbnailImage(width, height, size ?? Enums.Size.Force);
            }


            if (!string.IsNullOrWhiteSpace(options.Text))
            {
                var hexColor = options.TextColor ?? "000000";
                var colorType = ColorTranslator.FromHtml($"#{hexColor}");
                var rgbColor = new int[] {
                    Convert.ToInt16(colorType.R),
                    Convert.ToInt16(colorType.G),
                    Convert.ToInt16(colorType.B),
                };

                var font = $"{options.TextFontFamily ?? "sans"} {options.TextFontSize?.ToString() ?? String.Empty}";

                using var text = Image.Text(options.Text, dpi: 300, font: font);
                using var textLayer = text.Gravity(Enums.CompassDirection.Centre, image.Width, image.Height);
                using var color = textLayer.NewFromImage(rgbColor).Copy(interpretation: Enums.Interpretation.Srgb);
                using var overlay = color.Bandjoin(textLayer);

                image = image.Composite(overlay, Enums.BlendMode.Over);
            }


            var result = new MemoryStream();
            string? contentType;

            switch (options.Format)
            {
                case ImageFormat.Original:
                    image.WebpsaveStream(result, q: options.Quality);
                    contentType = "image/webp";
                    break;
                case ImageFormat.Png:
                    image.PngsaveStream(result, q: options.Quality);
                    contentType = "image/png";
                    break;
                case ImageFormat.Webp:
                    image.WebpsaveStream(result, q: options.Quality);
                    contentType = "image/webp";
                    break;
                case ImageFormat.Jpg:
                    image.JpegsaveStream(result, q: options.Quality);
                    contentType = "image/jpeg";
                    break;
                default:
                    image.WebpsaveStream(result, q: options.Quality);
                    contentType = "image/webp";
                    break;
            }

            image.Dispose();

            result.Seek(0, SeekOrigin.Begin);

            return (result, contentType);
        }
        catch
        {
            return (null, null);
        }
    }
}
