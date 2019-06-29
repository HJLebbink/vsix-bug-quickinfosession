using Microsoft.VisualStudio.Language.Intellisense;
using System;

namespace VsixBug.QuickInfo
{
    public partial class BugWindow : IInteractiveQuickInfoContent
    {
        private readonly Action _closeAction;

        public BugWindow(Action closeAction)
        {
            this.InitializeComponent();
            MyTools.Output_INFO(string.Format("{0}:constructor", this.ToString()));

            this._closeAction = closeAction;

            this.MainWindow.MouseLeftButtonDown += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:MouseLeftButtonDown", this.ToString()));
                //i.Handled = true; // dont let the mouse event from inside this window bubble up to VS
            };

            this.MainWindow.PreviewMouseLeftButtonDown += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:PreviewMouseLeftButtonDown", this.ToString()));
                //i.Handled = true; // if true then no event is able to bubble to the gui
            };

            this.MyExpander.MouseLeftButtonUp += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:MouseLeftButtonUp", this.ToString()));
                //i.Handled = true;
            };

            this.MyExpander.GotMouseCapture += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:GotMouseCapture", this.ToString()));
               //i.Handled = true;
            };
        }

        public bool KeepQuickInfoOpen => this.IsMouseOverAggregated || this.IsKeyboardFocusWithin || this.IsKeyboardFocused || this.IsFocused;

        public bool IsMouseOverAggregated => this.IsMouseOver || this.IsMouseDirectlyOver;

    }
}
