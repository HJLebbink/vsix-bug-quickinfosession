using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace VsixBug.Tagger
{
    internal static class MyClassificationDefinition
    {
        internal static class ClassificationTypeNames
        {
            public const string Xyzzy = "xyzzy-D74860FA-F0BC-4441-9D76-DF4ECB19CF71";
        }

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Xyzzy)]
        internal static ClassificationTypeDefinition zyxxy = null;
    }
}
