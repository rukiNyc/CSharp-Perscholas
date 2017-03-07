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
