using System.Globalization;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day16
{
	internal class PacketDecoder : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			ReadPackets("D2FE28");
			ReadPackets("38006F45291200");

			//ReadPackets("EE00D40C823060");
			//Debug.Line();
			

			// part1
			long? answer1 = null;

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long ReadPackets(string input)
		{
			var bytes = new byte[input.Length / 2];
			for (var i = 0; i < bytes.Length; i++)
				bytes[i] = byte.Parse($"{input[i * 2]}{input[i * 2 + 1]}", NumberStyles.HexNumber);

			return ParsePackets(bytes, 0, out int read,
				length: bytes.Length * 8);
		}

		private static long ParsePackets(byte[] bytes, int start, out int read,
			int? length = null, int? count = null)
		{
			long versionSum = 0;

			var b = start;
			var c = 0;
			while (length.HasValue ? (b + 6 < length) : c < count)
			{
				Debug.Line();
				Debug.Text("PACKET: ", indent: 6);

				var version = Mask(bytes, b, 3);
				b += 3;
				Debug.Text($" ver={version}");

				var typeId = Mask(bytes, b, 3);
				b += 3;
				Debug.Text($" id={typeId}");

				versionSum += version;

				if (typeId == 4)
				{
					var literal = ReadLiteral(bytes, b, out int r);
					b += r;
					Debug.Text($" literal={literal}");
				}
				else
				{
					var lengthTypeId = Mask(bytes, b, 1);
					b++;
					Debug.Text($" lengthTypeId={lengthTypeId}");

					if (lengthTypeId == 0)
					{
						var subPacketslength = Mask(bytes, b, 15);
						b += 15;
						Debug.Text($" subPacketslength={subPacketslength}");
						versionSum += ParsePackets(bytes, b, out int r, 
							length: subPacketslength);
						b += r;
					}
					else if (lengthTypeId == 1)
					{
						var numOfsubPackets = Mask(bytes, b, 11);
						b += 11;
						Debug.Text($" numOfsubPackets={numOfsubPackets}");
						versionSum += ParsePackets(bytes, b, out int r,
							count: numOfsubPackets);
						b += r;
					}
				}
			}

			c++;
			read = b - start;
			return versionSum;
		}

		private static long ReadLiteral(byte[] bytes, int start, out int length)
		{
			length = start;
			long literal = 0;
			var literalLength = 0;

			bool notLast;
			do
			{
				notLast = Mask(bytes, length, 1) == 1;
				length++;

				literal <<= literalLength;
				literal += Mask(bytes, length, 4);
				length += 4;
				literalLength = 4;

			} while (notLast);

			length -= start;
			return literal;
		}

		public static ushort Mask(byte[] bytes, int start, int length)
		{
			uint result = 0;
			var totalLength = 0;

			var b0 = start / 8;
			result += bytes[b0];
			totalLength += 8;

			if (b0 + 1 < bytes.Length)
			{
				result <<= 8;
				result += bytes[b0 + 1];
				totalLength += 8;
			}

			if (b0 + 2 < bytes.Length)
			{
				result <<= 8;
				result += bytes[b0 + 2];
				totalLength += 8;
			}

			result >>= totalLength - (start % 8) - length;

			var mask = (uint)(Math.Pow(2, length) - 1);

			return (ushort)(result & mask);
		}
	}
}
