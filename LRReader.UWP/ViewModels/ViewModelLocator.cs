﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using LRReader.Internal;
using Windows.Storage;

namespace LRReader.ViewModels
{
	public class ViewModelLocator
	{
		public ViewModelLocator()
		{

			Global.Init(); // Init global static data

			SimpleIoc.Default.Register<ArchivesPageViewModel>();
			SimpleIoc.Default.Register<SettingsPageViewModel>();
			SimpleIoc.Default.Register<HostTabPageViewModel>();
		}

		public ArchivesPageViewModel ArchivesPageInstance
		{
			get => SimpleIoc.Default.GetInstance<ArchivesPageViewModel>();
		}
		public SettingsPageViewModel SettingsPageInstance
		{
			get => SimpleIoc.Default.GetInstance<SettingsPageViewModel>();
		}
		public HostTabPageViewModel HostTabPageInstace
		{
			get => SimpleIoc.Default.GetInstance<HostTabPageViewModel>();
		}
	}
}
