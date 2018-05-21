using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Function;
using Function.Snip;
using Function.Util;
using Clipboard = System.Windows.Clipboard;

namespace ScreenshotApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly List<PreviewWindow> _previews;

        private readonly SnipQueue _snips;

        public MainWindow() {
            InitializeComponent();

            _snips = new SnipQueue(10);
            _previews = new List<PreviewWindow>();
            IcScreenshots.ItemsSource = _snips;
            Closing += delegate {
                for (var i = _previews.Count - 1; i >= 0; --i) {
                    _previews[i].Close();
                }
            };
        }

        public void PreviewSnip(Snip toPreview) {
            if (toPreview is null)
                return;

            Clipboard.SetDataObject(toPreview.BitmapImageScreenshot);
            var previewWindow = new PreviewWindow(toPreview) {
                Owner = GetWindow(this),
                WindowClosed = window => _previews.Remove(window)
            };
            _previews.Add(previewWindow);
            previewWindow.Show();
        }

        private async void BtnNew_OnClick(object sender, RoutedEventArgs e) {
            if (IudDelay.Value is int delay) {
                await Task.Delay(1000 * delay);

                var snip = new Snip {
                    PreviewCommand = new RelayCommand(
                        param => PreviewSnip(param as Snip),
                        param => param is Snip
                    ),
                    CopyCommand = new RelayCommand(
                        param => {
                            if (param is Snip pSnip) {
                                Clipboard.SetImage(pSnip.BitmapImageScreenshot);
                            }
                        },
                        param => param is Snip
                    )
                };

                snip.TakeScreenshot();

                Hide();
                var screenshotTaker = new ScreenshotTaker(snip) {
                    Enqueue = _snips.Enqueue
                };
                screenshotTaker.ShowDialog();
                Show();
            }
        }

        private void IncrementDelay(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs) {
            if (IudDelay.Value is int delay)
                ++IudDelay.Value;
            else
                IudDelay.Value = 1; // treat a null value as 0 and just increment it to 1
        }

        private void DecrementDelay(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs) {
            if (IudDelay.Value is int delay)
                --IudDelay.Value;
            else
                IudDelay.Value = 0; // treat a null value as 0 which is the minimum delay, but go ahead and convert it to 0
        }

        private void ShowHelpWindow(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs) {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }

        private void SaveAll(object sender, ExecutedRoutedEventArgs e) {
            using (var dialog = new FolderBrowserDialog()) {
                var result = dialog.ShowDialog();

                var path = dialog.SelectedPath;

                if (result != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(path))
                    return;

                var files = Directory.GetFiles(path);

                foreach (var file in files) {
                    Console.WriteLine(file);
                }

                var number = 1;
                var warnAboutDuplicates = true;
                foreach (var snip in _snips) {
                    var savePath = path + $@"\Snip {number++}.png";
                    if (File.Exists(savePath) && warnAboutDuplicates) {

                    }
                    snip.Screenshot.Save(savePath, ImageFormat.Png);
                }
            }
        }

        private void SvSnips_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0)
                SvSnips.LineLeft();
            else
                SvSnips.LineRight();
            e.Handled = true;
            // 
        }
    }
}
