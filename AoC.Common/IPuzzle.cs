namespace Ujeby.AoC.Common
{
	public interface IPuzzle
	{
		public int Day { get; }
		public int Year { get; }
		public string Title { get; }
		public bool Skip { get; set; }

		/// <summary></summary>
		/// <param name="inputStorage"></param>
		/// <param name="inputSuffix"></param>
		/// <returns>number of right answers / stars</returns>
		int Solve(string inputStorage, string inputSuffix = null);

		public (string Part1, string Part2) Answer { get; }

		public (string Part1, string Part2) Solution { get; }
		public double? Time { get; }
	}
}