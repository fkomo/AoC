using Ujeby.Vectors;

namespace Ujeby.Tools
{
	public class ReadingOrderComparer : IComparer<v2i>
	{
		public int Compare(v2i v1, v2i v2)
		{
			if (v1 == v2)
				return 0;

			if (v1.Y < v2.Y)
				return -1;

			if (v1.Y > v2.Y)
				return 1;

			if (v1.X < v2.X)
				return -1;

			if (v1.X > v2.X)
				return 1;

			return 0;
		}
	}
}
