using System;

namespace Base
{
	[Flags]
	public enum PrintOptions
	{
		None							= 0x00,
		Units							= 0x01,
		Qualifier					= 0x02
	}

	public static class PrintOptionsEx
	{
		public static bool ShowUnits(this PrintOptions op)
		{
			return op.HasFlag(PrintOptions.Units);
		}
		
		public static bool ShowQualifier(this PrintOptions op)
		{
			return op.HasFlag(PrintOptions.Qualifier);
		}
	}

	public interface IPrintable
	{
		string Print(PrintOptions ops);
	}
}
