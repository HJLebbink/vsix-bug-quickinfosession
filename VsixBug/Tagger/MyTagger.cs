using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;

namespace VsixBug
{
    internal sealed class MyTagger : ITagger<ClassificationTag>
    {
        private readonly ITextBuffer _buffer;
        private readonly ITagAggregator<MyTokenTag> _aggregator;
        private readonly IClassificationTypeRegistryService _typeService;

        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal MyTagger(ITextBuffer buffer, 
            ITagAggregator<MyTokenTag> myTagAggregator, 
            IClassificationTypeRegistryService typeService)
        {
            this._buffer = buffer;
            this._aggregator = myTagAggregator;
            this._typeService = typeService;
        }

        event EventHandler<SnapshotSpanEventArgs> ITagger<ClassificationTag>.TagsChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Search the given span for any instances of classified tags
        /// </summary>
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            {
                if (spans.Count == 0) yield break;

                foreach (IMappingTagSpan<MyTokenTag> tagSpan in this._aggregator.GetTags(spans))
                {
                    NormalizedSnapshotSpanCollection tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                    switch (tagSpan.Tag.Type)
                    {
                        case MyTokenType.Xyzzy: yield return new TagSpan<ClassificationTag>(tagSpans[0], new ClassificationTag(this._typeService.GetClassificationType(MyClassificationDefinition.ClassificationTypeNames.Xyzzy))); break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
