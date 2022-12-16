﻿using System.Numerics;

namespace Ujeby.AoC.Vis.App.Common
{
	public class Sprite
	{
		/// <summary>internal sprite id</summary>
		public string Id;

		public string Filename;

		/// <summary>width x height</summary>
		public Vector2 Size;
		
		/// <summary>
		/// pixel format: 0xARGB
		/// topLeft -> bottomRight
		/// </summary>
		public uint[] Data;

		/// <summary>sdl2 image pointer</summary>
		public IntPtr ImagePtr = IntPtr.Zero;

		/// <summary>sdl2 texture pointer</summary>
		public IntPtr TexturePtr = IntPtr.Zero;
	}
}
