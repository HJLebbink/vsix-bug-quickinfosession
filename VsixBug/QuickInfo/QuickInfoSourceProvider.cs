using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace QuickInfo.VsixBug
{
    //[ContentType(VSPackage1.MyContentType)]
    [Export(typeof(IQuickInfoSourceProvider))]
    [Name("VsixBugQuickInfoSourceProvider")]
    internal sealed class QuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer buffer)
        {
            Func<QuickInfoSource> sc = delegate () {
                return new QuickInfoSource(buffer);
            };
            return buffer.Properties.GetOrCreateSingletonProperty(sc);
        }
    }
}