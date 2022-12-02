namespace Netptune.Images.Core.Types;

public sealed record ProcessedImage
{
    public required Stream Content { get; init; }

    public required string ContentType { get; init; }
}
