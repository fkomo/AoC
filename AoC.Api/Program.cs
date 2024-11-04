using Ujeby.AoC.Common;

namespace Ujeby.AoC.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// f**k you CORS
			app.Use(async (context, next) =>
			{
				if (context.Request.Method == "OPTIONS")
				{
					context.Response.StatusCode = 200;

					await WriteCorsAsync(context);

					await context.Response.WriteAsync("");
				}
				else
				{
					context.Response.OnStarting(WriteCorsAsync, context);

					await next(context);
				}

				static Task WriteCorsAsync(object state)
				{
					var context = (HttpContext)state;

					context.Response.Headers.Append("Access-Control-Allow-Origin", context.Request.Headers.Origin);
					context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
					context.Response.Headers.Append("Access-Control-Allow-Methods", string.Join(", ", new[] { context.Request.Method }.Union(context.Request.Headers.AccessControlRequestMethod.Select(v => v))));
					context.Response.Headers.Append("Access-Control-Allow-Headers", string.Join(", ", context.Request.Headers.Select(p => p.Key).Union(context.Request.Headers.AccessControlRequestHeaders.Select(v => v))));

					return Task.CompletedTask;
				}
			});

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			// https://stackoverflow.com/questions/10284861/not-all-assemblies-are-being-loaded-into-appdomain-from-the-bin-folder
			Ujeby.AoC.App.Program.Init();

			app.MapGet("/meta", () =>
			{
				return AdventOfCode.AllPuzzles()
					.GroupBy(x => x.Year)
					.Select(x => new AoCYear(x.Key, x.Select(p => CreateMeta(p)).ToArray()))
					.ToArray();
			});

			app.MapGet("/{year}/{day}", (int year, int day) =>
			{
				string env = null;
				var envFilePath = Path.Combine(AppContext.BaseDirectory, "env.txt");
				if (File.Exists(envFilePath))
					env = File.ReadAllText(envFilePath);

				var settingsFile = (string.IsNullOrEmpty(env)) ? "appsettings.json" : $"appsettings.{env}.json";

				var config = new ConfigurationBuilder()
					.AddJsonFile(settingsFile)
					.Build();

				var puzzle = AdventOfCode.GetInstance(year, day);
				if (puzzle == null)
					return Results.NotFound();

				var result = puzzle.Solve(config["aoc:input"]);

				return Results.Ok(new PuzzleResult(CreateMeta(puzzle), puzzle.Solution.Part1, puzzle.Solution.Part2, puzzle.Time));
			});

			app.Run();
		}

		static PuzzleMeta CreateMeta(IPuzzle puzzle)
			=> new(puzzle.Year, puzzle.Day, puzzle.Title, puzzle.Answer.Part1, puzzle.Answer.Part2, puzzle.Skip);
	}

	record class PuzzleResult(
		PuzzleMeta Meta,
		string Part1 = null, 
		string Part2 = null, 
		double? Time = null);

	record class AoCYear(int Year, PuzzleMeta[] Puzzles);

	record class PuzzleMeta(int Year, int Day, string Title, string AnswerPart1, string AnswerPart2, bool Skip);
}