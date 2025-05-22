using System;

namespace Flidais.Helper
{
	public static class ByteSizeHelper
	{
		public static string DetermineByte(long bytes)
		{
			int shortenedBytes = 0;
			switch (bytes)
			{
				case long l when (l < 1024):
					shortenedBytes = (int)bytes;
					return $"{shortenedBytes} bytes";
				case long l when (l < 1048576):
					shortenedBytes = (int)bytes / 1024;
					return $"{shortenedBytes} kb";
				case long l when (l < 1073741824):
					shortenedBytes += (int)bytes / 1048576;
					return $"{shortenedBytes} mb";
				case long l when (l > 1073741824):
					shortenedBytes = (int)bytes / 1073741824;
					return $"{shortenedBytes} gb";
				default:
					throw new Exception();
			}
		}
	}
}
