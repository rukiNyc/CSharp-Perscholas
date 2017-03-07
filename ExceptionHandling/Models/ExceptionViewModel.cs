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
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Threading;

namespace ExceptionHandling.Models
{
	public class ExceptionViewModel : ModelBase
	{
		private List<ExceptionHandlerModel> _handlerModels = new List<ExceptionHandlerModel>();
		private RelayCommand<string> _startCmd;
		private Action<Action> _invoker;
		private Thread _runner;
		private bool _clearAll;
		public ExceptionViewModel(int handlerCount, Action<Action> invoker)
		{
			if (invoker == null) throw new ArgumentNullException(nameof(invoker));
			_invoker = invoker;
			CreateHandlers(handlerCount);
			_startCmd = new RelayCommand<string>(Execute, CanExecute);
			ClearAllHandlers = true;
			CallStack = String.Empty;
		}

		public IEnumerable<ExceptionHandlerModel> Handlers => _handlerModels;

		public ICommand StartStopCommand => _startCmd;

		public bool IsRunning => _runner != null;

		public bool ClearAllHandlers
		{
			get { return _clearAll; }
			set
			{
				_clearAll = value;
				_handlerModels.ForEach((m) => m.CatchExceptions = _clearAll);
			}
		}

		public string StartStopButtonLabel => IsRunning ? "Stop" : "Start";

		public string CallStack { get; private set; }

		public string UncaughtError { get; private set; }

		private void CreateHandlers(int handlerCount)
		{
			ExceptionHandlerModel lastM = null;
			List<ExceptionHandlerModel> handlers = new List<Models.ExceptionHandlerModel>();
			for (int i = handlerCount; i > 0; --i)
			{
				ExceptionHandlerModel ehm = new Models.ExceptionHandlerModel(i, this, lastM);
				_handlerModels.Add(ehm);
				lastM = ehm;
			}
			_handlerModels.Reverse();
		}

		private bool CanExecute(string cp)
		{
			switch (cp)
			{
				case "startstop":	return true;
				case "throw": return IsRunning;
				case "reset": return !IsRunning && !String.IsNullOrEmpty(CallStack);
			}
			return false;
		}

		private void Execute(string cp)
		{
			switch(cp)
			{
				case "startstop": Start(); break;
				case "throw":
					_handlerModels.First((hm) => hm.IsRunning).Throw = true;
					break;
				case "reset":
					SetCallStack(String.Empty);
					_handlerModels.ForEach(hm => hm.ResetRun());
					UncaughtError = String.Empty;
					RaisePropertyChanged(nameof(UncaughtError));
					break;
			}
		}

		private void Start()
		{
			if (IsRunning)
			{
				EndRun();
				return;
			}
			ThreadStart ts = new ThreadStart(() => {
				try
				{
					_handlerModels.First().Run();
				}
				catch(Exception ex)
				{
					UncaughtError = $"Last Chance Handler says: {ex.Message}";
				}
				System.Diagnostics.Debug.WriteLine("invoking ....");
				_invoker(() =>
				{
					_runner = null;
					RaisePropertyChanged(nameof(IsRunning), nameof(StartStopButtonLabel), nameof(UncaughtError));
					_startCmd.RaiseCanExecuteChanged();
				});
			});
			_handlerModels.ForEach((hm) => hm.ResetRun());
			_runner = new Thread(ts);
			_runner.IsBackground = true;
			_runner.Start();
			RaisePropertyChanged("IsRunning", "StartStopButtonLabel");
			_startCmd.RaiseCanExecuteChanged();
		}

		private void EndRun()
		{
			_handlerModels.Last().QuitLoop = true;
		}

		internal void SetCallStack(string callStack)
		{
			CallStack = callStack;
			_invoker(() => RaisePropertyChanged(nameof(CallStack)));
			_invoker(() => _startCmd.RaiseCanExecuteChanged());
		}

		internal void Invoke(Action a)
		{
			_invoker(a);
		}
	}
}
