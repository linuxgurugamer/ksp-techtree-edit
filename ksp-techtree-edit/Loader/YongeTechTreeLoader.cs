using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KerbalParser;
using ksp_techtree_edit.Models;
using ksp_techtree_edit.ViewModels;
using System.Windows;
using ksp_techtree_edit.Util;

namespace ksp_techtree_edit.Loader
{
    public class YongeTechTreeLoader : TreeLoader
    {
        public override void LoadTree(KerbalConfig config, TechTreeViewModel treeData)
        {
            var nameNodeHashtable = new Dictionary<string, TechNodeViewModel>();
            var techNodes = config.First(child => child.Name == "TechTree").Children.Where(node => node.Name == "RDNode").ToArray();

            foreach (KerbalNode node in techNodes.Where(kerbalNode => kerbalNode.Values.ContainsKey("nodepart")))
            {
                var v = node.Values;
                var id = v["id"].First();
                TechNodeViewModel techNodeViewModel;
                if (nameNodeHashtable.ContainsKey(id))
                {
                    techNodeViewModel = nameNodeHashtable[id];
                }
                else
                {
                    techNodeViewModel = new TechNodeViewModel();
                    nameNodeHashtable.Add(id, techNodeViewModel);
                }
                techNodeViewModel.TechNode = this.PopulateFromSource(node);

                // Find parent
                foreach (KerbalNode parentNode in node.Children.Where(child => child.Name == "Parent"))
                {
                    var parentKeyValuePairs = parentNode.Values.Where(pair => pair.Key == "parentID");
                    var parents = new List<string>();
                    foreach (var parentKeyValuePair in parentKeyValuePairs)
                    {
                        parents.Add(parentKeyValuePair.Value.First());
                    }
                    foreach (var parent in parents.Where(parent => !nameNodeHashtable.ContainsKey(parent)))
                    {
                        nameNodeHashtable.Add(parent, new TechNodeViewModel());
                    }
                    foreach (var parent in parents.Where(parent => !String.IsNullOrEmpty(parent) && nameNodeHashtable.ContainsKey(parent)))
                    {
                        techNodeViewModel.Parents.Add(nameNodeHashtable[parent]);
                    }
                }
                treeData.TechTree.Add(techNodeViewModel);
            }
        }

        public override TechNode PopulateFromSource(KerbalNode sourceNode)
        {
            TechNode newNode = new TechNode();
            var v = sourceNode.Values;
            newNode.NodePart = v.ContainsKey("nodepart") ? v["nodepart"].First() : "";

            double x;
            double y;
            newNode.Id = v.ContainsKey("id") ? v["id"].First() : "";

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
                    newNode.Pos = new Point(x, y);

                    decimal z;
                    if (!Decimal.TryParse(coordinates[2], out z))
                    {
                        newNode.Zlayer = -1;
                    }
                    newNode.Zlayer = (int)z;
                }
            }

            if (v.ContainsKey("icon"))
            {
                newNode.Icon = v["icon"].First();
            }

            if (v.ContainsKey("scale"))
            {
                var s = v["scale"].First();
                newNode.Scale = Double.Parse(s);
            }

            newNode.Title = v.ContainsKey("title") ? v["title"].First() : "";
            newNode.Description = v.ContainsKey("description") ? v["description"].First() : "";

            newNode.AnyToUnlock = false;
            newNode.HideEmpty = false;
            newNode.HideIfNoBranchParts = false;
            if (v.ContainsKey("cost"))
            {
                int c;
                if (!Int32.TryParse(v["cost"].First(), out c))
                {
                    newNode.Cost = 0;
                }
                newNode.Cost = c;
            }
            if (v.ContainsKey("anyParent"))
            {
                switch (v["anyParent"].First().Trim().ToLower())
                {
                    case "true":
                        newNode.AnyToUnlock = true;
                        break;
                }
            }
            if (v.ContainsKey("hideEmpty"))
            {
                switch (v["hideEmpty"].First().Trim().ToLower())
                {
                    case "true":
                        newNode.HideEmpty = true;
                        break;
                }
            }
            if (v.ContainsKey("hideIfNoBranchParts"))
            {
                switch (v["hideIfNoBranchParts"].First().Trim().ToLower())
                {
                    case "true":
                        newNode.HideIfNoBranchParts = true;
                        break;
                }
            }

            // Create an empty parents collection, populated during linking
            newNode.Parents = new List<TechNode>();

            var tmpParts = new List<string>();
            foreach (var child in sourceNode.Children.Where(child => child.Name == "Unlocks").Where(child => child.Values.ContainsKey("part")))
            {
                tmpParts.AddRange(child.Values["part"]);
            }
            newNode.Parts = new List<string>(tmpParts);

            return newNode;
        }

        public override void PopulateParts(PartCollectionViewModel pc, TechTreeViewModel ttvm)
        {

            // Create and init part table with name => partviewmodel
            var partTable = new Dictionary<string, PartViewModel>();
            foreach (PartViewModel part in pc.PartCollection)
            {
                try
                {
                    if (!partTable.ContainsKey(part.PartName))
                    {
                        partTable.Add(part.PartName, part);
                    }
                    else
                    {
                        var duplicate = partTable[part.PartName];
                        var existString = String.Format(" - Existing part: {0} ({1})", duplicate.PartName, duplicate.FileName);
                        Logger.Error("PartLoader: Error while storing part \"{0}\" " + "({1}) into PartCollection - {2}{3}", part.PartName, part.FileName, "Part already exists", existString);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("PartLoader: Error while storing part \"{0}\" " + "({1}) into PartCollection - {2}", part.PartName, part.FileName, e.Message);
                }
            }

            foreach( var node in ttvm.TechTree )
            {
                foreach (String part in node.TechNode.Parts)
                {
                    if (partTable.ContainsKey(part))
                    {
                        node.Parts.Add(partTable[part]);
                        pc.PartCollection.Remove(partTable[part]);
                    }
                    else
                    {
                        var tmpPart = new Part(part) { Title = part, TechRequired = node.Id, Category = "(Unknown)" };
                        node.Parts.Add(new PartViewModel(tmpPart));
                    }
                }
            }

        }
    }
}
