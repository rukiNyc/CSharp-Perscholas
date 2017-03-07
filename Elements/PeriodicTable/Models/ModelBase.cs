using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PeriodicTable.Models
{
	public abstract class ModelBase : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		protected virtual void RaisePropertyChanged(params string[] propertyNames)
		{
			if (PropertyChanged == null) return;
			foreach (string pname in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(pname));
		}
	}
}
