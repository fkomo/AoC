using System.IO.Compression;
using System.Text;

namespace Ujeby.Tools
{
	public class GZip
	{
		public static byte[] Compress(string stringToCompress)
		{
			var bytes = Encoding.UTF8.GetBytes(stringToCompress);

			using var msi = new MemoryStream(bytes);
			using var mso = new MemoryStream();
			using (var gs = new GZipStream(mso, CompressionMode.Compress))
				CopyTo(msi, gs);

			return mso.ToArray();
		}

		public static string Decompress(byte[] bytesToDecompress)
		{
			using var msi = new MemoryStream(bytesToDecompress);
			using var mso = new MemoryStream();
			using (var gs = new GZipStream(msi, CompressionMode.Decompress))
				CopyTo(gs, mso);

			return Encoding.UTF8.GetString(mso.ToArray());
		}

        static void CopyTo(Stream source, Stream destination)
        {
            var bytes = new byte[4096];

            int cnt;
            while ((cnt = source.Read(bytes, 0, bytes.Length)) != 0)
                destination.Write(bytes, 0, cnt);
        }
    }
}
