using Microsoft.VisualStudio.Language.Intellisense;
using System.Windows.Controls;

namespace VsixBug.QuickInfo
{
    public partial class BugWindow : UserControl
    {
        private readonly IAsyncQuickInfoSession _session;

        public BugWindow(IAsyncQuickInfoSession session)
        {
            this.InitializeComponent();
            MyTools.Output_INFO("BugWindow:constructor");

            this._session = session;

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

                IAsyncQuickInfoSession x = this._session;
                //i.Handled = true;
            };
        }
    }
}
