﻿using System.Data;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_07
{
	[AoCPuzzle(Year = 2022, Day = 07, Answer1 = "1086293", Answer2 = "366028")]
	public class NoSpaceLeftOnDevice : PuzzleBase
	{
		public class DirEntry
		{
			public string Name { get; set; }

			public long Size { get; set; }

			/// <summary> name, size </summary>
			public List<(string, long)> Files { get; set; } = new List<(string, long)>();
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			var root = new DirEntry
			{
				Name = "/",
			};

			var fs = new Dictionary<string, DirEntry>();
			fs.Add(root.Name, root);

			// part1
			var path = new Stack<DirEntry>();
			for (var i = 0; i < input.Length; i++)
			{
				var line = input[i];

				if (line.StartsWith("$ cd"))
				{
					var dirName = line["$ cd ".Length..];
					if (dirName == "..")
					{
						path.Pop();
					}
					else if (dirName == root.Name)
					{
						path.Clear();
						path.Push(root);
					}
					else
					{
						var fullName = string.Join("", path.Select(p => p.Name)) + dirName;
						path.Push(fs[fullName]);
					}
				}
				else if (line.StartsWith("$ ls"))
				{
				}
				else
				{
					var split = line.Split(' ');
					if (split[0] == "dir")
					{
						var fullName = string.Join("", path.Select(p => p.Name)) + split[1];
						//Debug.Line(fullName);

						if (!fs.ContainsKey(fullName))
							fs.Add(fullName, new DirEntry { Name = fullName });
						
						path.First().Files.Add((fullName, -1));
					}
					else
						path.First().Files.Add((split[1], long.Parse(split[0])));
				}
			}

			var fsSize = GetSize(fs, root.Name);
			long? answer1 = fs.Where(dir => dir.Value.Size < 100000).Sum(dir => dir.Value.Size);

			// part2
			long diskSize = 70000000;
			long requiredFreeSpace = 30000000;
			var toFreeMin = requiredFreeSpace - (diskSize - fsSize);
			long? answer2 = fs.Select(d => d.Value.Size).Where(s => s > toFreeMin).OrderBy(s => s).First();

			return (answer1?.ToString(), answer2?.ToString());
		}

		protected long GetSize(Dictionary<string, DirEntry> fs, string dirName)
		{
			if (fs[dirName].Size > 0)
				return fs[dirName].Size;

			long size = 0;
			foreach (var file in fs[dirName].Files)
				size += (file.Item2 == -1) ? GetSize(fs, file.Item1) : file.Item2;

			fs[dirName].Size = size;

			return size;
		}
	}
}
