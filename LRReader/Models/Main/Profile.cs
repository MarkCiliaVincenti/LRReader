﻿using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRReader.Models.Main
{
	public class ServerProfile : ObservableObject
	{
		public int Version { get; set; }
		public string UID { get; set; }
		public string Name { get; set; }
		public string ServerAddress { get; set; }
		public string ServerApiKey { get; set; }
		public List<BookmarkedArchive> Bookmarks { get; set; }

		[JsonIgnore]
		public bool HasApiKey { get => !string.IsNullOrEmpty(ServerApiKey); }

		public ServerProfile()
		{
			UID = Guid.NewGuid().ToString();
			Bookmarks = new List<BookmarkedArchive>();
			Version = 1;
		}

		public void Update()
		{
			RaisePropertyChanged(string.Empty);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class BookmarkedArchive
	{
		public string archiveID;
	}
}
