﻿using Elements;
using PeriodicTable.Graphs;
using PeriodicTable.Models;
using PeriodicTable.WebView;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PeriodicTable
{
	/// <summary>
	/// Interaction logic for PTable.xaml
	/// </summary>
	public partial class PTable : UserControl
	{
		#region RoutedEvents

		public static readonly RoutedEvent ElementSelectedEvent = EventManager.RegisterRoutedEvent("ElementSelected", RoutingStrategy.Bubble,
			typeof(ElementSelectionRoutedEventHandler), typeof(PTable));

		public static readonly RoutedEvent ElementDeselectedEvent = EventManager.RegisterRoutedEvent("ElementDeselected", RoutingStrategy.Bubble,
			typeof(ElementDeselectionRoutedEventHandler), typeof(PTable));

		#endregion

		public DependencyProperty SelectedElementProperty = DependencyProperty.Register("SelectedElement", typeof(Element), typeof(PTable));

		public PTable()
		{
			InitializeComponent();
			DataContext = new ElementsModel();
			ElementSelected += OnElementSelected;
			ElementDeselected += OnElementDeselected;
			AddHandler(Button.ClickEvent, new RoutedEventHandler(HandleButtonClicked));
		}

		public Element SelectedElement
		{
			get { return (Element)GetValue(SelectedElementProperty); }
			set { SetValue(SelectedElementProperty, value); }
		}

		public event ElementSelectionRoutedEventHandler ElementSelected
		{
			add { AddHandler(ElementSelectedEvent, value); }
			remove { RemoveHandler(ElementSelectedEvent, value); }
		}

		public event ElementDeselectionRoutedEventHandler ElementDeselected
		{
			add { AddHandler(ElementDeselectedEvent, value); }
			remove { RemoveHandler(ElementDeselectedEvent, value); }
		}

		private void OnElementSelected(object sender, ElementSelectedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine($"PTable: Element Selected: {e.Element} - {e.SelectionType}");
			switch (e.SelectionType)
			{
				case ElementSelectionType.Click:
				case ElementSelectionType.DoubleClick:
					ElementWindow w = new ElementWindow();
					w.Element = e.Element;
					PositionWindow(0.05, w);
					e.Handled = true;
					w.ShowDialog();
					return;
			}
		}

		private void PositionWindow(double margin, Window w)
		{
			Window main = Window.GetWindow(this);
			double frc = (1 - 2 * margin);
			w.Width = main.ActualWidth * frc;
			w.Height = main.ActualHeight * frc;
			w.Left = main.Left + margin * main.ActualWidth;
			w.Top = main.Top + margin * main.ActualHeight;
			w.WindowState = WindowState.Normal;
			w.Owner = main;		
		}

		private void OnElementDeselected(object sender, ElementDeselectedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine($"PTable: Element Deselected: {e.DeselectedElement} - {e.DeselectionType}");
		}

		private void HandleButtonClicked(object sender, RoutedEventArgs e)
		{
			Button b = e.OriginalSource as Button;
			if (b?.Name == "_ppButton")
			{
				GraphWindow gw = new GraphWindow();
				PositionWindow(0.05, gw);
				gw.ShowDialog();
			}
		}

	}

	public class PeriodicTableTemplateSelector : DataTemplateSelector
	{
		public DataTemplate Element { get; set; }
		public DataTemplate PeriodLabel { get; set; }
		public DataTemplate GroupLabel { get; set; }
		public DataTemplate ElementCategoryKey { get; set; }
		public DataTemplate FShellPHKey { get; set; }
		public DataTemplate Detail { get; set; }
		public DataTemplate PPButton { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			TableItemModelBase m = item as TableItemModelBase;
			switch(m.EntryType)
			{
				case TableEntryType.Element: return Element;
				case TableEntryType.PeriodLabel: return PeriodLabel;
				case TableEntryType.FamilyLabel: return GroupLabel;
				case TableEntryType.ElementCategoryKey: return ElementCategoryKey;
				case TableEntryType.FShellPlaceholder: return FShellPHKey;
				case TableEntryType.Detail: return Detail;
				case TableEntryType.PPButton: return PPButton;
			}
			return null;
		}
	}
}
