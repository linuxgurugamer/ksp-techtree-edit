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
                    Logger.Error("Part.Filename: " + "Couldn't auto-detect mod for {0}", value);
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

		string GetLocalized(string str)
		{
			if (str != null &&str != "" && TechTreeViewModel.localizationDictionary != null)
			{
				 str = str.Trim();
				for (int i = 0; i < str.Length; i++)
				{
					if (Char.IsWhiteSpace(str[i]))
					{
						str = str.Substring(0, i);
						break;
					}
				}

				if (TechTreeViewModel.localizationDictionary.ContainsKey(str))
				{
					TechTreeViewModel.localizationDictionary.TryGetValue(str, out string data);
					return data;
				}
				else
					return str;
			}
			else
				return str;
		}
        public Part( Part p)
        {
            PartName = p.PartName;
            Title = p.Title;
			Title = GetLocalized(p.Title);
			Description = GetLocalized(p.Description);

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
