using HttpLauncher.Components;
using HttpLauncher.Models;
using HttpLauncher.Properties;
using HttpLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace HttpLauncher.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Protected Properties

        protected HttpServer HttpServer
        {
            get { return TryFindResource("HttpServer") as HttpServer; }
        }

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Window Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.AutoStart)
                CustomCommands.Start.Execute(null, this);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ApplicationCommands.Stop.Execute(null, this);
            Settings.Default.Save();
        }

        #endregion

        #region Button Events

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAppDialog dialog = new AddAppDialog(this);
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ((AppDictionary)appList.ItemsSource).Add(dialog.AppInfo.Name, dialog.AppInfo);
            }
            appList.Items.Refresh();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (KeyValuePair<string, AppInfo> item in appList.SelectedItems)
            {
                ((AppDictionary)appList.ItemsSource).Remove(item.Key);
            }
            appList.Items.Refresh();
        }

        #endregion

        #region CommandBinding Events

        private void StartCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !HttpServer.IsListening;
        }

        private void StartCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HttpServer.Start();
        }

        private void StopCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = HttpServer.IsListening;
        }

        private void StopCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HttpServer.Stop();
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Hyperlink Events

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        #endregion
    }
}
