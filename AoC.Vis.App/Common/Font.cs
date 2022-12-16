using System.Numerics;

namespace Ujeby.AoC.Vis.App.Common
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
		public Vector4 Color = Colors.White;
	}

	public class Font
	{
		public string SpriteId;
		public string DataSpriteId;

		/// <summary></summary>
		public Vector2 CharSize;

		/// <summary></summary>
		public Vector2 Spacing;

		public AABB[] CharBoxes;
	}
}