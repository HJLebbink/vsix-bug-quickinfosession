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

        //http://glyphlist.azurewebsites.net/knownmonikers/
        private static readonly ImageId _icon = KnownMonikers.AbstractCube.ToImageId();

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
                var lineNumber = triggerPoint.Value.GetContainingLine().LineNumber;
                var lineSpan = this._textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

               // MyTools.Output_INFO("QuickInfoSource:AugmentQuickInfoSession: triggerPoint=" + lineSpan.GetText(this._textBuffer.CurrentSnapshot));

                return Testc(lineSpan, lineNumber);
            }
            return Task.FromResult<QuickInfoItem>(null);
        }

        private QuickInfoItem RunOnUI(ITrackingSpan lineSpan)
        {
            MyTools.Output_INFO("QuickInfoSource:DoSomethingWithUIAsync B");
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

        private Task<QuickInfoItem> Testc(ITrackingSpan lineSpan, int lineNumber)
        {
            if (true)
            {
                return Task<QuickInfoItem>.Factory.StartNew(() => RunOnUI(lineSpan), CancellationToken.None, TaskCreationOptions.None, this._uiScheduler);
            }
            else
            {
                var lineNumberElm = new ContainerElement(
                    ContainerElementStyle.Wrapped,
                    new ImageElement(_icon),
                    new ClassifiedTextElement(
                        new ClassifiedTextRun(PredefinedClassificationTypeNames.Keyword, "Line number: "),
                        new ClassifiedTextRun(PredefinedClassificationTypeNames.Identifier, $"{lineNumber + 1}")
                    ));

                var dateElm = new ContainerElement(
                    ContainerElementStyle.Stacked,
                    lineNumberElm,
                    new ClassifiedTextElement(
                        new ClassifiedTextRun(PredefinedClassificationTypeNames.SymbolDefinition, "The current date: "),
                        new ClassifiedTextRun(PredefinedClassificationTypeNames.Comment, DateTime.Now.ToShortDateString())
                    ));
                return Task.FromResult<QuickInfoItem>(new QuickInfoItem(lineSpan, dateElm));
            }
        }


        public void Dispose()
        {
            MyTools.Output_INFO("QuickInfoSource:Dispose");
        }
    }
}
