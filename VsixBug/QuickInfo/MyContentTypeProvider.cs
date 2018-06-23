using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace VsixBug.QuickInfo
{
    [ContentType(VsixBugPackage.MyContentType)]
    [Name("VsixBugContentTypeProvider")]
    internal sealed class MyContentTypeProvider
    {
        [Export]
        [Name("xyzzy!")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition XyzzyContentType = null;

        [Export]
        [FileExtension(".xyz")]
        [ContentType(VsixBugPackage.MyContentType)]
        internal static FileExtensionToContentTypeDefinition XyzzyFileType = null;
    }
}
