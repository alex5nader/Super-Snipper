using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Function {

    public static class BitmapUtil {

        public static Bitmap Crop(this Bitmap bmp, Rectangle cropArea) {
            var newBmp = new Bitmap(bmp);
            return newBmp.Clone(cropArea, bmp.PixelFormat);
        }

        public static Bitmap TakeScreenshot(Box bounds) {
            using (var bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb)) {
                using (var g = Graphics.FromImage(bmp)) {
                    g.CopyFromScreen(bounds.TopLeft, Point.Empty, bounds.Size, CopyPixelOperation.SourceCopy);
                }

                return new Bitmap(bmp);
            }
        }

        public static BitmapImage ToBitmapImage(this Bitmap bmp) {
            using (var mem = new MemoryStream()) {
                bmp.Save(mem, ImageFormat.Bmp);
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
