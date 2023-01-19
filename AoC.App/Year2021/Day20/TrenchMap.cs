using System.Text;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day20
{
	public class TrenchMap : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var imageEnhAlg = input.First();
			var inputImage = input.Skip(2).ToArray();

			// part1
			var output = (Image: inputImage, VoidPixel: '.');
			var i = 0;
			for (; i < 2; i++)
				output = EnhanceImage(output, imageEnhAlg);
			long? answer1 = output.Image.Sum(line => line.Count(c => c == '#'));

			// part2
			for (; i < 50; i++)
				output = EnhanceImage(output, imageEnhAlg);
			long? answer2 = output.Image.Sum(line => line.Count(c => c == '#'));

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static readonly v2i[] _dir = new v2i[]
		{
			v2i.Down + v2i.Left,	v2i.Down,	v2i.Down + v2i.Right,
			v2i.Left,				v2i.Zero,	v2i.Right,
			v2i.Up + v2i.Left,		v2i.Up,		v2i.Up + v2i.Right,
		};

		private static (string[] Image, char VoidPixel) EnhanceImage((string[] Image, char VoidPixel) input, string imageEnhAlg)
		{
			var pixelSb = new StringBuilder();
			var outputLineSb = new StringBuilder();

			var outputImage = new string[input.Image.Length + 2];

			var outputPixel = new v2i(0, 0);
			for (; outputPixel.Y < outputImage.Length; outputPixel.Y++)
			{
				outputLineSb.Clear();
				for (outputPixel.X = 0; outputPixel.X < input.Image[0].Length + 2; outputPixel.X++)
				{
					pixelSb.Clear();
					foreach (var d in _dir)
					{
						var inputPixel = outputPixel + d - 1;
						if (inputPixel.X < 0 || inputPixel.Y < 0 ||
							inputPixel.X >= input.Image[0].Length || inputPixel.Y >= input.Image.Length)
							pixelSb.Append(input.VoidPixel);
						else
							pixelSb.Append(input.Image[inputPixel.Y][(int)inputPixel.X]);
					}

					outputLineSb.Append(Enhance(imageEnhAlg, pixelSb.ToString()));
				}

				outputImage[outputPixel.Y] = outputLineSb.ToString();
			}

			return (Image: outputImage, VoidPixel: input.VoidPixel == '.' ? imageEnhAlg[0] : imageEnhAlg[511]);
		}

		private static void DrawImage(string[] image)
		{
			foreach (var line in image)
				Debug.Line(line);
			Debug.Line();
		}

		public static char Enhance(string imageEnhAlg, string input) 
			=> imageEnhAlg[(int)Tools.Numbers.BaseToDec(input, baseString: ".#")];
	}
}
