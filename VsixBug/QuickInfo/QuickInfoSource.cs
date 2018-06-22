using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using VsixBug.QuickInfo;

namespace QuickInfo.VsixBug
{
    internal sealed class QuickInfoSource : IQuickInfoSource
    {
        private readonly ITextBuffer _sourceBuffer;
        public QuickInfoSource(ITextBuffer buffer)
        {
            this._sourceBuffer = buffer;
        }
        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
        {
            var snapshot = this._sourceBuffer.CurrentSnapshot;
            var triggerPoint = (SnapshotPoint)session.GetTriggerPoint(snapshot);
            applicableToSpan = snapshot.CreateTrackingSpan(new SnapshotSpan(triggerPoint, triggerPoint), SpanTrackingMode.EdgeInclusive);
            quickInfoContent.Add(new BugWindow());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
