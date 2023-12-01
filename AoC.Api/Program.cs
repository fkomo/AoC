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

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapGet("/puzzle/{year}/{day}", (int year, int day) =>
			{
				return new PuzzleResult(year, day);
			});

			app.Run();
		}
	}

	record class PuzzleResult(
		int Year, 
		int Day, 
		string? Answer1 = null, 
		string? Answer2 = null, 
		double? Duration = null);
}