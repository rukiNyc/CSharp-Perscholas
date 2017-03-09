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
using NOAA.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace SavingState.Model
{
	public class UtilityViewModel : ModelBase
	{
		private static readonly string NewStationFilePath = Path.Combine(Environment.CurrentDirectory, "NewStations.txt");
		private RelayCommand<string> _loadCmd;
		private ProgressModel _progress;
		private Func<string, bool> _confirmation;

		public UtilityViewModel()
		{
			_loadCmd = new RelayCommand<string>(ExecuteLoad, CanExecuteLoad);
		}

		public ICommand LoadCommand => _loadCmd;

		public ProgressModel Progress
		{
			get { return _progress; }
			private set
			{
				_progress = value;
				RaisePropertyChanged(nameof(Progress));
			}
		}

		public void SetConfirmation(Func<string,bool> confirmation)
		{
			_confirmation = confirmation;
			_loadCmd.RaiseCanExecuteChanged();
		}

		private bool IsRunning { get; set; }

		private bool CanExecuteLoad(string parameter)
		{
			return _confirmation != null && !IsRunning;
		}

		private void ExecuteLoad(string parameter)
		{
			LoadStations();
			//TestProgress();
		}

		//private async void TestProgress()
		//{
		//	Progress = new ProgressModel(true, "----");
		//	IsRunning = true;
		//	await Task.Delay(2000);
		//	for(int i=1;i<=1000;++i)
		//	{
		//		await Task.Delay(50);
		//		if (i == 1)
		//		{
		//			_progress.InitializeCounting(1000);
		//		}
		//		Progress.Value = i;
		//	}
		//	IsRunning = false;
		//	_loadCmd.RaiseCanExecuteChanged();
		//}

		private async void LoadStations()
		{
			if (File.Exists(NewStationFilePath))
			{
				if (!_confirmation($"Overwrite existing Station file?")) return;
			}
			Progress = new ProgressModel(true, "---- Connecting ----");
			IsRunning = true;
			const int batchSize = 500;
			int offset = 1, totalFetched = 0;
			while (true)
			{
				var stations = await Station.Load(offset, batchSize);
				if (offset == 1)
				{
					Progress.InitializeCounting(stations.Metadata.ResultSet.Count);
				}
				SaveStations(stations.Results);
				totalFetched += stations.Results.Length;
				Progress.Value = totalFetched;
				if (totalFetched >= stations.Metadata.ResultSet.Count) break;
				offset += batchSize;
			}
			Progress.Value = Progress.Maximum;
			IsRunning = false;
			_loadCmd.RaiseCanExecuteChanged();
		}

		private void SaveStations(IEnumerable<Station> stations)
		{
			StringBuilder output = new StringBuilder();
			foreach(Station s in stations) output.AppendLine(s.AsTabbed);
			File.AppendAllText(NewStationFilePath, output.ToString());
		}
	}
}
