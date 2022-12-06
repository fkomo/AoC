namespace Ujeby.AoC.Common
{
	public interface ISolvable
	{
		public int Day { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>number of right answers / stars</returns>
		int Solve();
	}
}