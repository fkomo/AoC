using SDL2;
using System.Diagnostics;
using System.Numerics;
using Ujeby.AoC.Vis.App.Common;

namespace Ujeby.AoC.Vis.App
{
	internal class Program
	{
		public static IntPtr WindowPtr;
		public static IntPtr RendererPtr;
		public static Vector2 WindowSize = new(1920, 1080);
		public static Font CurrentFont;

		private static readonly Stopwatch stopwatch = Stopwatch.StartNew();

		static void Main()
		{
			try
			{
				stopwatch.Restart();

				SpriteCache.Initialize();

				InitSDL();

				CurrentFont = SpriteCache.LoadFont("font-5x7");
				SpriteCache.CreateTexture(CurrentFont.SpriteId, out _);

				var menu = new Menu(
					new IRunnable[]
					{
						new RopeBridge(),
						new Chitron(),
						new HillClimbingAlgorithm(),
						new RegolithReservoir(),
					}
				);
				
				menu.Run(HandleInput);

				menu.Selected?.Run(HandleInput);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				DestroySDL();
			}
		}

		private static void InitSDL()
		{
			if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
				throw new Exception($"Failed to initialize SDL2 library. SDL2Error({SDL.SDL_GetError()})");

			WindowPtr = SDL.SDL_CreateWindow("UjebyTest",
				SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
				(int)WindowSize.X, (int)WindowSize.Y,
				SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN);
			if (WindowPtr == IntPtr.Zero)
				throw new Exception($"Failed to create window. SDL2Error({SDL.SDL_GetError()})");

			RendererPtr = SDL.SDL_CreateRenderer(WindowPtr, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
			if (RendererPtr == IntPtr.Zero)
				throw new Exception($"Failed to create renderer. SDL2Error({SDL.SDL_GetError()})");

			SDL.SDL_SetRenderDrawBlendMode(RendererPtr, SDL.SDL_BlendMode.SDL_BLENDMODE_ADD);
		}

		private static void DestroySDL()
		{
			SDL.SDL_DestroyRenderer(RendererPtr);
			SDL.SDL_DestroyWindow(WindowPtr);
			SDL.SDL_Quit();
		}

		private static readonly byte[] CurrentKeys = new byte[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];
		private static readonly byte[] PreviousKeys = new byte[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];

		public static int MouseWheel { get; set; }

		private static bool HandleInput()
		{
			while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
			{
				MouseWheel = 0;

				switch (e.type)
				{
					case SDL.SDL_EventType.SDL_QUIT:
						return false;
				};

				if (e.type == SDL.SDL_EventType.SDL_KEYUP || e.type == SDL.SDL_EventType.SDL_KEYDOWN)
				{
					CurrentKeys.CopyTo(PreviousKeys, 0);
					var keysBuffer = SDL.SDL_GetKeyboardState(out int keysBufferLength);
					System.Runtime.InteropServices.Marshal.Copy(keysBuffer, CurrentKeys, 0, keysBufferLength);

					if (KeyPressed(SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE))
						return false;
				}

				else if (e.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
				{
					if (e.wheel.y > 0) // scroll up
					{
					}
					else if (e.wheel.y < 0) // scroll down
					{
					}

					MouseWheel = e.wheel.y;
				}
			}

			return true;
		}

		internal static bool KeyPressed(SDL.SDL_Scancode scanCode)
		{
			return CurrentKeys[(int)scanCode] == 1 && PreviousKeys[(int)scanCode] == 0;
		}

		internal static bool KeyReleased(SDL.SDL_Scancode scanCode)
		{
			return CurrentKeys[(int)scanCode] == 0 && PreviousKeys[(int)scanCode] == 1;
		}
	}
}
