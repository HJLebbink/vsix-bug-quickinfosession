using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;


namespace VsixBug
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(VsixBugPackage.MyContentType)]
    [TagType(typeof(ClassificationTag))]
    [Name("VsixBugTaggerProvider")]
    internal sealed class MyTaggerProvider : ITaggerProvider
    {
        [Export]
        [Name("xyzzy!")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition XyzzyContentType = null;

        [Export]
        [FileExtension(".xyz")]
        [ContentType(VsixBugPackage.MyContentType)]
        internal static FileExtensionToContentTypeDefinition XyzzyFileType = null;

        [Import]
        private readonly IClassificationTypeRegistryService _classificationTypeRegistry = null;

        [Import]
        private readonly IBufferTagAggregatorFactoryService _aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            MyTools.Output_INFO("MyTaggerProvider:CreateTagger");
            Func<ITagger<T>> sc = delegate ()
            {
                return new MyTagger(buffer, this._aggregatorFactory.CreateTagAggregator<MyTokenTag>(buffer), this._classificationTypeRegistry) as ITagger<T>;
            };
            return buffer.Properties.GetOrCreateSingletonProperty(sc);
        }
    }
}
