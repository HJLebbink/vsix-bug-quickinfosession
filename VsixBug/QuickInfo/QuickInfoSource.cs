﻿using System;
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

        private QuickInfoItem RunOnUI(ITrackingSpan applicableToSpan)
        {
            MyTools.Output_INFO(string.Format("{0}:RunOnUI (IAsyncQuickInfoSession)", this.ToString()));
            return new QuickInfoItem(applicableToSpan, new BugWindow());
        }

        // This is called on a background thread.
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken _) //XYZZY NEW
        {
            MyTools.Output_INFO(string.Format("{0}:GetQuickInfoItemAsync", this.ToString()));

            var triggerPoint = session.GetTriggerPoint(this._textBuffer.CurrentSnapshot);

            if (triggerPoint != null)
            {
                var line = triggerPoint.Value.GetContainingLine();
                var applicableToSpan = this._textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

                return Task<QuickInfoItem>.Factory.StartNew(() => this.RunOnUI(applicableToSpan), CancellationToken.None, TaskCreationOptions.None, this._uiScheduler);
            }
            return Task.FromResult<QuickInfoItem>(null);
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan) //XYZZY OLD
        {
            MyTools.Output_INFO(string.Format("{0}:AugmentQuickInfoSession", this.ToString()));
            applicableToSpan = null;
            var triggerPoint = session.GetTriggerPoint(this._textBuffer.CurrentSnapshot);

            session.Recalculated += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:Session_Recalculated", this.ToString()));
            };
            session.PresenterChanged += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:Session_PresenterChanged", this.ToString()));
            };
            session.ApplicableToSpanChanged += (o, i) =>
            {
                MyTools.Output_INFO(string.Format("{0}:Session_ApplicableToSpanChanged", this.ToString()));
            };

            if (triggerPoint != null)
            {
                var line = triggerPoint.Value.GetContainingLine();
                applicableToSpan = this._textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);
                quickInfoContent.Add(new BugWindow());
            } 
        }

        public void Dispose()
        {
            MyTools.Output_INFO(string.Format("{0}:Dispose", this.ToString()));
        }
    }
}
