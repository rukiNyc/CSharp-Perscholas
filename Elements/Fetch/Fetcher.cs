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

using Elements;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fetch
{
	class Fetcher
	{
		private static readonly string _wikiRoot = "https://en.wikipedia.org/w/index.php?title={0}&printable=yes";
		public static async Task<bool> FetchElement(Element e, Arguments A)
		{
			string webPath = String.Format(_wikiRoot, e.WebName);
			HttpClient client = new HttpClient();
			Task<string> tsk = client.GetStringAsync(webPath);
			string content = await tsk;
			string outPath = A.CreateOutputPath(e.Name);
			File.WriteAllText(outPath, content);
			return true;
		}
	}
}
