using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using VsixBug;

namespace QuickInfo.VsixBug
{
    [ContentType(VsixBugPackage.MyContentType)]
    //[Export(typeof(IAsyncQuickInfoSourceProvider))] //XYZZY NEW
    [Export(typeof(IQuickInfoSourceProvider))] //XYZZY OLD
    [Name("VsixBugQuickInfoSourceProvider")]
    [Order]
    //internal sealed class QuickInfoSourceProvider : IAsyncQuickInfoSourceProvider //XYZZY NEW
    internal sealed class QuickInfoSourceProvider : IQuickInfoSourceProvider //XYZZY OLD
    {
        //public IAsyncQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer) //XYZZY NEW
        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer) //XYZZY OLD
        {
            MyTools.Output_INFO(string.Format("{0}:TryCreateQuickInfoSource", this.ToString()));
            return textBuffer.Properties.GetOrCreateSingletonProperty(() => new QuickInfoSource(textBuffer));
        }
    }
}