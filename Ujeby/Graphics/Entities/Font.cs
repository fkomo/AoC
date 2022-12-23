using Ujeby.Vectors;

namespace Ujeby.Graphics.Entities
{
    public abstract class TextLine
    {
    }

    public class EmptyLine : TextLine
    {
    }

    public class Text : TextLine
    {
        public Text(string value)
        {
            Value = value;
        }

        public string Value;
        public v4f Color = Colors.White;
    }

    public class Font
    {
        public string SpriteId;
        public string DataSpriteId;

        /// <summary></summary>
        public v2i CharSize;

        /// <summary></summary>
        public v2i Spacing;

        public AABBi[] CharBoxes;
    }

    public enum TextAlign
    {
        Left,
        Center,
        Right
    }
}