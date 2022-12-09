namespace Ujeby.AoC.App.Year2022.Day07
{
	internal class DirEntry
	{
		public string Name { get; set; }

		public long Size { get; set; }

		/// <summary> name, size </summary>
		public List<(string, long)> Files { get; set; } = new List<(string, long)>();
	}
}
