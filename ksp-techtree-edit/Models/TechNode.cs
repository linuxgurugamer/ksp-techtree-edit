using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using KerbalParser;
using ksp_techtree_edit.Util;

namespace ksp_techtree_edit.Models
{
	public class TechNode
	{
		#region Members

		public string NodePart { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Cost { get; set; }
		public Point Pos { get; set; }

		private int _zlayer;

		public int Zlayer
		{
			get { return _zlayer; }
			set { _zlayer = value.Clamp(-24, -1); }
		}

		public string Id { get; set; }

        
		public string Icon { get; set; }
		public bool AnyToUnlock { get; set; }
		public bool HideEmpty { get; set; }
		public bool HideIfNoBranchParts { get; set; }

        public double Scale { get; set; }

		public List<TechNode> Parents { get; set; }

		public List<string> Parts { get; set; }

		#endregion Members

		#region Constructors

		public TechNode()
		{
		}

		public TechNode(string name)
		{
            NodePart = name;
			Title = String.IsNullOrEmpty(name) ? String.Empty : char.ToUpper(name[0]) + name.Substring(1);
			Description = "";
			Cost = 0;
			Pos = new Point(0, 0);
			Zlayer = 0;
			Id = GenerateTechId();
			Icon = "RDicon_generic";
            AnyToUnlock = false;
			HideEmpty = false;
			HideIfNoBranchParts = false;
            Scale = 0.6;
			Parents = new List<TechNode>();
			Parts = new List<string>();
		}

        #endregion Constructors

        #region Methods

        // Here if you add a new treetype
        public void PopulateFromSource( KerbalNode sourceNode, TreeType treeType = TreeType.YongeTech)
		{
            switch(treeType)
            {
                case TreeType.YongeTech:
                    PopulateYongeTechNode(sourceNode);
                break;
            }			
		}

        private void PopulateYongeTechNode(KerbalNode sourceNode)
        {
            var v = sourceNode.Values;
            NodePart = v.ContainsKey("nodepart") ? v["nodepart"].First() : "";

            double x;
            double y;
            Id = v.ContainsKey("id") ? v["id"].First() : "";

            if (v.ContainsKey("pos"))
            {
                var posString = v["pos"].First();
                var coordinates = posString.Split(',');

                if (coordinates.Length >= 2)
                {
                    if (!Double.TryParse(coordinates[0], out x))
                    {
                        x = 0;
                    }

                    if (!Double.TryParse(coordinates[1], out y))
                    {
                        y = 0;
                    }
                    Pos = new Point(x, y);

                    decimal z;
                    if (!Decimal.TryParse(coordinates[2], out z))
                    {
                        Zlayer = -1;
                    }
                    Zlayer = (int)z;
                }
            }

            if (v.ContainsKey("icon"))
            {
                Icon = v["icon"].First();
            }

            if (v.ContainsKey("scale")) {
                var s = v["scale"].First();
                Scale = Double.Parse(s);
            }

            Title = v.ContainsKey("title") ? v["title"].First() : "";
            Description = v.ContainsKey("description") ? v["description"].First() : "";

            AnyToUnlock = false;
            HideEmpty = false;
            HideIfNoBranchParts = false;
            if (v.ContainsKey("cost"))
            {
                int c;
                if (!Int32.TryParse(v["cost"].First(), out c))
                {
                    Cost = 0;
                }
                Cost = c;
            }
            if (v.ContainsKey("anyParent"))
            {
                switch (v["anyParent"].First().Trim().ToLower())
                {
                    case "true":
                        AnyToUnlock = true;
                        break;
                }
            }
            if (v.ContainsKey("hideEmpty"))
            {
                switch (v["hideEmpty"].First().Trim().ToLower())
                {
                    case "true":
                        HideEmpty = true;
                        break;
                }
            }
            if (v.ContainsKey("hideIfNoBranchParts"))
            {
                switch (v["hideIfNoBranchParts"].First().Trim().ToLower())
                {
                    case "true":
                        HideIfNoBranchParts = true;
                        break;
                }
            }

            // Create an empty parents collection, populated during linking
            Parents = new List<TechNode>();

            var tmpParts = new List<string>();
            foreach (var child in sourceNode.Children.Where(child => child.Name == "Unlocks").Where(child => child.Values.ContainsKey("part")))
            {
                tmpParts.AddRange(child.Values["part"]);
            }
            Parts = new List<string>(tmpParts);
        }

		public string GenerateTechId()
		{
			if (String.IsNullOrEmpty(NodePart)) return "";
			var i = NodePart.IndexOf('_');
			return i > 0 && i + 1 < NodePart.Length ? NodePart.Substring(i + 1) : NodePart;
		}

		#endregion Methods
	}
}


