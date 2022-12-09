using System.Diagnostics.CodeAnalysis;

namespace Ujeby.AoC.App.Year2022.Day09
{
	internal class Int2EqComparer : IEqualityComparer<int[]>
	{
		public bool Equals(int[] x, int[] y)
		{
			if (x.Length != y.Length)
				return false;

			for (var i = 0; i < x.Length; i++)
				if (x[i] != y[i])
					return false;

			return true;
		}

		public int GetHashCode([DisallowNull] int[] obj) => obj.GetHashCode();
	}
}