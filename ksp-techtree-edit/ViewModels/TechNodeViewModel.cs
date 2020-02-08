using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows;
using ksp_techtree_edit.Models;
using ksp_techtree_edit.Util;
using System.ComponentModel;
using ksp_techtree_edit.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ksp_techtree_edit.ViewModels
{
    [DataContract]
    [DefaultProperty("Id")]
    public class TechNodeViewModel : NotificationViewModel
    {
        #region Data Members

        #region Model Wrappers
        public string Id
        {
            get { return _techNode.Id; }
            set
            {
                if (_techNode.Id == value) return;
                _techNode.Id = value;
                OnPropertyChanged();
            }
        }

        public string NodePart
        {
            get { return _techNode.NodePart; }
            set
            {
                if (_techNode.NodePart == value) return;
                _techNode.NodePart = value;
                OnPropertyChanged();
            }
        }

        static public string GetText(string key)
        {
            if (key.StartsWith("#"))
            {
                var pieces = key.Split(new[] { ' ' }, 2);
                if (TechTreeViewModel.localizationDictionary.ContainsKey(pieces[0].Trim()))
                {
                    TechTreeViewModel.localizationDictionary.TryGetValue(pieces[0], out string data);
                    return data;
                }
            }
            return key;
        }
        public string L_Title
        {
            get { return _techNode.Title; }
        }
        public string L_Description
        {
            get { return _techNode.Description; }
        }
        public string Title
        {
            get { return GetText(_techNode.Title); }
            set
            {
                if (_techNode.Title == value) return;
                _techNode.Title = value;
                OnPropertyChanged();
            }
        }
        public string TitleAbbr {
            get {
                string str = "";
                int idx = 0;
                int cnt = 0;
                for (int i = 0; i < Title.Length; i++)
                    if (Char.IsUpper(Title[i]))
                    {
                        idx = i+1;
                        str += Title[i];
                        if (cnt == 0)
                        {
                            for (int i1 = idx; i1 < idx + 3; i1++)
                            {
                                if (Char.IsUpper(Title[i1]))
                                    break;
                                str += Title[i1];
                            }
                            str += "\n";
                            cnt = 1;
                        }
                    }
                for (int i = idx; i < idx +3; i++)
                    str += Title[i];
                return str;
                //return Title.Substring(0, 2); 
            } 
        }
        public string Description
        {
            get { return GetText(_techNode.Description); }
            set
            {
                if (_techNode.Description == value) return;
                _techNode.Description = value;
                OnPropertyChanged();
            }
        }

        public int Cost
        {
            get { return _techNode.Cost; }
            set
            {
                if (_techNode.Cost == value) return;
                _techNode.Cost = value;
                OnPropertyChanged();
            }
        }

        public Point Pos
        {
            get { return _techNode.Pos; }
            set
            {
                if (_techNode.Pos == value) return;
                _techNode.Pos = value;
                OnPropertyChanged();
            }
        }

        public string Icon
        {
            get
            {
                return _techNode.Icon;
            }
            set
            {
                if (_techNode.Icon == value)
                    return;
                _techNode.Icon = value;
                OnPropertyChanged();
            }
        }

        public double Scale
        {
            get { return _techNode.Scale; }
            set
            {
                if (_techNode.Scale == value) return;
                _techNode.Scale = value;
                OnPropertyChanged();
            }
        }

        public bool AnyToUnlock
        {
            get { return _techNode.AnyToUnlock; }
            set
            {
                if (_techNode.AnyToUnlock == value) return;
                _techNode.AnyToUnlock = value;
                OnPropertyChanged();
            }
        }

        public bool HideEmpty
        {
            get { return _techNode.HideEmpty; }
            set
            {
                if (_techNode.HideEmpty == value) return;
                _techNode.HideEmpty = value;
                OnPropertyChanged();
            }
        }

        public bool HideIfNoBranchParts
        {
            get { return _techNode.HideIfNoBranchParts; }
            set
            {
                if (_techNode.HideIfNoBranchParts == value) return;
                _techNode.HideIfNoBranchParts = value;
                OnPropertyChanged();
            }
        }



        [Browsable(false)]
        public int Zlayer
        {
            get { return _techNode.Zlayer; }
            set
            {
                if (_techNode.Zlayer == value) return;
                _techNode.Zlayer = value;
                OnPropertyChanged();
            }
        }

        #endregion Model Wrappers

        private bool _isSelected;
        private TechNode _techNode;
        private ObservableCollection<TechNodeViewModel> _parents = new ObservableCollection<TechNodeViewModel>();
        private ObservableCollection<PartViewModel> _parts = new ObservableCollection<PartViewModel>();

        [Browsable(false)]
        public int Width { get; set; }
        [Browsable(false)]
        public int Height { get; set; }

        [Browsable(false)]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public TechNode TechNode
        {
            get { return _techNode; }
            set
            {
                if (_techNode == value) return;
                _techNode = value;
                OnPropertyChanged();
            }
        }

        [Browsable(false)]
        public ObservableCollection<TechNodeViewModel> Parents
        {
            get { return _parents; }
            set { _parents = value; }
        }

        [Browsable(false)]
        public ObservableCollection<PartViewModel> Parts
        {
            get { return _parts; }
            set { _parts = value; }
        }

        #endregion Data Members

        #region Constructors

        public TechNodeViewModel()
        {
            Width = 40;
            Height = 40;

            TechNode = new TechNode();
        }

        #endregion Constructors

        #region Helper Methods

        public void RemovePart(PartViewModel part)
        {
            Parts.Remove(part);
            TechNode.Parts.Remove(part.PartName);
        }

        public void AddPart(PartViewModel part)
        {
            Parts.Add(part);
            TechNode.Parts.Add(part.PartName);
        }

        public void RemoveParent(TechNodeViewModel parent)
        {
            Parents.Remove(parent);
            TechNode.Parents.Remove(parent.TechNode);
        }

        public void AddParent(TechNodeViewModel parent)
        {
            Parents.Add(parent);
            TechNode.Parents.Add(parent.TechNode);
        }

        #endregion Helper Methods
    }
}
