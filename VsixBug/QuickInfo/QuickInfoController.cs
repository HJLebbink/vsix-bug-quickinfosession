using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace QuickInfo.VsixBug
{
    internal sealed class QuickInfoController : IIntellisenseController
    {
        private readonly IList<ITextBuffer> _subjectBuffers;
        private readonly IQuickInfoBroker _quickInfoBroker;
        private ITextView _textView;

        internal QuickInfoController(
            ITextView textView,
            IList<ITextBuffer> subjectBuffers,
            IQuickInfoBroker quickInfoBroker)
        {
            this._textView = textView;
            this._subjectBuffers = subjectBuffers;
            this._quickInfoBroker = quickInfoBroker;
            this._textView.MouseHover += (o, e) => {
                SnapshotPoint? point = GetMousePosition(new SnapshotPoint(this._textView.TextSnapshot, e.Position));
                if (point.HasValue)
                {
                    ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position, PointTrackingMode.Positive);
                    if (!this._quickInfoBroker.IsQuickInfoActive(this._textView))
                    {
                        this._quickInfoBroker.TriggerQuickInfo(this._textView, triggerPoint, false);
                    }
                }
            };
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            throw new NotImplementedException();
        }

        public void Detach(ITextView textView)
        {
            throw new NotImplementedException();
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            throw new NotImplementedException();
        }

        private SnapshotPoint? GetMousePosition(SnapshotPoint topPosition)
        {
            return this._textView.BufferGraph.MapDownToFirstMatch(
                topPosition,
                PointTrackingMode.Positive,
                snapshot => this._subjectBuffers.Contains(snapshot.TextBuffer),
                PositionAffinity.Predecessor
            );
        }
    }
}
