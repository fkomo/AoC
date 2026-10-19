namespace Ujeby.Tools
{
	public static class AssertEx
	{
		public static void NotEqual<T>(T expected, T result, string message = null)
		{
			if (!expected.Equals(result))
				throw new Exception($"{message ?? "?"}: {result} != {expected} (expected)");
		}
	}
}
