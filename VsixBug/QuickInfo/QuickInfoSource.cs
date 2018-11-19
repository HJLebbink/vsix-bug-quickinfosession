using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using VsixBug;
using VsixBug.QuickInfo;

// for QuickInfo,  see https://github.com/Microsoft/vs-editor-api/wiki/Modern-Quick-Info-API
// for Completion, see https://github.com/Microsoft/vs-editor-api/wiki/Modern-completion-walkthrough


namespace QuickInfo.VsixBug
{
    internal sealed class QuickInfoSource : IAsyncQuickInfoSource
    {
        private readonly TaskScheduler _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private readonly ITextBuffer _textBuffer;

        public QuickInfoSource(ITextBuffer textBuffer)
        {
            this._textBuffer = textBuffer;
        }

        private QuickInfoItem RunOnUI(IAsyncQuickInfoSession session, ITrackingSpan lineSpan)
        {
            MyTools.Output_INFO("QuickInfoSource:RunOnUI");
            var elem = new ContainerElement(
                ContainerElementStyle.Wrapped,
                new BugWindow(session)
            );
            return new QuickInfoItem(lineSpan, elem);
        }

        // This is called on a background thread.
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            //MyTools.Output_INFO("QuickInfoSource:GetQuickInfoItemAsync"); logging here bricks the app
            session.StateChanged += this.Session_StateChanged;

            var triggerPoint = session.GetTriggerPoint(this._textBuffer.CurrentSnapshot);
            if (triggerPoint != null)
            {
                var line = triggerPoint.Value.GetContainingLine();
                var lineSpan = this._textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

                return Task<QuickInfoItem>.Factory.StartNew(() => this.RunOnUI(session, lineSpan), CancellationToken.None, TaskCreationOptions.None, this._uiScheduler);
            }
            return Task.FromResult<QuickInfoItem>(null);
        }

        private void Session_StateChanged(object sender, QuickInfoSessionStateChangedEventArgs e)
        {
            MyTools.Output_INFO("QuickInfoSource:Session_StateChanged: sender="+sender.ToString() + "; e="+e.ToString());
        }

        public void Dispose()
        {
            MyTools.Output_INFO("QuickInfoSource:Dispose");
        }
    }
}
