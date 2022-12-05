namespace Ujeby.AoC.Common
{
	public interface ISolvable
	{
		public int Day { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputUrl"></param>
		/// <param name="session"></param>
		/// <returns>number of right answers / stars</returns>
		int Solve(
			string inputUrl = null, string session = null);
	}
}