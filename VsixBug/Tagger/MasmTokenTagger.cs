// The MIT License (MIT)
//
// Copyright (c) 2018 Henk-Jan Lebbink
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace VsixBug
{
    internal sealed class MasmTokenTagger : ITagger<MyTokenTag>
    {
        private readonly ITextBuffer _buffer;

        private readonly MyTokenTag _xyzzy;
        private readonly MyTokenTag _UNKNOWN;

        internal MasmTokenTagger(ITextBuffer buffer)
        {
            this._buffer = buffer;
            this._xyzzy = new MyTokenTag(MyTokenType.Xyzzy);
            this._UNKNOWN = new MyTokenTag(MyTokenType.UNKNOWN);
            MyTools.Output_INFO("MasmTokenTagger:constructor");
        }

        event EventHandler<SnapshotSpanEventArgs> ITagger<MyTokenTag>.TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<MyTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            MyTools.Output_INFO("MasmTokenTagger:GetTags");
            if (spans.Count == 0) yield break;

            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();

                string line = containingLine.GetText().ToUpper();
            }
        }
    }
}
