using System.Windows.Controls;

namespace VsixBug.QuickInfo
{
    public partial class BugWindow : UserControl
    {
        public BugWindow()
        {
            InitializeComponent();
            MyTools.Output_INFO("BugWindow:constructor");

            this.MainWindow.MouseLeftButtonDown += (o, i) =>
            {
                MyTools.Output_INFO("BugWindow:MainWindow.MouseLeftButtonDown");
                //i.Handled = true; // dont let the mouse event from inside this window bubble up to VS
            };

            this.MainWindow.PreviewMouseLeftButtonDown += (o, i) =>
            {
                MyTools.Output_INFO("BugWindow:MainWindow.PreviewMouseLeftButtonDown");
                //i.Handled = true; // if true then no event is able to bubble to the gui
            };

            this.MyExpander.MouseLeftButtonUp += (o, i) =>
            {
                MyTools.Output_INFO("BugWindow:MyExpander.MouseLeftButtonUp");
                //i.Handled = true;
            };

            this.MyExpander.GotMouseCapture += (o, i) =>
            {
                MyTools.Output_INFO("BugWindow:GotMouseCapture_Click");
                i.Handled = true;
            };
        }
    }
}
