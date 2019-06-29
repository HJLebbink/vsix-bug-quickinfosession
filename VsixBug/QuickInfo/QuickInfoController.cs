using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace VsixBug.QuickInfo
{
    internal sealed class QuickInfoController : IIntellisenseController
    {
        private readonly IList<ITextBuffer> _subjectBuffers;

        //private readonly IAsyncQuickInfoBroker _quickInfoBroker; //XYZZY NEW
        private readonly IQuickInfoBroker _quickInfoBroker; //XYZZY OLD
        private ITextView _textView;

        internal QuickInfoController(
            ITextView textView,
            IList<ITextBuffer> subjectBuffers,
            //IAsyncQuickInfoBroker quickInfoBroker) //XYZZY NEW
            IQuickInfoBroker quickInfoBroker) //XYZZY OLD
        {
            this._textView = textView ?? throw new ArgumentNullException(nameof(textView));
            this._subjectBuffers = subjectBuffers ?? throw new ArgumentNullException(nameof(subjectBuffers));
            this._quickInfoBroker = quickInfoBroker ?? throw new ArgumentNullException(nameof(quickInfoBroker));
            this._textView.MouseHover += this.OnTextViewMouseHover;
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            MyTools.Output_INFO(string.Format("{0}:ConnectSubjectBuffer", this.ToString()));
        }

        public void Detach(ITextView textView)
        {
            MyTools.Output_INFO(string.Format("{0}:Detach", this.ToString()));
            if (this._textView == textView)
            {
                this._textView.MouseHover -= this.OnTextViewMouseHover;
                this._textView = null;
            }
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            MyTools.Output_INFO(string.Format("{0}:DisconnectSubjectBuffer", this.ToString()));
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
                    //IAsyncQuickInfoSession current_Session = this._quickInfoBroker.GetSession(this._textView); //XYZZY NEW
                    IQuickInfoSession current_Session = this._quickInfoBroker.GetSessions(this._textView)[0]; //XYZZY OLD

                    var span = current_Session.ApplicableToSpan;
                    if ((span!=null) && span.GetSpan(this._textView.TextSnapshot).IntersectsWith(new Span(pos, 0)))
                    {
                        MyTools.Output_INFO(string.Format("{0}::OnTextViewMouseHover: A: quickInfoBroker is already active: intersects!", this.ToString()));
                    }
                    else
                    {
                        MyTools.Output_INFO("QuickInfoController:OnTextViewMouseHover: B: quickInfoBroker is already active, but we need a new session at " + pos);
                        //_ = current_Session.DismissAsync(); //XYZZY NEW
                        //_ = this._quickInfoBroker.TriggerQuickInfoAsync(this._textView, triggerPoint, QuickInfoSessionOptions.None); //BUG here QuickInfoSessionOptions.None behaves as TrackMouse  //XYZZY NEW
                        current_Session.Dismiss(); //XYZZY OLD
                        this._quickInfoBroker.TriggerQuickInfo(this._textView, triggerPoint, false);  //XYZZY OLD
                    }
                }
                else
                {
                    MyTools.Output_INFO(string.Format("{0}::OnTextViewMouseHover: C: quickInfoBroker was not active, create a new session for triggerPoint {1}", this.ToString(), pos));
                    //_ = this._quickInfoBroker.TriggerQuickInfoAsync(this._textView, triggerPoint, QuickInfoSessionOptions.None); //XYZZY NEW
                    this._quickInfoBroker.TriggerQuickInfo(this._textView, triggerPoint, false);  //XYZZY OLD
                }
            }
            else
            {
                MyTools.Output_INFO(string.Format("{0}:OnTextViewMouseHover: point has not value", this.ToString()));
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
