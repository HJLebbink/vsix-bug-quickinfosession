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
        private readonly ITextBuffer _sourceBuffer;
        public QuickInfoSource(ITextBuffer buffer)
        {
            this._sourceBuffer = buffer;
        }
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            var snapshot = this._sourceBuffer.CurrentSnapshot;
            var triggerPoint = (SnapshotPoint)session.GetTriggerPoint(snapshot);
            var applicableToSpan = snapshot.CreateTrackingSpan(new SnapshotSpan(triggerPoint, triggerPoint), SpanTrackingMode.EdgeInclusive);

            MyTools.Output_INFO("QuickInfoSource:AugmentQuickInfoSession: triggerPoint=" + triggerPoint.Position);

            return Task.FromResult<QuickInfoItem>(new QuickInfoItem(applicableToSpan, new BugWindow()));
        }

        public void Dispose()
        {
            MyTools.Output_INFO("QuickInfoSource:Dispose");
        }
    }
}
