using Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Elements
{
	public class ValuePair : Tuple<double?,double?>
	{
		internal ValuePair(double? v1, double? v2, Element element): base(v1, v2)
		{
			if (element == null) throw new ArgumentNullException(nameof(element));
			Element = element;
		}

		public double V1 => Item1.HasValue ? Item1.Value : double.NaN;
		public double V2 => Item2.HasValue ? Item2.Value : double.NaN;

		public bool IsComplete => Item1.HasValue && Item2.HasValue;

		public Element Element { get; private set; }
	}

	public class ElementList : List<Element>
	{
		public ElementList(bool loadCore = true)
		{
			if (loadCore) AddRange(Element.LoadCore());
		}

		public ElementList(string tablePath): this(true)
		{
			ReadTable(tablePath);
		}

		public ElementList(IEnumerable<Element> elements): base(elements)
		{
		}

		public Element Find(string symbolOrName)
		{
			Element e;
			TryFind(symbolOrName, out e);
			return e;
		}

		/// <summary>
		/// Try to find an element with the given symbol or name.
		/// </summary>
		/// <param name="symbolOrName"></param>
		/// <param name="elem"></param>
		/// <returns>true if found</returns>
		/// <remarks>This method performs in O(N) time.</remarks>
		public bool TryFind(string symbolOrName, out Element elem)
		{
			elem = null;
			if (String.IsNullOrEmpty(symbolOrName)) return false;
			bool isSym = symbolOrName.Length <= 2;
			// Note: linear search
			Element e = this.FirstOrDefault(ee => isSym ? symbolOrName == ee.Symbol : symbolOrName == ee.Name);
			if (e == null) return false;
			elem = e;
			return true;
		}

		/// <summary>
		/// Return a list of quality values, one for each element.
		/// </summary>
		/// <param name="qualityName"></param>
		/// <returns></returns>
		public List<double?> GetValuesAsDouble(string qualityName)
		{
			return new List<double?>(this.Select(e => e.Qualities.GetValueAsDouble(qualityName)));
		}

		public List<ValuePair> GetValuesAsDouble(string qualityName1, string qualityName2, bool completeOnly)
		{
			IEnumerable<ValuePair> pairs = this.Select(e => new ValuePair(e.Qualities.GetValueAsDouble(qualityName1),
				e.Qualities.GetValueAsDouble(qualityName2), e));
			if (completeOnly) pairs = pairs.Where(vp => vp.IsComplete);
			return new List<ValuePair>(pairs);
		}
		
		/// <summary>
		/// Used during development of the Wikipedia scraping to examine the scraped data
		/// </summary>
		/// <param name="outDir"></param>
		/// <param name="terms"></param>
		public void CompileUniqueTerms(string outDir, Terms terms)
		{
			foreach(Term t in terms)
			{
				List<EValue> evals = new List<Elements.ElementList.EValue>();
				foreach(Element e in this)
				{
					object o = e.Qualities.ValueOf(t.QualityName);
					if (o != null)
					{
						EValue eval = new EValue(t, e, o);
						int ndx = evals.BinarySearch(eval);
						if (ndx < 0) evals.Insert(~ndx, eval);
					}
				}
				if (evals.Count > 0)
				{
					evals.Sort(delegate (EValue ex, EValue ey)
					{
						return Comparer<int>.Default.Compare(ex.Number, ey.Number);
					});
					string fpath = Path.Combine(outDir, t.QualityName + ".txt");
					using (FileStream fs = File.OpenWrite(fpath))
					{
						using (StreamWriter w = new StreamWriter(fs))
						{
							foreach (EValue ev in evals) w.WriteLine(ev.ToString());
						}
					}
					//fpath = Path.Combine(outDir, "Units.txt");
					//Qualities.WriteUnitsCode(fpath);
				}
			}
		}

		/// <summary>
		/// Dump all qualities of all elements into a table.
		/// </summary>
		/// <param name="outDir"></param>
		/// <param name="tblName"></param>
		public void CreateTable(string outDir, string tblName = "")
		{
			if (String.IsNullOrEmpty(tblName)) tblName = "table.txt";
			string fpath = Path.Combine(outDir, tblName);
			StringBuilder s = new StringBuilder();
			s.AppendLine(Qualities.QualityHeader());
			s.AppendLine(Qualities.UnitsHeader());
			foreach(Element e in this)
			{
				s.AppendLine(e.Qualities.Values());
			}
			File.WriteAllText(fpath, s.ToString());
		}

		/// <summary>
		/// Read element qualities from the table
		/// </summary>
		/// <param name="fpath"></param>
		public void ReadTable(string fpath)
		{
			string[] lines = File.ReadAllLines(fpath);
			int nLine = 2;
			foreach(Element e in this)
			{
				string line = lines[nLine++];
				if (!line.StartsWith(e.Name)) throw new Exception(String.Format("Mismatch between element '{0}' and line '{1}'.", e.Name, line));
				Qualities q = new Qualities(e);
				q.ReadValues(line);
			}
		}

		private class EValue : IComparable, IComparable<EValue>
		{
			int _n;
			string _sym, _val;
			bool _isNum;
			double _dval;

			internal EValue(Term t, Element e, object val)
			{
				_n = e.Number;
				_sym = e.Symbol;
				_isNum = t.IsNumeric;
				if (_isNum) _dval = Convert.ToDouble(val);
				else
				{
					if (val is UnitValue)
					{
						UnitValue dv = (UnitValue)val;
						_val = dv.Units;
					}
					else _val = val.ToString();
				}
			}

			internal int Number {  get { return _n; } }

			public override bool Equals(object obj)
			{
				EValue ev = obj as EValue;
				if (ev == null) return false;
				if (_isNum) return Double.Equals(_dval, ev._dval);
				return String.Equals(_val, ev._val);
			}

			public override string ToString()
			{
				string val = _isNum ? _dval.ToString() : _val;
				return String.Format("{0}:\t{1}", _sym, val);
			}

			public override int GetHashCode()
			{
				return ToString().GetHashCode();
			}

			internal int Compare(object obj)
			{
				EValue ev = obj as EValue;
				if (ev == null) return 1;
				if (_isNum != ev._isNum) return 1;	// non-comparable
				if (_isNum) return (_dval > ev._dval) ? 1 : (_dval < ev._dval) ? -1 : 0;
				return String.Compare(_val, ev._val);
			}

			public int CompareTo(object obj)
			{
				return Compare(obj);
			}

			public int CompareTo(EValue other)
			{
				return Compare(other);
			}
		}
	}
}
