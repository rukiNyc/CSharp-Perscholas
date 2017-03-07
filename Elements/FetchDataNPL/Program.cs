using Base;
using Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace FetchDataNPL
{
	class Program : ConsoleAppBase
	{
		private static string _outFolder = Path.Combine(Environment.CurrentDirectory, "Output");
		private static ElementList _elements = new ElementList();
		static int Main(string[] args)
		{
			const string OO = "/o:";
			if (args.Length > 1)
			{
				ShowHelp();
				return 1;
			}
			if (args.Length == 1 && args[0].StartsWith(OO))
			{
				_outFolder = args[0].Substring(OO.Length);
			}
			Dictionary<Element, XElement>  xels = LoadTable();
			ExtractValues(xels);
			if (!Directory.Exists(_outFolder)) Directory.CreateDirectory(_outFolder);
			StringBuilder output = new StringBuilder();
			output.AppendFormat("Symbol\tWeight\tDensity\tMP\tBP").AppendLine();
			foreach(Element e in xels.Keys)
			{
				Qualities q = e.Qualities;
				//if (q.BoilingPoint == null) q.BoilingPoint = String.Empty;
				if (q.Density == null) q.Density = new UnitValue(0, String.Empty);
				output.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}", e.Symbol, q.StandardAtomicWeight, q.Density.Value, q.MeltingPoint, q.BoilingPoint).AppendLine();
			}
			File.WriteAllText(Path.Combine(_outFolder, "values.txt"), output.ToString());
			return 0;
		}

		static void ShowHelp()
		{
			Console.WriteLine("Arguments:");
			Console.WriteLine("\t/o:outputFolder\t\t(defaults to <current-directory>/Output");
		}

		private static Dictionary<Element, XElement> LoadTable()
		{
			const string NPL = "npl.html";
			string html;
			WebRequest wreq = WebRequest.Create("http://www.kayelaby.npl.co.uk/chemistry/3_1/3_1_2.html");
			WebResponse wresp = wreq.GetResponse();
			using (Stream ss = wresp.GetResponseStream())
			{
				StreamReader rdr = new StreamReader(ss);
				html = rdr.ReadToEnd();
			};
			if (String.IsNullOrEmpty(html)) return null;
			File.WriteAllText(Path.Combine(_outFolder, NPL), html);
			List<Element> byName = new List<Element>(_elements);
			byName.Sort((ex, ey) =>
			{
				return String.Compare(ex.Name, ey.Name);
			});
			int n1st = html.IndexOf(byName[0].Name), nLst = html.IndexOf(byName.Last().Name);
			List<string> notFound = new List<string>();
			Dictionary<Element, XElement> xels = new Dictionary<Element, XElement>();
			foreach(Element e in byName)
			{
				// two alternate spellings:
				string name = e.Name;
				if (name == "Cesium") name = "Caesium"; else if (name == "Sulfur") name = "Sulphur";
				int ndx = html.IndexOf(name, n1st);
				if (ndx >= n1st && ndx <= nLst)
				{
					int trn = html.LastIndexOf("<TR", ndx), trx = html.IndexOf("</TR>", trn);
					string str = html.Substring(trn, 5 + trx - trn).Replace("&nbsp;", "").Replace("&plusmn;", "+-").Replace("&dagger;", "")
						.Replace("&ndash;", "").Replace("&minus;", "-").Replace("&sect;", "").Replace("&deg;", "").Replace("&Dagger;", "");
					try
					{
						XElement xl = XElement.Parse(str);
						//WriteSuccess("{0}:\tok", e.Symbol);
						xels.Add(e, xl);
					}
					catch(Exception ex)
					{
						WriteError("{0}:\t{1}", e.Symbol, ex.Message);
					}
				}
				else notFound.Add(e.Symbol);
			}
			Console.WriteLine("Not Found:\t{0}", String.Join(",", notFound.ToArray()));
			return xels;
		}

		private static bool TryParse(ref string val, out double d)
		{
			d = double.NaN;
			if (String.IsNullOrEmpty(val)) return false;
			val = val.Trim().Replace(" ", String.Empty).Replace("\t", String.Empty).Replace("\n", String.Empty).Replace("\u0096", "-");
			int n;
			if (val.IndexOf("(")== 0)
			{
				n = val.IndexOf(")");
				val = val.Substring(1, n-1);
			}
			n = val.IndexOf("+-");
			if (n > 0) val = val.Substring(0, n);
			if (double.TryParse(val, out d)) return true;
			for(n=0;n<val.Length;++n)
			{
				var c = val[n];
				if (!Char.IsNumber(c) && (c != '.')) break;
			}
			if (n < val.Length) val = val.Substring(0, n);
			if (double.TryParse(val, out d)) return true;
			return false;
		}

		private static void ExtractValues(Dictionary<Element,XElement> rows)
		{
			foreach (Element e in rows.Keys)
			{
				XElement xe = rows[e];
				List<XElement> tds = new List<XElement>(xe.Elements("TD"));
				int nProp = 0;
				Qualities q = new Qualities(e);
				foreach(XElement xel in tds)
				{
					double d;
					string val = xel.Value;
					switch (nProp)
					{
						case 3:		// Atomic weight
							if (TryParse(ref val, out d))
							{
								q.StandardAtomicWeight = d;
							}
							break;
						case 4:
							if (TryParse(ref val, out d))
							{
								d /= 1000;
								q.Density = new UnitValue(d, "g/cm3");
							}
							break;
						case 5:
							if (TryParse(ref val, out d))
							{
								q.MeltingPoint = new UnitValue(d, Units.DegreesK);
							}
							break;
						case 6:
							if (TryParse(ref val, out d))
							{
								q.BoilingPoint = new UnitValue(d, Units.DegreesK);
							}
							break;
					}
					nProp++;
				}
			}
		}

	}
}
