﻿using LRReader.Internal;
using LRReader.Shared.Models.Main;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LRReader.UWP.Views.Tabs
{
	public sealed partial class ArchiveEditTab : CustomTab
	{
		private Archive archive;

		private bool loaded;

		public ArchiveEditTab(Archive archive)
		{
			this.InitializeComponent();
			this.archive = archive;
			this.CustomTabId = "Edit_" + archive.title;
		}

		private void TabViewItem_Loaded(object sender, RoutedEventArgs e)
		{
			if (loaded)
				return;
			loaded = true;
			TabContent.LoadArchive(archive);
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			TabContent.Refresh();
		}
	}
}
