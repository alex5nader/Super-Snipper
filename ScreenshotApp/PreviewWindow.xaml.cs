using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Function;
using Function.Snip;
using Console = System.Console;

namespace ScreenshotApp {
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window {

        private readonly bool _showingDecoration;
        private readonly Snip _toPreview;
        
        private static readonly double TitleBarHeight = SystemInformation.CaptionHeight;
        private const double LeftBorderWidth = 1;

        public Action<PreviewWindow> WindowClosed;

        public PreviewWindow(Snip toPreview) {
            InitializeComponent();

            Title = "Previewing Snip";

            _toPreview = toPreview;
            _showingDecoration = true;

            MouseDown += DragToMove;
            MouseDoubleClick += ToggleDecoration;
            Preview.Source = toPreview.BitmapImageScreenshot;
            Closing += OnClosing;
        }

        private PreviewWindow(Snip toPreview, double x, double y, bool shouldShowDecoration) : this(toPreview) {
            if (shouldShowDecoration) {
                WindowStyle = WindowStyle.SingleBorderWindow;
                AllowsTransparency = false;
            } else {
                WindowStyle = WindowStyle.None;
                AllowsTransparency = true;
            }

            _showingDecoration = shouldShowDecoration;

            Left = x;
            Top = y;
        }

        private void ToggleDecoration(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var newLeft = _showingDecoration
                ? Left - LeftBorderWidth
                : Left + LeftBorderWidth;
            var newTop = _showingDecoration
                ? Top + TitleBarHeight
                : Top - TitleBarHeight;
            var newWindow = new PreviewWindow(_toPreview, newLeft, newTop, !_showingDecoration);
            newWindow.Show();
            Close();
        }

        private void OnClosing(object sender, EventArgs e) {
            WindowClosed(this);
        }

        private void DragToMove(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
