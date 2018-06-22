using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace VsixBug.Tagger
{
    [Export(typeof(EditorFormatDefinition))] // export as EditorFormatDefinition otherwise the syntax coloring does not work
    [ClassificationType(ClassificationTypeNames = MyClassificationDefinition.ClassificationTypeNames.Xyzzy)]
    [Name(MyClassificationDefinition.ClassificationTypeNames.Xyzzy)]
    [UserVisible(true)] // sets this editor format definition visible for the user (in Tools>Options>Environment>Fonts and Colors>Text Editor
    [Order(After = Priority.High)] //set the priority to be after the default classifiers
    internal sealed class XyzzyWord : ClassificationFormatDefinition
    {
        public XyzzyWord()
        {
            this.DisplayName = "VsixBug - XyzzyWord"; //human readable version of the name found in Tools>Options>Environment>Fonts and Colors>Text Editor

            var drawingColor = System.Drawing.Color.Red;
            this.ForegroundColor = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
        }
    }
}
