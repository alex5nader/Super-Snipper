using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Function.Snip;
using Clipboard = System.Windows.Clipboard;

namespace SuperSnipper {
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
            var newWindow =
                new PreviewWindow(_toPreview, newLeft, newTop, !_showingDecoration) {WindowClosed = WindowClosed};
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

        private void SaveSnip(object sender, ExecutedRoutedEventArgs e) {
            _toPreview.Save();
        }

        private void CopySnip(object sender, ExecutedRoutedEventArgs e) {
            Clipboard.SetDataObject(_toPreview.BitmapImageScreenshot);
        }
    }
}
