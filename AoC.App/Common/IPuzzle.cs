namespace Ujeby.AoC.Common
{
	public interface IPuzzle
	{
		public int Day { get; }
		public int Year { get; }
		public string Title { get; }

		/// <summary></summary>
		/// <param name="inputStorage"></param>
		/// <returns>number of right answers / stars</returns>
		int Solve(string inputStorage);
	}
}