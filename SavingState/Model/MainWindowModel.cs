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
using SavingState.Properties;
using System;
using System.IO;
using System.Windows;

namespace SavingState.Model
{
	public class MainWindowModel : ModelBase
	{
		public static string GetApplicationDataPath(string fileName)
		{
			return Path.Combine(ApplicationDataFolder, fileName);
		}

		public static readonly string ApplicationDataFolder;

		static MainWindowModel()
		{
			string exeName = Path.GetFileNameWithoutExtension(typeof(MainWindowModel).Assembly.Location);
			ApplicationDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), exeName);
			if (!Directory.Exists(ApplicationDataFolder)) Directory.CreateDirectory(ApplicationDataFolder);
		}

		private ApiExplorerViewModel _apiExplorerModel = new ApiExplorerViewModel();
		private StationViewModel _dataModel = new StationViewModel();
		private UtilityViewModel _utilityModel = new UtilityViewModel();

		public ApiExplorerViewModel ApiExplorerModel => _apiExplorerModel;
		public StationViewModel DataModel => _dataModel;
		public UtilityViewModel UtilityModel => _utilityModel;

		public WindowState WindowState
		{
			get { return Settings.Default.MainWindowState; }
			set
			{
				Settings.Default.MainWindowState = value;
				Settings.Default.Save();
			}
		}

		public int CurrentTab
		{
			get { return Settings.Default.MainWindowTab; }
			set
			{
				Settings.Default.MainWindowTab = value;
				Settings.Default.Save();
			}
		}

		// Cannot bind RestoreBounds because it is not a DependencyProperty.
		public void SetBounds(Rect bounds)
		{
			Settings.Default.MainWindowBounds = bounds;
			Settings.Default.Save();
		}
	}
}
