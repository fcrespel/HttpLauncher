using HttpLauncher.Models;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HttpLauncher.Windows
{
    /// <summary>
    /// Interaction logic for AddAppDialog.xaml
    /// </summary>
    public partial class AddAppDialog : Window
    {
        #region Constructors

        public AddAppDialog()
        {
            InitializeComponent();
        }

        public AddAppDialog(Window owner) : this()
        {
            this.Owner = owner;
        }

        #endregion

        #region Public Properties

        public AppInfo AppInfo
        {
            get { return TryFindResource("AppInfo") as AppInfo; }
        }

        #endregion

        #region Private Methods

        private bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
                LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }

        #endregion

        #region Button Events

        private void ButtonBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select the application to launch";
            dialog.CheckFileExists = true;
            dialog.Filter = "Applications (.exe)|*.exe";
            if (dialog.ShowDialog(this).Value == true)
            {
                AppInfo.Path = dialog.FileName;
            }
        }

        private void ButtonBrowseWorkDir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Select the working directory for the application";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AppInfo.WorkDir = dialog.SelectedPath;
            }
        }

        #endregion

        #region CommandBinding Events

        private void SaveCommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsValid(this);
        }

        private void SaveCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion
    }
}
