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
        #region Private Fields

        private System.Windows.Forms.NotifyIcon notifyIcon;

        #endregion

        #region Protected Properties

        protected HttpServer HttpServer
        {
            get { return TryFindResource("HttpServer") as HttpServer; }
        }

        protected System.Windows.Forms.NotifyIcon NotifyIcon
        {
            get
            {
                if (notifyIcon == null)
                {
                    notifyIcon = new System.Windows.Forms.NotifyIcon();
                    notifyIcon.Text = App.AssemblyTitle;
                    notifyIcon.Icon = Properties.Resources.AppIcon;
                    notifyIcon.Click += NotifyIcon_Click;
                    notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[]
                        {
                            new System.Windows.Forms.MenuItem("Show main window", NotifyIcon_Click),
                            new System.Windows.Forms.MenuItem("-"),
                            new System.Windows.Forms.MenuItem("Exit", NotifyIcon_CloseMenuItem_Click)
                        });
                }
                return notifyIcon;
            }
            set
            {
                if (notifyIcon != null && notifyIcon != value)
                    notifyIcon.Dispose();
                notifyIcon = value;
            }
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
            checkboxRunAtStartup.IsChecked = App.RunAtStartup;
            NotifyIcon.Visible = true;
            if (Settings.Default.StartServer)
            {
                WindowState = WindowState.Minimized;
                CustomCommands.Start.Execute(null, this);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NotifyIcon = null;
            ApplicationCommands.Stop.Execute(null, this);
            Settings.Default.Save();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                NotifyIcon.ShowBalloonTip(5000, App.AssemblyTitle + " is running in the background", "Click this icon to open and configure it.", System.Windows.Forms.ToolTipIcon.Info);
            }
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

        #region Checkbox Events

        private void checkboxRunAtStartup_Changed(object sender, RoutedEventArgs e)
        {
            App.RunAtStartup = checkboxRunAtStartup.IsChecked.Value;
        }

        #endregion

        #region CommandBinding Events

        private void StartCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !HttpServer.IsListening;
        }

        private void StartCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                HttpServer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HTTP server could not be started. " + ex.Message, "Server Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            Close();
        }

        #endregion

        #region Hyperlink Events

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        #endregion

        #region NotifyIcon Events

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void NotifyIcon_CloseMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
