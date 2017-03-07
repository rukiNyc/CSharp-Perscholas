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
using System;
using System.Threading;
using System.Windows.Media;

namespace ExceptionHandling.Models
{
	public class ExceptionHandlerModel : ModelBase
	{
		private bool _catchExceptions = false, _isWaiting = false, _isRunning = false, _quitLoop;
		private string _output = String.Empty;
		private ExceptionViewModel _parentModel;

		public ExceptionHandlerModel(int index, ExceptionViewModel parentModel, ExceptionHandlerModel nextModel)
		{
			_parentModel = parentModel;
			Index = index;
			NextModel = nextModel;
		}

		public int Index { get; private set; }

		public bool Throw { get; set; }

		public string Label => $"Handler {Index}";

		public Brush Background { get { return IsRunning ? Brushes.Red : IsWaiting ? Brushes.Blue :Brushes.SlateGray; } }

		public bool QuitLoop
		{
			get { return _quitLoop; }
			set
			{
				_quitLoop = value;
				ThreadedPropertyChanged(nameof(QuitLoop));
			}
		}

		public bool IsRunning
		{
			get { return _isRunning; }
			private set
			{
				_isRunning = value;
				ThreadedPropertyChanged(nameof(IsRunning), nameof(Background));
			}
		}

		public bool IsWaiting
		{
			get { return _isWaiting; }
			private set
			{
				_isWaiting = value;
				RaisePropertyChanged(nameof(IsWaiting),nameof(Background));
			}
		}

		public bool CatchExceptions
		{
			get { return _catchExceptions; }
			set
			{
				_catchExceptions = value;
				RaisePropertyChanged(nameof(CatchExceptions));
			}
		}

		public bool LoopAfterCatch { get; set; }

		public string Output
		{
			get { return _output; }
			private set
			{
				_output = value;
				ThreadedPropertyChanged(nameof(Output));
			}
		}

		/// <summary>
		/// Gracefully terminate any running loop
		/// </summary>
		public void ResetRun()
		{
			QuitLoop = false;
			Throw = false;
			IsWaiting = false;
			IsRunning = false;
			Output = String.Empty;
		}

		/// <summary>
		/// If this is the last model of the chain, run the loop.
		/// Otherwise, delegate to the next model and wait for an exception.
		/// </summary>
		public void Run()
		{
			try
			{
				if (NextModel == null)
				{
					RunLoop();
				}
				else
				{
					IsWaiting = true;
					NextModel.Run();
				}
			}
			catch(Exception ex)
			{
				if (!CatchExceptions)
				{
					AppendOutput($"I see the exception, but can't handle it....");
					IsWaiting = false;
					IsRunning = false;
					throw;
				}
				AppendOutput($"Handler {Index} caught this:  {ex.Message}");
				if (LoopAfterCatch) RunLoop(false);
			}
			finally
			{
				IsWaiting = false;
			}
		}

		private void RunLoop(bool clearOutput = true)
		{
			if (clearOutput) Output = "Beginning Loop."; else AppendOutput("Beginning Loop.");
			IsRunning = true;
			TheActualLoop();
		}

		private void TheActualLoop()
		{
			_parentModel.SetCallStack(Environment.StackTrace);
			int nLoop = 1;
			while(!QuitLoop)
			{
				AppendOutput($"Loop #{nLoop++}");
				Thread.Sleep(500);
				if (Throw)
				{
					IsRunning = false;
					throw new LoopException(Index);
				}
			}
			AppendOutput("Ending loop at your request :)");
		}

		private void AppendOutput(string newOutput)
		{
			if (String.IsNullOrEmpty(Output)) Output = newOutput;
			else
				Output = String.Concat(Output, "\n", newOutput);
			ThreadedPropertyChanged(nameof(Output));
		}

		private ExceptionHandlerModel NextModel { get; set; }

		public override string ToString()
		{
			return $"ExceptionHandlerModel {Index}";
		}

		private void ThreadedPropertyChanged(params string[] propertyNames)
		{
			_parentModel.Invoke(() =>
			{
				RaisePropertyChanged(propertyNames);
			});
		}
	}
}
