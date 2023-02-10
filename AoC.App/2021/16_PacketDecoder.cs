using System.Globalization;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_16
{
	internal class PacketDecoder : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			//ParsePacket("D2FE28", out _);
			//ParsePacket("38006F45291200", out _);
			//ParsePacket("EE00D40C823060", out _);
			//ParsePacket("8A004A801A8002F478", out _);
			//ParsePacket("620080001611562C8802118E34", out _);
			//ParsePacket("C0015000016115A2E0802F182340", out _);
			//ParsePacket("A0016C880162017C3686B18A3D4780", out _);

			// part1
			// part2
			var answer2 = ParsePacket(input[0], out long answer1);

			return (answer1.ToString(), answer2.ToString());
		}

		private static long ParsePacket(string input, out long versionSum)
		{
			Debug.Line(input);

			var bytes = new byte[input.Length / 2];
			for (var i = 0; i < bytes.Length; i++)
				bytes[i] = byte.Parse($"{input[i * 2]}{input[i * 2 + 1]}", NumberStyles.HexNumber);

			var result = ParsePacket(bytes, 0, out _, out versionSum);
			Debug.Line();

			return result;
		}

		private static long ParsePacket(byte[] bytes, int start, out int read, out long versionSum)
		{
			Debug.Indent += 2;

			long result = 0;

			versionSum = 0;
			var version = GetValue(bytes, start, 3);
			var typeId = GetValue(bytes, start + 3, 3);
			read = 6;

			versionSum += version;

			if (typeId == 4)
			{
				result = ParseLiteral(bytes, start + read, out int literalLength);
				read += literalLength;

				Debug.Line($"LITERAL[{start}-{start + read}]: version={version} typeId={typeId} literal={result}");
			}
			else
			{
				var operands = new List<long>();

				var lengthTypeId = GetValue(bytes, start + read, 1);
				read++;

				if (lengthTypeId == 0)
				{
					var subPacketslength = GetValue(bytes, start + read, 15);
					read += 15;

					Debug.Line($"OP[{start}-{start + read}]: " 
						+ $"version={version} typeId={typeId} lengthTypeId={lengthTypeId} subPacketslength={subPacketslength}");

					var toRead = (int)subPacketslength;
					while (toRead > 0)
					{
						operands.Add(ParsePacket(bytes, start + read, out int packetLength, out long _versionSum));
						read += packetLength;
						versionSum += _versionSum;

						toRead -= packetLength;
					}

					result = Result(typeId, operands);
				}
				else if (lengthTypeId == 1)
				{
					var numOfsubPackets = GetValue(bytes, start + read, 11);
					read += 11;

					Debug.Line($"OP[{start}-{start + read}]: " 
						+ $"version={version} typeId={typeId} lengthTypeId={lengthTypeId} numOfsubPackets={numOfsubPackets}");

					for (var i = 0; i < numOfsubPackets; i++)
					{
						operands.Add(ParsePacket(bytes, start + read, out int packetsLength, out long _versionSum));
						read += packetsLength;
						versionSum += _versionSum;
					}

					result = Result(typeId, operands);
				}
			}

			Debug.Indent -= 2;

			return result;
		}

		private static long Result(ushort packetTypeId, IEnumerable<long> operands)
		{
			switch (packetTypeId)
			{
				// sum
				case 0:
					return operands.Sum();

				// product
				case 1:
					{
						var result = operands.First();
						foreach (var p in operands.Skip(1))
							result *= p;

						return result;
					}

				// min
				case 2:
					return operands.Min();

				// max
				case 3:
					return operands.Max();

				// greater than
				case 5:
					return operands.First() > operands.Last() ? 1 : 0;

				// less than
				case 6:
					return operands.First() < operands.Last() ? 1 : 0;

				// equal to
				case 7:
					return operands.First() == operands.Last() ? 1 : 0;

				default:
					throw new NotImplementedException();
			};
		}

		private static long ParseLiteral(byte[] bytes, int start, out int read)
		{
			read = 0;
			long literal = 0;
			var literalLength = 0;

			bool last;
			do
			{
				last = GetValue(bytes, start + read, 1) == 0;
				literal <<= literalLength;

				literal += GetValue(bytes, start + 1 + read, 4);
				literalLength = 4;

				read += 5;
			} while (!last);

			return literal;
		}

		/// <summary>
		/// reads (max 2B/16Bit) value from specified position in byte array
		/// </summary>
		/// <param name="bytes">byte array</param>
		/// <param name="start">starting bit</param>
		/// <param name="length">bit length of desired value</param>
		/// <returns></returns>
		private static ushort GetValue(byte[] bytes, int start, int length)
		{
			uint result = 0;
			var resultLength = 0;

			var shift = 0;
			var b = start / 8;
			for (var i = 0; i < 3 && b < bytes.Length; i++, b++, resultLength += 8)
			{
				result <<= shift;
				result += bytes[b];
				shift = 8;
			}

			return (ushort)((result >> resultLength - (start % 8) - length) & (uint)(Math.Pow(2, length) - 1));
		}
	}
}
