using Bookstore.Domain;
using ImageMagick;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Data.ImageResizeService;

public class ImageResizeService : IImageResizeService
{
    private const int BookCoverImageWidth = 400;
    private const int BookCoverImageHeight = 600;

    public async Task<Stream> ResizeImageAsync(Stream image)
    {
        using MagickImage magickImage = new(image);

        if (magickImage.BaseWidth == BookCoverImageWidth && magickImage.BaseHeight == BookCoverImageHeight) return image;

        MagickGeometry size = new(BookCoverImageWidth, BookCoverImageHeight) { IgnoreAspectRatio = false };

        magickImage.Resize(size);

        MemoryStream result = new();

        await magickImage.WriteAsync(result);

        result.Position = 0;

        return result;
    }
}