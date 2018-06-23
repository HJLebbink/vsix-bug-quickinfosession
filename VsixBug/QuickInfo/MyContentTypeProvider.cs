using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace VsixBug.QuickInfo
{
    [ContentType(VsixBugPackage.MyContentType)]
    [Name("VsixBugContentTypeProvider")]
    internal sealed class MyContentTypeProvider
    {
        [Export]
        [Name(VsixBugPackage.MyContentType)]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition XyzzyContentType = null;

        [Export]
        [FileExtension(".xyz")]
        [ContentType(VsixBugPackage.MyContentType)]
        internal static FileExtensionToContentTypeDefinition XyzzyFileType = null;
    }
}
