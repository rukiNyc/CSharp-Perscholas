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
using Newtonsoft.Json.Linq;
using NOAA.Lib.CDO;
using SavingState.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace SavingState.Model
{
	public class ApiExplorerViewModel : ModelBase
	{
		private const string JSON_Output = "ApiExplorerOutput.json";
		private static string[] _endpoints = new string[] { "Datasets", "Data Categories", "Datatypes", "Location Categories", "Locations", "Stations", "Data" };

		private string _selectedEnpoint, _parameters, _url;
		private RelayCommand<string> _requestCommand;
		private StatusModel _status;
		private string _output;
		private PagingControlModel _pagingModel;

		public ApiExplorerViewModel()
		{
			_requestCommand = new RelayCommand<string>(MakeRequest, CanMakeRequest);
			_status = new StatusModel();
			_pagingModel = new PagingControlModel();
			IncludePaging = Settings.Default.ApiEx_IncludePaging;
			string lastEp = Settings.Default.ApiEx_Endpoint;
			if (string.IsNullOrEmpty(lastEp)) lastEp = _endpoints.First();
			SelectedEndpoint = lastEp;
			Parameters = Settings.Default.ApiEx_Parameters;
			Output = LoadLastOutput();
			_pagingModel.PropertyChanged += _pagingModel_PropertyChanged;
		}

		public ICommand RequestCommand => _requestCommand;

		public IEnumerable<string> Endpoints => _endpoints;

		public bool IncludePaging { get; set; }

		public string SelectedEndpoint
		{
			get { return _selectedEnpoint; }
			set
			{
				_selectedEnpoint = value;
				RaisePropertyChanged(nameof(SelectedEndpoint));
				UpdateUrl();
			}
		}

		public string Parameters
		{
			get { return _parameters; }
			set
			{
				_parameters = value;
				RaisePropertyChanged(nameof(Parameters));
				_requestCommand.RaiseCanExecuteChanged();
				UpdateUrl();
			}
		}

		public string Url
		{
			get { return _url; }
			private set
			{
				_url = value;
				RaisePropertyChanged(nameof(Url));
			}
		}

		public StatusModel Status => _status;

		public string Output
		{
			get { return _output; }
			private set
			{
				_output = value;
				RaisePropertyChanged(nameof(Output));
				try
				{
					if (String.IsNullOrEmpty(_output) || _output == "{}") _pagingModel.Reset(); else
						_pagingModel.ApplyResultSet(CDOService.ExtractResultSet(value));
				}
				catch { }
			}
		}

		public PagingControlModel Paging => _pagingModel;

		private bool CanMakeRequest(string p)
		{
			return _status.IsSuccess.HasValue;
		}

		private void MakeRequest(string p)
		{
			DoRequest();
		}

		private async void DoRequest()
		{
			_output = String.Empty;
			RaisePropertyChanged(nameof(Output));
			Status.Message = "Request is Pending ....";
			Status.IsSuccess = null;
			_requestCommand.RaiseCanExecuteChanged();
			DateTime start = DateTime.Now;
			try
			{
				string json = await CDOService.StartRequestUrl(Url);
				Output = JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);
				Status.IsSuccess = true;
				Status.Message = $"Request completed in {(DateTime.Now - start).TotalSeconds.ToString("F2")} seconds";
				Settings.Default.ApiEx_Endpoint = _selectedEnpoint;
				Settings.Default.ApiEx_Parameters = _parameters;
				Settings.Default.Save();
				SaveLastOutput();
			}
			catch (Exception ex)
			{
				Status.IsSuccess = false;
				Status.Message = ex.Message;
			}
			_requestCommand.RaiseCanExecuteChanged();
		}

		private void UpdateUrl()
		{
			string endpoint = _selectedEnpoint.Replace(" ", string.Empty).ToLower();
			string p = Parameters;
			if (p == null) p = String.Empty;
			if (p.StartsWith("?")) p = p.Substring(1);  // remove '?' if user typed it
			if (IncludePaging)
			{
				p = CDOService.ApplyPaging(p, _pagingModel.Offset, _pagingModel.Limit);
			}
			Url = string.Concat(CDOService.BasePath, endpoint, "?", p);
		}

		private void SaveLastOutput()
		{
			string fpath = MainWindowModel.GetApplicationDataPath(JSON_Output);
			File.WriteAllText(fpath, Output);
		}

		private string LoadLastOutput()
		{
			string fpath = MainWindowModel.GetApplicationDataPath(JSON_Output);
			if (File.Exists(fpath)) return File.ReadAllText(fpath);
			return String.Empty;
		}

		private void _pagingModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(_pagingModel.Offset):
				case nameof(_pagingModel.Limit):
					UpdateUrl();
					break;
			}
		}
	}
}
