using System;
using System.IO;
using System.Linq;
using KerbalParser;
using ksp_techtree_edit.Util;
using ksp_techtree_edit.ViewModels;

namespace ksp_techtree_edit.Models
{
	public class Part
	{
		#region Members

		private string _fileName;

		public string FileName
		{
			get { return _fileName; }
			set
			{
				if (value == _fileName) return;
				_fileName = value;
                try
                {
                    ModName = FindModName();
                }
                catch
                {
                    ModName = "Unknown";
                    Logger.Error("Couldn't auto-detect mod for {0}", value);
                }
            }
        }

		public string PartName { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Cost { get; set; }
		public string TechRequired { get; set; }
		public string Category { get; set; }
		public string Icon { get; set; }
		public string ModName { get; set; }

		#endregion Members

		#region Constructors

		public Part()
		{
		}

		public Part(string name)
		{
			PartName = name;
		}

        public Part( Part p)
        {
            PartName = p.PartName;
            Title = p.Title;

			if (p.Description != null && p.Description != "" && TechTreeViewModel.localizationDictionary != null)
			{
				string str = p.Description.Trim();
				if (str.IndexOf(' ') >= 0)
					str = str.Substring(0, str.IndexOf(' '));
				if (TechTreeViewModel.localizationDictionary.ContainsKey(str))
				{
					TechTreeViewModel.localizationDictionary.TryGetValue(str, out string data);
					Description = data;
				}
				else
					Description = str;
			}
			else
				Description = p.Description;

			Cost = p.Cost;
            TechRequired = p.TechRequired;
            Category = p.Category;
            Icon = p.Icon;
            ModName = p.ModName;
            _fileName = p.FileName;
        }

#endregion Constructors

#region Members

        public void PopulateFromSource(KerbalNode node)
		{
			var v = node.Values;

			if (PartName == null) PartName = v["name"].First();

			if (v.ContainsKey("title"))
			{
				Title = v["title"].First();
			}

			if (v.ContainsKey("description"))
			{
				Description = v["description"].First();
			}

			if (v.ContainsKey("cost"))
			{
				int c;
				Int32.TryParse(v["cost"].First(), out c);
				Cost = c;
			}

			if (v.ContainsKey("TechRequired"))
			{
				TechRequired = v["TechRequired"].First();
			}

			if (v.ContainsKey("category"))
			{
				Category = v["category"].First();
			}
		}

        // TODO Beter find Mod name (.version file...)
		private string FindModName()
		{
			var di = new DirectoryInfo(FileName);
			while (true)
			{
				if (di.Parent != null)
				{
					if (di.Parent.Name == "GameData")
                    {
                        return di.Name;
                    }                        
					di = di.Parent;
					continue;
				}
				break;
			}
			return "Unknown";
		}

#endregion Members
	}
}
