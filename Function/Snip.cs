using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Function {

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
            Screenshot = BitmapUtil.TakeScreenshot(bounds);
        }

        public void Crop(Box bounds) {
            Screenshot = Screenshot.Crop(bounds.ToRectangle());
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
    }
}
