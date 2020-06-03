﻿using LRReader.Internal;
using LRReader.Shared.Models.Main;
using LRReader.Shared.Providers;
using LRReader.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LRReader.UWP.Views.Tabs.Content
{
	public sealed partial class ArchivesTabContent : UserControl
	{

		private ArchivesPageViewModel Data;

		private bool loaded;

		private string query = "";

		public ArchivesTabContent()
		{
			this.InitializeComponent();
			Data = DataContext as ArchivesPageViewModel;
		}

		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (loaded)
				return;
			loaded = true;
			Data.ControlsEnabled = false;
			Data.LoadBookmarks();
			await HandleSearch();
			Data.ControlsEnabled = true;
		}

		private void ArchivesGrid_ItemClick(object sender, ItemClickEventArgs e) => Global.EventManager.AddTab(new ArchiveTab(e.ClickedItem as Archive), true);

		private void Button_Click(object sender, RoutedEventArgs e) => Refresh();

		public async void SearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				Data.Suggestions.Clear();
				if (!string.IsNullOrEmpty(sender.Text))
				{
					string text;
					var sQuery = sender.Text.ToUpper();
					if (sender.Text.Length > query.Length)
					{
						text = sQuery.Substring(query.Length).TrimStart();
					}
					else
					{
						text = sQuery.Split(" ").Last();
						query = sender.Text.Substring(0, Math.Max(0, sQuery.LastIndexOf(" ")));
					}
					foreach (var t in Global.ArchivesManager.TagStats.Where(t => t.GetNamespacedTag().ToUpper().Contains(text)))
					{
						Data.Suggestions.Add(query.TrimEnd() + (string.IsNullOrEmpty(query) ? "" : " ") + t.GetNamespacedTag());
					}
				}
				else
				{
					query = "";
					await HandleSearch();
				}
			}
		}

		public async void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			if (args.ChosenSuggestion != null)
			{
				query = args.ChosenSuggestion as string;
				await HandleSearch();
			}
			else
			{
				query = args.QueryText;
				await HandleSearch();
			}
		}

		private void RandomButton_Click(object sender, RoutedEventArgs e)
		{
			var random = new Random();
			var list = Global.ArchivesManager.Archives;
			var item = list.ElementAt(random.Next(list.Count() - 1));
			Global.EventManager.AddTab(new ArchiveTab(item));
		}

		private async void FilterToggle_Click(object sender, RoutedEventArgs e) => await Data.ReloadSearch();

		private void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
		{
			using (var deferral = args.GetDeferral())
			{
				Refresh();
			}
		}

		private async void Refresh_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
		{
			await HandleSearch();
			args.Handled = true;
		}

		private void RefreshFull_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
		{
			Refresh();
			args.Handled = true;
		}

		private async void PrevButton_Click(object sender, RoutedEventArgs e) => await Data.PrevPage();

		private async void NextButton_Click(object sender, RoutedEventArgs e) => await Data.NextPage();

		private async Task HandleSearch()
		{
			Data.Query = query;
			await Data.ReloadSearch();
		}

		public async void Refresh()
		{
			Data.ControlsEnabled = false;
			await Data.Refresh();
			await HandleSearch();
			Data.ControlsEnabled = true;
		}

		private async void ArchivesGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var pointerPoint = e.GetCurrentPoint(ArchivesGrid);
			if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
			{
				if (pointerPoint.Properties.IsXButton1Pressed)
				{
					await Data.PrevPage();
				}
				else if (pointerPoint.Properties.IsXButton2Pressed)
				{
					await Data.NextPage();
				}
			}
		}

	}
}
