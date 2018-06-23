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
                MyTools.Output_INFO("BugWindow:MouseLeftButtonDown");
                //i.Handled = true; // dont let the mouse event from inside this window bubble up to VS
            };

            this.MainWindow.PreviewMouseLeftButtonDown += (o, i) =>
            {
                MyTools.Output_INFO("BugWindow:PreviewMouseLeftButtonDown");
                //i.Handled = true; // if true then no event is able to bubble to the gui
            };
        }

        private void GotMouseCapture_Click(object sender, RoutedEventArgs e)
        {
            MyTools.Output_INFO("BugWindow:GotMouseCapture_Click");
            //e.Handled = true;
        }
    }
}
