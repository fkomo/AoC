namespace Ujeby.AoC.Common
{
	public static class AoCHttpClient
	{
		public const string BaseUrl = "https://adventofcode.com";

		private readonly static HttpClient _httpClient = new();
		private static bool _aocSessionSet = false;

		public static void SetSession(string session)
		{
			if (string.IsNullOrEmpty(session))
				return;

			_httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");
			_aocSessionSet = true;
		}

		public static async Task<HttpResponseMessage> GetAsync(string url)
		{
			if (!_aocSessionSet)
				throw new Exception($"AoC session not set!");

			return await _httpClient.GetAsync(url);
		}

		public static async Task<bool> SendAnswerAsync(int year, int day, int part, string answer)
		{
			if (!_aocSessionSet)
				throw new Exception($"AoC session not set!");

			var url = $"{BaseUrl}/{year}/day/{day}/answer";

			var content = new FormUrlEncodedContent(
				new Dictionary<string, string>
				{
					{ "answer", answer },
					{ "level", part.ToString() }
				});

			var response = await _httpClient.PostAsync(url, content);
			if (response.IsSuccessStatusCode)
			{

			}

			// TODO parse answer response

			return false;
		}
	}
}
