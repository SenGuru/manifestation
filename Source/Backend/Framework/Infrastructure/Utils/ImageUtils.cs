using System;
using System.IO;
using SkiaSharp;

namespace Framework.Infrastructure.Utils
{
    public static class ImageUtils
    {
        public static SKBitmap GetImage(this MemoryStream stream)
        {
            if (StreamUtils.IsStreamEmpty(stream))
                return null;

            stream.Position = 0;
            var skBitmap = SKBitmap.Decode(stream);
            return skBitmap;
        }

        public static string GetImageType(this MemoryStream stream)
        {
            if (StreamUtils.IsStreamEmpty(stream))
                return "";

            stream.Position = 0;
            using (var skData = SKData.CreateCopy(stream.ToArray()))
            {
                var codec = SKCodec.Create(skData);
                if (codec == null)
                {
                    return "";
                }

                switch (codec.EncodedFormat)
                {
                    case SKEncodedImageFormat.Jpeg:
                        return "jpeg";
                    case SKEncodedImageFormat.Png:
                        return "png";
                    case SKEncodedImageFormat.Gif:
                        return "gif";
                    case SKEncodedImageFormat.Bmp:
                        return "bmp";
                    case SKEncodedImageFormat.Webp:
                        return "webp";
                    case SKEncodedImageFormat.Heif:
                        return "heif";
                    case SKEncodedImageFormat.Avif:
                        return "avif";
                    case SKEncodedImageFormat.Wbmp:
                        return "wbmp";
                    case SKEncodedImageFormat.Pkm:
                        return "pkm";
                    case SKEncodedImageFormat.Ktx:
                        return "ktx";
                    case SKEncodedImageFormat.Astc:
                        return "astc";
                    case SKEncodedImageFormat.Dng:
                        return "dng";
                    default:
                        return "";
                }
            }
        }

        public static string GetBase64(this string imagePath)
        {
            if (!File.Exists(imagePath))
                    return "";

            byte[] imageArray = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageArray);
        }

        public static string GetBase64(this MemoryStream stream)
        {
            if (StreamUtils.IsStreamEmpty(stream))
                return "";

            byte[] imageBytes = stream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }
}
