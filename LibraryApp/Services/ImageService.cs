using SkiaSharp;

namespace LibraryApp.Services
{
    public static class ImageService
    {
        public static void ResizeAndCrop(string sourcePath,
            string destinationPath,int targetWidth,int targetHeight, 
            int quality = 90)
        {
            using var input = File.OpenRead(sourcePath);
            using var original = SKBitmap.Decode(input);

            float sourceAspect = (float)original.Width / original.Height;
            float targetAspect = (float)targetWidth / targetHeight;

            SKRect cropRect;

            if(sourceAspect > targetAspect)
            {
                //Crop left/right
                int newWidth = (int)(original.Height * targetAspect);
                int x = (original.Width - newWidth) / 2;
                cropRect = new SKRect(x, 0, x + newWidth, original.Height);
            }
            else
            {
                //Crop top/bottom
                int newHeight = (int)(original.Width / targetAspect);
                int y = (original.Height - newHeight) / 2;
                cropRect = new SKRect(0, y, original.Width, y + newHeight);
            }

            using var cropped = new SKBitmap((int)cropRect.Width, (int)cropRect.Height);
            using(var canvas = new SKCanvas(cropped))
            {
                canvas.DrawBitmap(original, cropRect, new SKRect(0, 0, cropped.Width, cropped.Height));
            }

            using var resized = cropped.Resize(
                new SKImageInfo(targetWidth, targetHeight),
                new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));

            using var image = SKImage.FromBitmap(resized);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, quality);

            using var output = File.OpenWrite(destinationPath);
            data.SaveTo(output);
        }
    }
}
