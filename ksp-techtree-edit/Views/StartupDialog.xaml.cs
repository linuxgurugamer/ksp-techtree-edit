using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using ksp_techtree_edit.Annotations;
using ksp_techtree_edit.Properties;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ksp_techtree_edit.Models;

namespace ksp_techtree_edit.Views
{
	/// <summary>
	/// Interaction logic for StartupDialog.xaml
	/// </summary>
	public partial class StartupDialog : INotifyPropertyChanged
	{

        #region Members

        // Boolean for enble the button
        private bool _canLoad;

		public bool CanLoad
		{
			get { return _canLoad; }
			set
			{
				if (_canLoad == value)
                    return;
				_canLoad = value;
				OnPropertyChanged();
			}
		 }

        #endregion

        /**
         * Constructor
         */
        public StartupDialog()
		{
			try
			{
				var kspDir = new DirectoryInfo(Settings.Default.KspPath);
				CanLoad = kspDir.Exists;
				Console.WriteLine(kspDir.FullName);
				Console.WriteLine(kspDir.Exists);
			}
			catch (Exception e)
			{
				Console.WriteLine("StartupDialog.xaml.cs.StartupDialog: " + e.Message);
				CanLoad = false;
			}
			InitializeComponent();
		}

        /**
         * Method for setup the KSP installation
         * Status: OK
         */
		private void StartupDialog_SetKSPFolder(object sender, RoutedEventArgs e)
		{
			var dlg = new CommonOpenFileDialog { Title = "Select your KSP installation folder", IsFolderPicker = true };
			var result = dlg.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                if (dlg.FileName != null && dlg.FileName != "" && Directory.Exists(dlg.FileName + "/GameData"))
                {
                    Settings.Default.KspPath = dlg.FileName;
                    Settings.Default.Save();
                    CanLoad = true;
                }
            }
            else
            {
                MessageBox.Show("The directory is not KSP directory. Please select your KSP installation folder !");
            }
		}

        /**
         * Generic method for loading a techtree
         * Status: OK
         */
        private void Load(String title, TreeType type) {
            var mainWindow = Owner as MainWindow;
            if (mainWindow == null)
                return;
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".cfg",
                Filter = "Tech Tree Config Files|*.cfg",
                Title = title,
            };
            var result = dlg.ShowDialog();
            if (result == false)
                return;
            CanLoad = false;
            try
            {
                mainWindow.LoadTree(dlg.FileName, type);
                mainWindow.FindParts();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("StartupDialog.xaml.cs.Load: " + exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                CanLoad = true;
            }
        }

        /**
         * Method for loading a techtree in stock format
         * Status: OK
         */
        private void StartupDialog_LoadStockTree(object sender, RoutedEventArgs e)
        {
            Load("Select TechMananger tree to load", TreeType.StockTechTree);
        }

        /**
         * Method for loading a techtree in yongeTech format
         * Status: OK
         */
        private void StartupDialog_LoadYongeTree(object sender, RoutedEventArgs e)
        {
            Load("Select TechMananger tree to load", TreeType.YongeTech);
        }

        /**
         * Method for create a new trechtree with stock techtree model.
         * Status: OK
         */
        private void StartupDialog_NewStockTree(object sender, RoutedEventArgs e)
		{
			var mainWindow = Owner as MainWindow;
			if (mainWindow == null)
                return;
			CanLoad = false;
			try
			{
				mainWindow.LoadTree(Settings.Default.StockTreePath);
				mainWindow.FindParts();
				Close();
			}
			catch (Exception exception)
			{
				MessageBox.Show("StartupDialog.xaml.cs.StartupDialog_NewStockTree: " + exception.Message,"", MessageBoxButton.OK, MessageBoxImage.Error);
				CanLoad = true;
			}
		}

        /**
         * Method for create a empty techtree
         * Status: OK
         */
        private void StartupDialog_NewBlankTree(object sender, RoutedEventArgs e)
		{
			var mainWindow = Owner as MainWindow;
			if (mainWindow == null)
                return;
			mainWindow.NewTree();
			mainWindow.FindParts();
			Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

    }
}
