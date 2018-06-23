using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace VsixBug
{
    internal static class MyClassificationDefinition
    {
        internal static class ClassificationTypeNames
        {
            public const string Xyzzy = "xyzzy134-84d2-44b4-a750-8a4a674aa12f";
        }

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Xyzzy)]
        internal static ClassificationTypeDefinition zyxxy = null;
    }
}
