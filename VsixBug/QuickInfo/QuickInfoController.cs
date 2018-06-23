using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using VsixBug;

namespace QuickInfo.VsixBug
{
    internal sealed class QuickInfoController : IIntellisenseController
    {
        private readonly IList<ITextBuffer> _subjectBuffers;
        private readonly IAsyncQuickInfoBroker _quickInfoBroker;
        private ITextView _textView;

        internal QuickInfoController(
            ITextView textView,
            IList<ITextBuffer> subjectBuffers,
            IAsyncQuickInfoBroker quickInfoBroker)
        {
            MyTools.Output_INFO("QuickInfoController:constructor");

            this._textView = textView;
            this._subjectBuffers = subjectBuffers;
            this._quickInfoBroker = quickInfoBroker;
            
            this._textView.MouseHover += (o, e) => {
                //MyTools.Output_INFO("QuickInfoController:MouseHover");
                
                SnapshotPoint? point = GetMousePosition(new SnapshotPoint(this._textView.TextSnapshot, e.Position));
                if (point.HasValue)
                {
                    ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position, PointTrackingMode.Positive);
                    if (!this._quickInfoBroker.IsQuickInfoActive(this._textView))
                    {
                        MyTools.Output_INFO("QuickInfoController:MouseHover: position="+ point.Value.Position.ToString());
                        this._quickInfoBroker.TriggerQuickInfoAsync(this._textView, triggerPoint);
                    }
                }
            };
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            MyTools.Output_INFO("QuickInfoController:ConnectSubjectBuffer");
        }

        public void Detach(ITextView textView)
        {
            MyTools.Output_INFO("QuickInfoController:Detach");
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            MyTools.Output_INFO("QuickInfoController:DisconnectSubjectBuffer");
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
