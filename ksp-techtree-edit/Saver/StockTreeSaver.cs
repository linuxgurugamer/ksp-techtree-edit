using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ksp_techtree_edit.ViewModels;
using System.Globalization;
using ksp_techtree_edit.Util;

namespace ksp_techtree_edit.Saver
{
    public class StockTreeSaver : TreeSaver
    {
        public override TreeSaver EndNode()
        {
            IndentationLevel--;
            AddLine("}");
            return this;
        }

        public override TreeSaver EndParents()
        {
            return this;
        }

        public override TreeSaver EndParts()
        {
            return this;
        }

        public override TreeSaver EndTree()
        {
            IndentationLevel--;
            AddLine("}");
            AddLine();
            return this;
        }

        public override void Save(TechTreeViewModel techtreeviewmodel, string path)
        {
            this.StartTree(techtreeviewmodel);
            var totalCost = 0;
            foreach (TechNodeViewModel node in techtreeviewmodel.TechTree)
            {
                totalCost += node.Cost;
                var parts = new List<string>();
                foreach (var part in node.Parts)
                {
                    parts.Add(part.PartName);
                }
                var parents = new List<string>();
                foreach (var parent in node.Parents)
                {
                    String parentPos = FindParentPosition(node.Pos, parent.Pos);
                    String parentIdPos = parent.Id + "|" + parentPos;
                    parents.Add(parentIdPos);
                }
                this.StartNode().
                      SaveAttribute(new KeyValuePair<string, string>("id", node.Id)).
                      SaveAttribute(new KeyValuePair<string, string>("nodeName", node.NodePart)).
                      SaveAttribute(new KeyValuePair<string, string>("title", node.L_Title)).
                      SaveAttribute(new KeyValuePair<string, string>("description", node.L_Description)).
                      SaveAttribute(new KeyValuePair<string, string>("cost", node.Cost.ToString(CultureInfo.InvariantCulture))).
                      SavePosition(node.Pos.X, node.Pos.Y, node.Zlayer).
                      SaveAttribute(new KeyValuePair<string, string>("icon", node.Icon)).
                      SaveAttribute(new KeyValuePair<string, string>("anyToUnlock", node.AnyToUnlock.ToString())).
                      SaveAttribute(new KeyValuePair<string, string>("hideEmpty", node.HideEmpty.ToString())).
                      SaveAttribute(new KeyValuePair<string, string>("hideIfNoBranchParts", node.HideIfNoBranchParts.ToString())).
                      SaveAttribute(new KeyValuePair<string, string>("scale", node.Scale.ToString()));
                StartParents().
                SaveParents(parents).
                EndParents().
                EndNode();
            }
            this.EndTree();
            this.Save(path);
            Logger.Log("Tree saved succesfully to {0}. Total cost: {1} science. Total nodes: {2} nodes.", path, totalCost, techtreeviewmodel.TechTree.Count);
        }

        public override TreeSaver SaveAttribute(KeyValuePair<string, string> nameAttributePair)
        {
            AddLine(nameAttributePair.Key + " = " + nameAttributePair.Value);
            return this;
        }

        public override TreeSaver SaveParents(IEnumerable<string> parentsList)
        {
            var parents = parentsList as string[] ?? parentsList.ToArray();
            String[] decode;
            String parentId, pos;
            foreach (String parent in parents)
            {
                decode = parent.Split('|');
                parentId = decode[0];
                pos = decode[1];
                AddLine("Parent");
                AddLine("{");
                IndentationLevel++;
                AddLine("parentID = " + parentId);
                switch (pos)
                {
                    case "LR":
                        AddLine("lineFrom = LEFT");
                        AddLine("lineTo = RIGHT");
                        break;
                    case "RL":
                        AddLine("lineFrom = RIGHT");
                        AddLine("lineTo = LEFT");
                        break;
                    case "TB":
                        AddLine("lineFrom = TOP");
                        AddLine("lineTo = BOTTOM");
                        break;
                    case "BT":
                        AddLine("lineFrom = BOTTOM");
                        AddLine("lineTo = TOP");
                        break;
                }
                IndentationLevel--;
                AddLine("}");
            }
            return this;
        }

        public override TreeSaver SaveParts(TechNodeViewModel node)
        {
            return this;
        }

        public override TreeSaver SavePosition(double x, double y, double z)
        {
            var pos = x + "," + y + "," + z;
            AddLine("pos = " + pos);
            return this;
        }

        public override TreeSaver StartNode()
        {
            AddLine("RDNode");
            AddLine("{");
            IndentationLevel++;
            return this;
        }

        public override TreeSaver StartParents()
        {
            return this;
        }

        public override TreeSaver StartParts()
        {
            return this;
        }

        public override TreeSaver StartTree(TechTreeViewModel techTree = null)
        {
            AddLine("TechTree");
            AddLine("{");
            IndentationLevel++;
            AddLine("id = TEDGeneratedTree");
            AddLine("label = TED Generated Tree");
            return this;
        }
    }
}
