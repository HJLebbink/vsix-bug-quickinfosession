using System;
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace VsixBug
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(VsixBugPackage.MyContentType)]
    [TagType(typeof(MyTokenTag))]
    [Name("VsixBugMyTokenTaggerProvider")]
    [Order(Before = "default")]
    internal sealed class MyTokenTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            MyTools.Output_INFO("MyTokenTaggerProvider:CreateTagger");
            Func<ITagger<T>> sc = delegate ()
            {
                return new MasmTokenTagger(buffer) as ITagger<T>;
            };
            return buffer.Properties.GetOrCreateSingletonProperty(sc);
        }
    }
}
