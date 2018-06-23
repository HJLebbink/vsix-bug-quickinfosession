using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using VsixBug;
using VsixBug.QuickInfo;

// for QuickInfo,  see https://github.com/Microsoft/vs-editor-api/wiki/Modern-Quick-Info-API
// for Completion, see https://github.com/Microsoft/vs-editor-api/wiki/Modern-completion-walkthrough


namespace QuickInfo.VsixBug
{
    internal sealed class QuickInfoSource : IAsyncQuickInfoSource
    {
        private ITextBuffer _textBuffer;
        public QuickInfoSource(ITextBuffer textBuffer)
        {
            this._textBuffer = textBuffer;
        }
        // This is called on a background thread.
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            //MyTools.Output_INFO("QuickInfoSource:AugmentQuickInfoSession");

            var triggerPoint = session.GetTriggerPoint(this._textBuffer.CurrentSnapshot);
            if (triggerPoint != null)
            {
                var line = triggerPoint.Value.GetContainingLine();
                var lineSpan = this._textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

               // MyTools.Output_INFO("QuickInfoSource:AugmentQuickInfoSession: triggerPoint=" + lineSpan.GetText(this._textBuffer.CurrentSnapshot));

                //return Task.FromResult(new QuickInfoItem(lineSpan, new BugWindow()));
                return Task.FromResult(new QuickInfoItem(lineSpan, "Bla"));
            }
            return Task.FromResult<QuickInfoItem>(null);
        }

        public void Dispose()
        {
            MyTools.Output_INFO("QuickInfoSource:Dispose");
        }
    }
}
