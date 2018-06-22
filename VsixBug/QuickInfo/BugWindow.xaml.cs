using System.Windows;
using System.Windows.Controls;

namespace VsixBug.QuickInfo
{
    /// <summary>
    /// Interaction logic for BugWindow.xaml
    /// </summary>
    public partial class BugWindow : UserControl
    {
        public BugWindow()
        {
            InitializeComponent();

            this.MainWindow.MouseLeftButtonDown += (o, i) =>
            {
                //i.Handled = true; // dont let the mouse event from inside this window bubble up to VS
            };

            this.MainWindow.PreviewMouseLeftButtonDown += (o, i) =>
            {
                //i.Handled = true; // if true then no event is able to bubble to the gui
            };
        }

        private void GotMouseCapture_Click(object sender, RoutedEventArgs e)
        {
            //e.Handled = true;
        }
    }
}
