using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using ksp_techtree_edit.Models;
using ksp_techtree_edit.Properties;
using ksp_techtree_edit.Util;
using ksp_techtree_edit.Saver;

namespace ksp_techtree_edit.ViewModels
{
	public class TechTreeViewModel : NotificationViewModel
	{
		#region Members

		#region Private

		private WorkspaceViewModel _workspaceViewModel;
		private PartCollectionViewModel _partCollectionViewModel;
		private Point _mousePosition;

		public static Dictionary<string, string> localizationDictionary = new Dictionary<string, string>();

		#endregion Private

		#region Public

		public ObservableCollection<TechNodeViewModel> TechTree { get; private set; }

		public ObservableCollection<ConnectionViewModel> Connections { get; private set; }

		public WorkspaceViewModel WorkspaceViewModel
		{
			get { return _workspaceViewModel; }
			set
			{
				if (_workspaceViewModel == value) return;
				_workspaceViewModel = value;
				OnPropertyChanged();
			}
		}

		public PartCollectionViewModel PartCollectionViewModel
		{
			get { return _partCollectionViewModel; }
			set
			{
				if (_partCollectionViewModel == value) return;
				_partCollectionViewModel = value;
				OnPropertyChanged();
			}
		}

		public Point MousePosition
		{
			get { return _mousePosition; }
			set
			{
				if (_mousePosition == value) return;
				_mousePosition = value;
				OnPropertyChanged();
			}
		}

		public string[] StockNodes { get; set; }

		#endregion Public

		#endregion Members

		#region Constructors

		public TechTreeViewModel()
		{
			Connections = new ObservableCollection<ConnectionViewModel>();
			TechTree = new ObservableCollection<TechNodeViewModel>();
			StockNodes = Resources.stocknodes.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Searches through the tech tree and creates new connection models
		/// for each child-to-parent relationship. Connection models are stored
		/// in <see cref="Connections"/>.
		/// </summary>
		public void LinkNodes()
		{
			Connections.Clear();
            foreach (var node in TechTree)
            {
                foreach (var parent in node.Parents)
                {
                    Connections.Add(new ConnectionViewModel(node, parent));
                }
            }
		}

		public void DeleteNode(TechNodeViewModel node)
		{
			WorkspaceViewModel.SelectedNode = null;
			TechTree.Remove(node);
			UnlinkParent(node);
			var parts = new PartViewModel[node.Parts.Count];
			node.Parts.CopyTo(parts, 0);
			foreach (var part in parts)
			{
				PartCollectionViewModel.RemovePartFromNode(part, node);
			}
			LinkNodes();
		}

		public TechNodeViewModel AddNode(Point pos)
		{
			var node = new TechNode(GenerateNodeName()) { Pos = pos };
			var nodeViewModel = new TechNodeViewModel { TechNode = node };
			TechTree.Add(nodeViewModel);
			return nodeViewModel;
		}

		public string GenerateNodeName()
		{
			foreach (var stockNodeName in StockNodes)
			{
				if (!ContainsNodePart(stockNodeName))
                    return stockNodeName;
			}

			const string namePrefix = "newnode_";
			var randGen = new Random();
			var limit = 9999;
			var name = namePrefix + randGen.Next(100, limit);

			for (var i = 0; i < limit; i++)
			{
				if (!ContainsNodePart(name))
				{
					return name;
				}

				if (i == limit - 1)
				{
					limit = (limit * 10) + 9;
				}

				name = namePrefix + randGen.Next(100, limit);
			}

			return name;
		}

		public bool ContainsNodePart(string nodePart)
		{
			foreach (var nodeViewModel in TechTree)
			{
				if (nodeViewModel.NodePart == nodePart)
				{
					return true;
				}
			}
			return false;
		}

		//TODO: Remove this terrible terrible method and replace with proper
		//MVVM model-viewmodel dependencies.
		public void UnlinkParent(TechNodeViewModel parentToRemove)
		{
			foreach (var node in TechTree)
			{
				if (node.Parents.Contains(parentToRemove))
				{
					node.RemoveParent(parentToRemove);
				}
			}
		}

		#endregion Methods
	}
}
