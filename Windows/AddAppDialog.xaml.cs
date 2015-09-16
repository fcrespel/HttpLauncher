using HttpLauncher.Models;
using Microsoft.Win32;
using System.Windows;

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

        #region Button Events

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select the application to launch";
            dialog.CheckFileExists = true;
            dialog.Filter = "Applications (.exe)|*.exe";
            if (dialog.ShowDialog(this).Value == true)
            {
                textboxPath.Text = dialog.FileName;
            }
        }

        private void ButtonBrowseWorkDir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Select the working directory for the application";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textboxWorkDir.Text = dialog.SelectedPath;
            }
        }

        #endregion
    }
}
