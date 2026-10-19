namespace Ujeby.Tools
{
	public class IntArrayComparer : IEqualityComparer<int[]>
	{
		public bool Equals(int[] a, int[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (var i = 0; i < a.Length; i++)
				if (a[i] != b[i])
					return false;

			return true;
		}

		public int GetHashCode(int[] obj)
		{
			int result = 17;
			for (int i = 0; i < obj.Length; i++)
			{
				unchecked
				{
					result = result * 23 + obj[i];
				}
			}
			return result;
		}
	}
}
