using System;
using ksp_techtree_edit.Models;

namespace ksp_techtree_edit.ViewModels
{
	public class PartViewModel : NotificationViewModel
	{
		#region Members

		private Part _part;

		public Part Part
		{
			get { return _part; }
			set
			{
				if (_part == value) return;
				_part = value;
				OnPropertyChanged();
			}
		}

		public string ComposedTitle
		{
			get { return String.Format("{0} - {1}", ModName, TechNodeViewModel.GetText(Title)); }
		}

		#region Model Wrappers

		public string PartName
		{
			get { return _part.PartName; }
			set
			{
				if (_part.PartName == value) return;
				_part.PartName = value;
				OnPropertyChanged();
			}
		}

		public string FileName
		{
			get { return _part.FileName; }
			set
			{
				if (_part.FileName == value) return;
				_part.FileName = value;
				OnPropertyChanged();
			}
		}

		public string ModName
		{
			get { return _part.ModName; }
		}

		public string Title
		{
			get { return TechNodeViewModel.GetText( _part.Title); }
			set
			{
				if (_part.Title == value) return;
				_part.Title = value;
				OnPropertyChanged();
			}
		}

		public string Description
		{
			get { return _part.Description; }
			set
			{
				if (_part.Description == value) return;
				_part.Description = value;
				OnPropertyChanged();
			}
		}

		public int Cost
		{
			get { return _part.Cost; }
			set
			{
				if (_part.Cost == value) return;
				_part.Cost = value;
				OnPropertyChanged();
			}
		}

		public string TechRequired
		{
			get { return _part.TechRequired; }
			set
			{
				if (_part.TechRequired == value) return;
				_part.TechRequired = value;
				OnPropertyChanged();
			}
		}

		public string Category
		{
			get { return _part.Category; }
			set
			{
				if (_part.Category == value) return;
				_part.Category = value;
				OnPropertyChanged();
			}
		}

		public string Icon
		{
			get { return _part.Icon; }
			set
			{
				if (_part.Icon == value) return;
				_part.Icon = value;
				OnPropertyChanged();
			}
		}

		#endregion Model Wrappers

		#endregion Members

		#region Constructors

		public PartViewModel()
		{
			Part = new Part();
		}

		public PartViewModel(Part part)
		{
			Part = new Part(part);
		}

		#endregion Constructors
	}
}
