/*	Copyright (c) 2017  Kenneth Brady
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and asssociated documentation files (the "Software"), to deal
 *	in the Sortware without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *	
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *	
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
*/

using Base;
using System;
using System.IO;

namespace Elements
{
	public enum Block
	{
		S,
		P,
		D,
		F
	}

	public enum Family
	{
		Carbon								= 14,
		NobleGases						= 18,
		AlkaliMetals					= 1,
		AlkalineEarthMetals		= 2,
		Pnictogens						= 15,
		Chalcogens						= 16,
		Halogens							= 17,
		None									= 0
	}

	public class Group : IPrintable, IAsDouble
	{
		public static Group Parse(string v)
		{
			return new Group(v);
		}

		/// <summary>
		/// Restore from printed value
		/// </summary>
		/// <param name="value"></param>
		private Group(string value)
		{
			// '1-S'  '13-P', etc.
			string[] parts = value.Split('-');
			Number = int.Parse(parts[0]);
			Block b;
			if (Enum.TryParse<Block>(parts[1], out b)) Block = b;
		}

		public Group(int number, string block, bool isHydrogen)
		{
			Number = number;
			Block = (Block)Enum.Parse(typeof(Block), block.ToUpper());
			IsHydrogen = isHydrogen;
		}

		public int Number { get; private set; }
		public Block Block { get; private set; }
		private bool IsHydrogen { get; set; }
		public Family Name
		{
			get
			{
				if (IsHydrogen) return Family.None;
				if (Enum.IsDefined(typeof(Family), Number))
				{
					return (Family)Number;
				}
				return Family.None;
			}
		}

		public override string ToString()
		{
			return String.Format("Group {0} {1}-block", Number, Block);
		}

		public string Print(PrintOptions ops)
		{
			return String.Format("{0}-{1}", Number, Block);
		}

		public double AsDouble()
		{
			return Number;
		}
	}
}
