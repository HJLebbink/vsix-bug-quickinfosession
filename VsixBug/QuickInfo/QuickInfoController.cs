using System.Collections.Generic;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace VsixBug.QuickInfo
{
    internal sealed class QuickInfoController : IIntellisenseController
    {
        private readonly IList<ITextBuffer> _subjectBuffers;
        private readonly IAsyncQuickInfoBroker _quickInfoBroker;
        private readonly ITextView _textView;

        internal QuickInfoController(
            ITextView textView,
            IList<ITextBuffer> subjectBuffers,
            IAsyncQuickInfoBroker quickInfoBroker)
        {
            //AsmDudeToolsStatic.Output_INFO("AsmQuickInfoController:constructor: file=" + AsmDudeToolsStatic.GetFileName(textView.TextBuffer));
            this._textView = textView;
            this._subjectBuffers = subjectBuffers;
            this._quickInfoBroker = quickInfoBroker;
            this._textView.MouseHover += this.OnTextViewMouseHover;
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


        /// <summary>
        /// Determine if the mouse is hovering over a token. If so, display QuickInfo
        /// </summary>
        private void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
        {
            //AsmDudeToolsStatic.Output_INFO("AsmQuickInfoController:OnTextViewMouseHover: file=" + AsmDudeToolsStatic.GetFileName(this._textView.TextBuffer));
            SnapshotPoint? point = this.GetMousePosition(new SnapshotPoint(this._textView.TextSnapshot, e.Position));
            if (point.HasValue)
            {
                int pos = point.Value.Position;
                ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(pos, PointTrackingMode.Positive);

                if (this._quickInfoBroker.IsQuickInfoActive(this._textView))
                {
                    IAsyncQuickInfoSession current_Session = this._quickInfoBroker.GetSession(this._textView);
                    if (current_Session.ApplicableToSpan.GetSpan(this._textView.TextSnapshot).IntersectsWith(new Span(pos, 0)))
                    {
                        MyTools.Output_INFO("QuickInfoController:OnTextViewMouseHover: A: quickInfoBroker is already active: intersects!");
                    }
                    else
                    {
                        MyTools.Output_INFO("QuickInfoController:OnTextViewMouseHover: B: quickInfoBroker is already active, but we need a new session at " + pos);
                        current_Session.DismissAsync();
                        this._quickInfoBroker.TriggerQuickInfoAsync(this._textView, triggerPoint, QuickInfoSessionOptions.None); //BUG here QuickInfoSessionOptions.None behaves as TrackMouse
                    }
                }
                else
                {
                    MyTools.Output_INFO("QuickInfoController:OnTextViewMouseHover: C: quickInfoBroker was not active, create a new session for triggerPoint " + pos);
                    this._quickInfoBroker.TriggerQuickInfoAsync(this._textView, triggerPoint, QuickInfoSessionOptions.None);
                }
            }
            else
            {
                MyTools.Output_INFO("QuickInfoController:OnTextViewMouseHover: point has not value");
            }
        }

        /// <summary>
        /// get mouse location on screen. Used to determine what word the cursor is currently hovering over
        /// </summary>
        private SnapshotPoint? GetMousePosition(SnapshotPoint topPosition)
        {
            // Map this point down to the appropriate subject buffer.

            return this._textView.BufferGraph.MapDownToFirstMatch(
                topPosition,
                PointTrackingMode.Positive,
                snapshot => this._subjectBuffers.Contains(snapshot.TextBuffer),
                PositionAffinity.Predecessor
            );
        }
    }
}
