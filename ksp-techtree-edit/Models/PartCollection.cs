using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KerbalParser;
using ksp_techtree_edit.Util;
using ksp_techtree_edit.ViewModels;

namespace ksp_techtree_edit.Models
{
	public class PartCollection : IList<Part>
	{
		#region Members

		public DirectoryInfo Directory { get; set; }

		private readonly IList<Part> _parts = new List<Part>();

		#endregion Members

		#region Constructors

		public PartCollection()
		{
		}

		public PartCollection(string path)
		{
			Directory = new DirectoryInfo(path);
		}

		#endregion Constructors

		#region Methods

		public event ProgressFileSearch Progress;

		protected virtual void OnProgress()
		{
			var handler = Progress;
			if (handler != null)
                handler(this, EventArgs.Empty);
		}

		public List<KerbalNode> LoadParts()
		{
			var partlist = new List<KerbalNode>();
			var filter = new List<string> { "PART" };
			var parser = new Parser(filter, false, true);
			foreach (var file in Directory.GetFiles("*.cfg", SearchOption.AllDirectories))
			{
				OnProgress();
				KerbalConfig parts = null;
				try
				{
					parts = parser.ParseConfig(file.FullName);
				}
				catch (Exception e)
				{
					Logger.Error(e.Message);
				}
				if (parts == null || parts.Count < 1) continue;
				foreach (var p in parts.Where(p => !partlist.Contains(p) && p.Name == "PART"))
				{
					if (!p.Values.ContainsKey("name"))
                        continue;
					var part = new Part(p.Values["name"].First());
					part.PopulateFromSource(p);
					part.FileName = file.FullName;
					_parts.Add(part);
				}
			}
			Logger.Log("Loaded {0} parts from {1}", _parts.Count, Directory);
			return partlist;
		}

		public void LoadDictionary()
		{
			var dict = TechTreeViewModel.localizationDictionary;
			foreach (var file in Directory.GetFiles("*.cfg", SearchOption.AllDirectories))				
			{
				//Logger.Log("file: " + file.FullName);
				string[] lines = System.IO.File.ReadAllLines(file.FullName);
				bool lfile = false;
				bool en_us = false;
				int brace = 0;
#if true
				foreach (string line in lines)
				{

					if (line.Contains("Localization"))
					{
						lfile = true;
						//Logger.Log("Localization found");
					}
					if (lfile)
					{
						if (line.Contains("en-us"))
						{
							en_us = true;
							//Logger.Log("en-us found");
						}
						if (line.Contains("{") && en_us)
						{
							brace++;
							//Logger.Log("brace found");
						}
						if (line.Contains("}"))
						{
							brace--;
							if (en_us)
								en_us = false;
						}
						if (en_us)
						{
							if (line.Contains('='))
							{
								var pieces = line.Split(new[] { '=' }, 2);

								if (dict.ContainsKey(pieces[0].Trim()))
								{
									Logger.Log("Duplicated key: " + pieces[0]);
								}
								else
									dict.Add(pieces[0].Trim(), pieces[1].Trim());
							}
						}
					}
				}
#endif
			}

#if false
			foreach (var d in dict)
			{
				Logger.Log("key: [" + d.Key + "] = " + d.Value);
			}
#endif
		}
		#endregion Methods

		#region Interface Members

		public IEnumerator<Part> GetEnumerator()
		{
			return _parts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_parts).GetEnumerator();
		}

		public void Add(Part item)
		{
			_parts.Add(item);
		}

		public void Clear()
		{
			_parts.Clear();
		}

		public bool Contains(Part item)
		{
			return _parts.Contains(item);
		}

		public void CopyTo(Part[] array, int arrayIndex)
		{
			_parts.CopyTo(array, arrayIndex);
		}

		public bool Remove(Part item)
		{
			return _parts.Remove(item);
		}

		public int Count
		{
			get { return _parts.Count; }
		}

		public bool IsReadOnly
		{
			get { return _parts.IsReadOnly; }
		}

		public int IndexOf(Part item)
		{
			return _parts.IndexOf(item);
		}

		public void Insert(int index, Part item)
		{
			_parts.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_parts.RemoveAt(index);
		}

		public Part this[int index]
		{
			get { return _parts[index]; }
			set { _parts[index] = value; }
		}

		#endregion Interface Members
	}
}
