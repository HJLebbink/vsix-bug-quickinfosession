using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using VsixBug;

namespace QuickInfo.VsixBug
{
    [ContentType(VsixBugPackage.MyContentType)]
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("VsixBugQuickInfoControllerProvider")]
    internal sealed class QuickInfoControllerProvider : IIntellisenseControllerProvider
    {
        [Import]
        private IQuickInfoBroker _quickInfoBroker = null;

        public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            MyTools.Output_INFO("QuickInfoControllerProvider:TryCreateIntellisenseController");
            return new QuickInfoController(textView, subjectBuffers, this._quickInfoBroker);
        }
    }
}
