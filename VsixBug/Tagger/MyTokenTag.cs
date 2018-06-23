using Microsoft.VisualStudio.Text.Tagging;

namespace VsixBug
{
    public class MyTokenTag : ITag
    {
        public MyTokenType Type { get; private set; }
        public MyTokenTag(MyTokenType type)
        {
            this.Type = type;
        }
    }
}
