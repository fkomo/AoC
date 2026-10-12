namespace Ujeby.AoC.Common
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AoCPuzzleAttribute : Attribute
	{
		public string Answer1 { get; set; }
		public string Answer2 { get; set; }
		public bool Skip { get; set; }
		public int Year { get; set; }
		public int Day { get; set; }
	}
}
