using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Function.Integration;
using Function.Util;
using Imgur.API.Models;

namespace Function.Snip {

    public class Snip {

        private ICommand _saveCommand;

        public ICommand SaveCommand {
            get {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           param => Save(),
                           param => CanSave()
                       ));
            }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand PreviewCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand ImgurExportCommand => new RelayCommand(
            async param => await ImgurExport(),
            param => Screenshot != null
        );

        public BitmapImage BitmapImageScreenshot => Screenshot.ToBitmapImage();
        public Bitmap Screenshot { get; private set; }

        /// <summary>
        /// Takes a screenshot of the entire screen
        /// </summary>
        public void TakeScreenshot() {
            TakeScreenshot(new Box {
                X1 = 0,
                Y1 = 0,
                X2 = Screen.PrimaryScreen.Bounds.Width,
                Y2 = Screen.PrimaryScreen.Bounds.Height
            });
        }

        /// <summary>
        /// Takes a screenshot of a region of the screen
        /// </summary>
        /// <param name="bounds">The region to capture</param>
        public void TakeScreenshot(Box bounds) {
            Screenshot?.Dispose();
            Screenshot = ImageUtil.TakeScreenshot(bounds) as Bitmap;
        }

        public void Crop(Box bounds) {
            Screenshot = Screenshot.Crop(bounds.ToRectangle()) as Bitmap;
        }

        public void Save() {
            if (Screenshot is null)
                return;

            var dialogue = new Microsoft.Win32.SaveFileDialog {
                FileName = "Snip",
                DefaultExt = ".png",
                Filter = "PNG (*.png)|*.png|"
                         + "BMP (*.bmp)|*.bmp|"
                         + "JPEG (*.jpeg)|*.jpeg"
            };

            var result = dialogue.ShowDialog();

            if (result != true)
                return;

            var filename = dialogue.FileName;

            Screenshot.Save(filename, filename.Split('.').Last().ToImageFormat());
        }

        private bool CanSave() {
            return Screenshot != null;
        }

        public void Dispose() {
            Screenshot.Dispose();
        }

        public async Task<string> ImgurExport() {
            var image = await ImgurIntegration.UploadImage(Screenshot);
            Debug.WriteLine(image.Link);
            return image.Link;
        }
    }
}
