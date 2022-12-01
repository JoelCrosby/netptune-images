using Microsoft.AspNetCore.Mvc;

using Netptune.Images.Core.Types;

namespace Netptune.Images;

public record ImageQueryParams
{
    [FromQuery(Name = "w")]
    public int? Width { get; init; }

    [FromQuery(Name = "h")]
    public int? Height { get; init; }

    [FromQuery(Name = "scale")]
    public string? Scale { get; init; }

    [FromQuery(Name = "thumbnail")]
    public int? Thumbnail { get; init; }

    [FromQuery(Name = "format")]
    public string? Format { get; init; }

    [FromQuery(Name = "q")]
    public int? Quality { get; init; }

    [FromQuery(Name = "text")]
    public string? Text { get; init; }

    [FromQuery(Name = "text.color")]
    public string? TextColor { get; init; }

    [FromQuery(Name = "text.font.size")]
    public int? TextFontSize { get; init; }

    [FromQuery(Name = "text.font.family")]
    public string? TextFontFamily { get; init; }

    public ProcessingOptions ToOptions()
    {
        Enum.TryParse<ImageScale>(Scale, true, out var scale);
        Enum.TryParse<ImageFormat>(Format, true, out var format);

        return new ProcessingOptions
        {
            Width = Width,
            Height = Height,
            Scale = scale,
            Thumbnail = Thumbnail,
            Format = format,
            Quality = Quality,
            Text = Text,
            TextColor = TextColor,
            TextFontSize = TextFontSize,
            TextFontFamily = TextFontFamily,
        };
    }

    public static readonly string[] QueryKeys =
    {
        "w",
        "h",
        "scale",
        "thumbnail",
        "format",
        "q",
        "text",
        "text.color",
        "text.font.size",
        "text.font.family",
    };
}
