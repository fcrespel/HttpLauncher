using HttpLauncher.Models;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion
    }
}
