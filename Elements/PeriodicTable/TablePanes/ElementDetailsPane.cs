﻿using Base;
using Elements;
using PeriodicTable.Models;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Windows.Threading;

namespace PeriodicTable
{
	public class ElementDetailsPane : Control
	{
		static ElementDetailsPane()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementDetailsPane), new FrameworkPropertyMetadata(typeof(ElementDetailsPane)));
		}

		// Prevent flicker by delaying hover-deselections:
		private DispatcherTimer _elTimer = new DispatcherTimer();

		public ElementDetailsPane()
		{
			_elTimer.Interval = TimeSpan.FromMilliseconds(50);
			_elTimer.Tick += _elTimer_Tick;
		}

		private void HandleElementSelected(object sender, ElementSelectedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine($"ElementDetailsPane: Element Selected: {e.Element} - {e.SelectionType}");
			_elTimer.Stop();
			Model.Element = e.Element;
		}

		private void HandleElementDeselected(object sender, ElementDeselectedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine($"ElementDetailsPane: Element Deselected: {e.DeselectedElement} - {e.DeselectionType}");
			_elTimer.Start();
		}

		private void _elTimer_Tick(object sender, EventArgs e)
		{
			Model.Element = null;
		}

		private ElementDetailsModel Model => (ElementDetailsModel)DataContext;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			DataContext = new ElementDetailsModel();
			DependencyObject d = this;
			while(d != null)
			{
				d = VisualTreeHelper.GetParent(d);
				if (d is PTable)
				{
					PTable pt = (PTable)d;
					pt.ElementSelected += HandleElementSelected;
					pt.ElementDeselected += HandleElementDeselected;
				}
			}
		}

		public class ElementDetailsModel : ModelBase, PropertyValue.IPropertySource
		{
			private Element _element;
			private Visibility _visibility = Visibility.Collapsed;

			public Visibility Visibility => _visibility;

			public Element Element
			{
				get { return _element; }
				set
				{
					_element = value;
					_visibility = _element == null ? Visibility.Collapsed : Visibility.Visible;
					RaisePropertyChanged("Element", "Visibility", "Background");
					UpdatePropertyValues();
				}
			}

			public IEnumerable<PropertyValue> PropertyValues { get; private set; }

			public IEnumerable<PropertyValue> PropertyValues2 { get; private set; }

			public Brush Background
			{
				get
				{
					return (_element == null) ?
						Brushes.Transparent : ElementModel.ColorForCategory(_element.Qualities.ElementCategory);
				}
			}

			private void UpdatePropertyValues()
			{
				PropertyValues = (_element == null) ? null : PropertyValue.CreatePropertyValues(this, "AtomicNumber", "AtomicWeight", "BoilingPoint", "MeltingPoint", "Density",
					"OxidationStates", "Electronegativity");
				PropertyValues2 = (_element == null) ? null : PropertyValue.CreatePropertyValues(this, "ElectronConfiguration", "CovalentRadius", "VanDerWaalsRadius", "CrystalStructure", "SpeedOfSound",
					"ThermalConductivity", "ElectricalResistivity", "MagneticOrdering", "Discovery");

				RaisePropertyChanged("PropertyValues", "PropertyValues2");			}

			string PropertyValue.IPropertySource.GetProperty(string propertyName)
			{
				if (_element == null) return String.Empty;
				return _element.Qualities.GetValue(propertyName);
			}
		}
	}
}
