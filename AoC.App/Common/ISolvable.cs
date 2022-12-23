namespace Ujeby.AoC.Common
{
	public interface IPuzzle
	{
		public int Day { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>number of right answers / stars</returns>
		int Solve();
	}
}