using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PeriodicTable
{
	public class PropertyValue
	{
		public interface IPropertySource
		{
			string GetProperty(string propertyName);
		}

		private static string DePascal(string name)
		{
			StringBuilder s = new StringBuilder();
			foreach(char c in name)
			{
				if ((s.Length > 0) && Char.IsUpper(c)) s.Append(" ");
				s.Append(c);
			}
			return s.ToString();
		}

		public static List<PropertyValue> CreatePropertyValues(IPropertySource propertySource, params string[] propertyNames)
		{
			if (propertySource == null) throw new ArgumentNullException(nameof(propertySource));
			List<PropertyValue> r = new List<PropertyValue>(propertyNames.Length);
			foreach(string n in propertyNames) r.Add(new PropertyValue(propertySource, n));
			return r;
		}

		public PropertyValue(IPropertySource propertySource, string propertyName)
		{
			if (propertySource == null) throw new ArgumentNullException(nameof(propertySource));
			PropertyName = DePascal(propertyName);
			Value = propertySource.GetProperty(propertyName);
		}

		public PropertyValue(string name, object value)
		{
			PropertyName = DePascal(name);
			Value = value;
		}

		public string PropertyName { get; private set; }
		public object Value { get; private set; }

		public override string ToString()
		{
			return $"{PropertyName}: {Value}";
		}
	}

	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:PeriodicTable"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:PeriodicTable;assembly=PeriodicTable"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:PropertyValueView/>
	///
	/// </summary>
	public class PropertyValueView : Control
	{
		static PropertyValueView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyValueView), new FrameworkPropertyMetadata(typeof(PropertyValueView)));
		}
	}
}
