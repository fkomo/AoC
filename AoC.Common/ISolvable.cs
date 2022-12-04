namespace Ujeby.AoC.Common
{
	public interface ISolvable
	{
		public int Day { get; }
		bool Solve(
			string inputUrl = null, string session = null);
	}
}