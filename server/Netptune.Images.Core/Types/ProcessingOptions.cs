namespace Netptune.Images.Core.Types;

public sealed record ProcessingOptions
{
    public int? Width { get; init; }

    public int? Height { get; init; }

    public ImageScale? Scale { get; init; }

    public int? Thumbnail { get; init; }

    public ImageFormat? Format { get; init; }

    public int? Quality { get; init; }

    public string? Text { get; init; }

    public string? TextColor { get; init; }

    public int? TextFontSize { get; init; }

    public string? TextFontFamily { get; init; }
}

public enum ImageScale
{
    Ignore = 0,
    Fill = 1,
    Noup = 2,
}

public enum ImageFormat
{
    Original = 0,
    Png = 1,
    Webp = 2,
    Jpg = 3,
}
