using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Function.Util {

    public static class ImageUtil {

        public static Image Crop(this Image img, Rectangle cropArea) {
            var bmp = new Bitmap(img);
            return bmp.Clone(cropArea, img.PixelFormat);
        }

        public static Image TakeScreenshot(Box bounds) {
            using (var bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb)) {
                using (var g = Graphics.FromImage(bmp)) {
                    g.CopyFromScreen(bounds.TopLeft, Point.Empty, bounds.Size, CopyPixelOperation.SourceCopy);
                }

                return new Bitmap(bmp);
            }
        }

        public static BitmapImage ToBitmapImage(this Image img) {
            using (var mem = new MemoryStream()) {
                img.Save(mem, ImageFormat.Bmp);
                mem.Position = 0;

                var output = new BitmapImage();
                output.BeginInit();
                output.StreamSource = mem;
                output.CacheOption = BitmapCacheOption.OnLoad;
                output.EndInit();
                output.Freeze();

                return output;
            }
        }

        public static byte[] ToByteArray(this Image img) {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof(byte[]));
        }

        public static ImageFormat ToImageFormat(this string str) {
            switch (str) {
            case "bmp":
                return ImageFormat.Bmp;
            case "png":
                return ImageFormat.Png;
            case "jpeg":
                return ImageFormat.Jpeg;
            default:
                return null;
            }
        }
    }
}
