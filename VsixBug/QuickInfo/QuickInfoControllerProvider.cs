﻿using System.Collections.Generic;
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace VsixBug.QuickInfo
{
    [Export(typeof(IIntellisenseControllerProvider))]
    [ContentType(VsixBugPackage.MyContentType)]
    [Name("QuickInfoControllerProvider")]
    internal sealed class QuickInfoControllerProvider : IIntellisenseControllerProvider
    {
        [Import]
        private readonly IAsyncQuickInfoBroker _quickInfoBroker = null; //XYZZY BEW
        //private readonly IQuickInfoBroker _quickInfoBroker = null;  //XYZZY OLD

        public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            MyTools.Output_INFO(string.Format("{0}:TryCreateIntellisenseController", this.ToString()));
            return new QuickInfoController(textView, subjectBuffers, this._quickInfoBroker);
        }
    }
}
