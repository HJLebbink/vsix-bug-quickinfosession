using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using VsixBug;

namespace QuickInfo.VsixBug
{
    [ContentType(VsixBugPackage.MyContentType)]
    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [Name("VsixBugQuickInfoSourceProvider")]
    [Order(After = "default")]
    internal sealed class QuickInfoSourceProvider : IAsyncQuickInfoSourceProvider
    {
        public IAsyncQuickInfoSource TryCreateQuickInfoSource(ITextBuffer buffer)
        {
            MyTools.Output_INFO("QuickInfoSourceProvider:TryCreateQuickInfoSource");
            Func<QuickInfoSource> sc = delegate () {
                return new QuickInfoSource(buffer);
            };
            return buffer.Properties.GetOrCreateSingletonProperty(sc);
        }
    }
}