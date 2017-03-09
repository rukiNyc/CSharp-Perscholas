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

using CommonTools.Lib.MVVM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NOAA.Lib.CDO;
using NOAA.Lib.DataModel;
using SavingState.Model.Plots;
using SavingState.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SavingState.Model
{
	public class StationViewModel : ModelBase
	{
		private static readonly List<Station> _allStations;
		private static List<string> _allCountries;
		private static List<int> _allYears;

		static StationViewModel()
		{
			_allStations = Station.LoadFromFile();
			int yMin = _allStations.Min(s => s.MinDate.Year), yMax = _allStations.Max(s => s.MaxDate.Year);
			_allYears = new List<int>();
			for (int i = yMin; i <= yMax; ++i) _allYears.Add(i);
			_allYears.Reverse();
			_allCountries = new List<string>();
			_allStations.ForEach(s =>
			{
				int n = _allCountries.BinarySearch(s.Country);
				if (n < 0) _allCountries.Insert(~n, s.Country);
			});
		}

		private string _selectedCountry;
		private IEnumerable<Station> _availableStations;
		private Station _selectedStation;
		private IEnumerable<int> _availableYears;
		private int _year;
		private string _output, _url;
		private TemperaturePlotViewModel _tpvModel;

		public StationViewModel()
		{
			_selectedCountry = Settings.Default.SelectedCountry;
			_selectedStation = _allStations.FirstOrDefault(s => s.Id == Settings.Default.SelectedStationId);
			_year = Settings.Default.SelectedYear;
			_tpvModel = new TemperaturePlotViewModel();
			if (!String.IsNullOrEmpty(_selectedCountry) && (_selectedStation != null) && _year > 0)
			{
				_availableStations = _allStations.Where(s => s.Country == _selectedCountry);
				int minYear = _selectedStation.MinDate.Year, maxYear = _selectedStation.MaxDate.Year;
				_availableYears = _allYears.Where(y => y >= minYear && y <= maxYear);
				ApplyYear();
			}
			else SelectedCountry = _allCountries.First();
		}

		public IEnumerable<string> Countries => _allCountries;

		public string SelectedCountry
		{
			get { return _selectedCountry; }
			set
			{
				_selectedCountry = value;
				RaisePropertyChanged(nameof(SelectedCountry));
				ApplyCountry();
			}
		}

		public IEnumerable<Station> Stations => _availableStations;

		public Station SelectedStation
		{
			get { return _selectedStation; }
			set
			{
				_selectedStation = value;
				RaisePropertyChanged(nameof(SelectedStation));
				if (_selectedStation != null) ApplyStation();
			}
		}

		public IEnumerable<int> Years => _availableYears;

		public int SelectedYear
		{
			get { return _year; }
			set
			{
				_year = value;
				RaisePropertyChanged(nameof(SelectedYear));
				ApplyYear();
			}
		}

		public string Output
		{
			get { return _output; }
			private set
			{
				_output = value;
				RaisePropertyChanged(nameof(Output));
			}
		}

		public string Url
		{
			get { return _url; }
			set
			{
				_url = value;
				RaisePropertyChanged(nameof(Url));
			}
		}

		public object TemperaturePlot => _tpvModel;

		public string StatusMessage { get; private set; }
		public Brush StatusColor { get; private set; }

		private void ApplyCountry()
		{
			_availableStations = _allStations.Where(s => s.Country == SelectedCountry);
			RaisePropertyChanged(nameof(Stations));
			SelectedStation = _availableStations.FirstOrDefault();
		}

		private void ApplyStation()
		{
			int minYear = SelectedStation.MinDate.Year, maxYear = SelectedStation.MaxDate.Year;
			_availableYears = _allYears.Where(y => y >= minYear && y <= maxYear);
			RaisePropertyChanged(nameof(Years));
			if (_availableYears.Count() > 0)
			{
				SelectedYear = _availableYears.First();
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("No Years");
			}
			
		}

		private async void ApplyYear()
		{
			Output = String.Empty;
			SetStatus("Request Pending ....", null);
			string ext = $"data?datasetid=GHCND&datatypeid=TMAX,TMIN&stationid={SelectedStation.Id}&startdate={SelectedYear}-01-01&enddate={SelectedYear}-12-31&limit=1000";
			Url = ext;
			DateTime start = DateTime.Now;
			try
			{
				string json = await CDOService.StartRequest(ext);
				Output = JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);
				SetStatus($"Request completed in {(DateTime.Now - start).TotalSeconds.ToString("F3")} seconds.");
				SaveState();
				UpdatePlot(json);
			}
			catch(Exception ex)
			{
				SetStatus(ex.Message, true);
			}
		}

		private void SetStatus(string msg, bool? isError = false)
		{
			StatusMessage = msg;
			if (!isError.HasValue) StatusColor = Brushes.Blue;
			else
				StatusColor = isError.Value ? Brushes.Red : Brushes.Green;
			RaisePropertyChanged(nameof(StatusMessage), nameof(StatusColor));
		}

		private void SaveState()
		{
			Settings.Default.SelectedCountry = SelectedCountry;
			Settings.Default.SelectedStationId = SelectedStation.Id;
			Settings.Default.SelectedYear = SelectedYear;
			Settings.Default.Save();
		}

		private void UpdatePlot(string json)
		{
			try
			{
				Loader<StationValue> svs = JsonConvert.DeserializeObject<Loader<StationValue>>(json);
				_tpvModel.SetStationData(svs.Results, _year);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

	}
}
