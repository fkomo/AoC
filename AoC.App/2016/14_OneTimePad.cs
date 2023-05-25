using System.Security.Cryptography;
using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_14;

[AoCPuzzle(Year = 2016, Day = 14, Answer1 = "15168", Answer2 = null, Skip = false)]
public class OneTimePad : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var salt = input.Single();

		// part1
		var index = -1;
		var keysFound = 0;
		do
		{
			index++;
			var source = salt + index.ToString();
			var hash = HashCached(source);

			if (!Same3(hash, out char same3))
				continue;

			for (var i = 1; i <= 1000; i++)
			{
				var nextSource = salt + (index + i).ToString();
				var nextHash = HashCached(nextSource);
				if (Same5(nextHash, same3))
				{
					keysFound++;
					Debug.Line($"{keysFound,2}: {hash} {same3} {source}");
					break;
				}
			}
		}
		while (keysFound < 64);
		var answer1 = index;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	//private static Dictionary<int, string> _indexedHashCache = new();

	//private static string NextHash(string salt, int lastIndex, out int index)
	//{
	//	var nextIndex = lastIndex + 1;
	//	if (_indexedHashCache.Keys.Last() >= nextIndex)
	//	{
	//		index = _indexedHashCache.Keys.Where(x => x > nextIndex).First();
	//		return _indexedHashCache[index];
	//	}

	//	index = nextIndex;
	//	do
	//	{
	//		var source = salt + index.ToString();
	//		var hash = HashCached(source);
	//	}
	//	while ();
	//}

	private static Dictionary<string, string> _hashCache = new();
	private static string HashCached(string source)
	{
		if (_hashCache.TryGetValue(source, out string hash))
			return hash;

		hash = Hash(source);
		_hashCache.Add(source, hash);
		return hash;
	}

	private static MD5 _md5 = MD5.Create();

	private static string Hash(string source)
		=> Convert.ToHexString(_md5.ComputeHash(Encoding.ASCII.GetBytes(source))).ToLower();

	private static bool Same3(string hash, out char same3)
	{
		same3 = ' ';
		for (var i = 0; i < hash.Length - 2; i++)
			if (hash[i] == hash[i + 1] && hash[i] == hash[i + 2])
			{
				same3 = hash[i];
				return true;
			}

		return false;
	}

	private static bool Same5(string hash, char same3)
	{
		for (var i = hash.IndexOf(same3); i >= 0 && i < hash.Length - 5; i = hash.IndexOf(same3, i))
		{
			var valid = true;
			for (var i2 = 1; i2 < 5; i2++)
				if (hash[i2 + i] != same3)
				{
					i += i2;
					valid = false;
					break;
				}

			if (valid)
				return true;
		}

		return false;
	}
}