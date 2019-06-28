using Microsoft.VisualStudio.Language.Intellisense;

namespace VsixBug.QuickInfo
{
    public partial class BugWindow : IInteractiveQuickInfoContent
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

                foreach (var content2 in this._session.Content)
                {
                    MyTools.Output_INFO("BugWindow:GotMouseCapture_Click: "+ content2.GetType());

                    var io = content2 as IInteractiveQuickInfoContent;
                    if (io == null)
                    {
                        MyTools.Output_INFO("BugWindow:not interactive content");
                        continue;
                    }
                    else
                    {
                        MyTools.Output_INFO("BugWindow:found interactive content");

                    }
                    if (io.KeepQuickInfoOpen || io.IsMouseOverAggregated)
                    {
                        MyTools.Output_INFO("BugWindow:found interactive content and it needs to be kept open");
                        return;
                    }
                }
                //i.Handled = true;
            };
        }

        public bool KeepQuickInfoOpen => this.IsMouseOverAggregated || this.IsKeyboardFocusWithin || this.IsKeyboardFocused || this.IsFocused;

        public bool IsMouseOverAggregated => this.IsMouseOver || this.IsMouseDirectlyOver;

    }
}
