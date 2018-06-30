using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
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

        private QuickInfoItem RunOnUI(ITrackingSpan lineSpan)
        {
            MyTools.Output_INFO("QuickInfoSource:RunOnUI");
            var elem = new ContainerElement(
                ContainerElementStyle.Wrapped,
                new Expander
                {
                    IsExpanded = false,
                    Content = "bla"
                }
            );
            return new QuickInfoItem(lineSpan, elem);
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

                return Task<QuickInfoItem>.Factory.StartNew(() => RunOnUI(lineSpan), CancellationToken.None, TaskCreationOptions.None, this._uiScheduler);
            }
            return Task.FromResult<QuickInfoItem>(null);
        }

        public void Dispose()
        {
            MyTools.Output_INFO("QuickInfoSource:Dispose");
        }
    }
}
