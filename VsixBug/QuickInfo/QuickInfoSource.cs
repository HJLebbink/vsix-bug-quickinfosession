using System;
using System.Collections.Generic;
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
    internal sealed class QuickInfoSource : IAsyncQuickInfoSource //XYZZY NEW
    //internal sealed class QuickInfoSource : IQuickInfoSource //XYZZY OLD
    {
        private readonly TaskScheduler _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private readonly ITextBuffer _textBuffer;

        public QuickInfoSource(ITextBuffer textBuffer)
        {
            this._textBuffer = textBuffer ?? throw new ArgumentNullException(nameof(textBuffer));
        }

        private QuickInfoItem RunOnUI(IAsyncQuickInfoSession session, ITrackingSpan lineSpan)
        {
            MyTools.Output_INFO(string.Format("{0}:RunOnUI (IAsyncQuickInfoSession)", this.ToString()));
            return new QuickInfoItem(lineSpan, new BugWindow(() => { try { _ = session.DismissAsync(); } catch { } }));
        }

        private QuickInfoItem RunOnUI(IQuickInfoSession session, ITrackingSpan lineSpan)
        {
            MyTools.Output_INFO(string.Format("{0}:RunOnUI (IQuickInfoSession)", this.ToString()));
            return new QuickInfoItem(lineSpan, new BugWindow(() => { try { session.Dismiss(); } catch { } }));
        }

        // This is called on a background thread.
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken _) //XYZZY NEW
        {
            //MyTools.Output_INFO(string.Format("{0}:GetQuickInfoItemAsync", this.ToString())); logging here bricks the app
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

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan) //XYZZY OLD
        {
            applicableToSpan = null;
            var triggerPoint = session.GetTriggerPoint(this._textBuffer.CurrentSnapshot);
            if (triggerPoint != null)
            {
                var line = triggerPoint.Value.GetContainingLine();
                applicableToSpan = this._textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

                this.RunOnUI(session, applicableToSpan);
            } 
        }

        private void Session_StateChanged(object sender, QuickInfoSessionStateChangedEventArgs e)
        {
            MyTools.Output_INFO(string.Format("{0}:Session_StateChanged; sender={1}; e={2}", this.ToString(), sender.ToString(), e.ToString()));
        }

        public void Dispose()
        {
            MyTools.Output_INFO(string.Format("{0}:Dispose", this.ToString()));
        }
    }
}
