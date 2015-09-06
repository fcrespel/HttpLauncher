using HttpLauncher.Components;
using HttpLauncher.Models;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Windows;

namespace HttpLauncher.Controllers
{
    public class LaunchController : DependencyObject, HttpController
    {
        #region Dependency Properties

        public static readonly DependencyProperty AppDictionaryProperty =
            DependencyProperty.Register("AppDictionary", typeof(AppDictionary), typeof(LaunchController), new PropertyMetadata(new AppDictionary()));

        public AppDictionary AppDictionary
        {
            get { return (AppDictionary)GetValue(AppDictionaryProperty); }
            set { SetValue(AppDictionaryProperty, value); }
        }

        #endregion

        #region Public Methods

        public HttpStatusCode ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(request.Url.Query);
            string app = query.Get("app");
            if (app == null || app.Length == 0)
                throw new ArgumentException("Missing 'app' parameter");

            AppInfo appInfo = GetAppInfo(app);
            if (appInfo != null)
            {
                Debug.WriteLine("Launching application " + app + "...");
                ProcessStartInfo psi = new ProcessStartInfo(appInfo.Path, appInfo.Args);
                psi.WorkingDirectory = appInfo.WorkDir;
                Process.Start(psi);

                return HttpStatusCode.NoContent;
            }

            Debug.WriteLine("Application " + app + " does not exist");
            return HttpStatusCode.NotFound;
        }

        #endregion

        #region Private Methods

        private AppInfo GetAppInfo(string app)
        {
            if (!CheckAccess())
                return Dispatcher.Invoke(() => GetAppInfo(app));

            AppInfo appInfo = null;
            if (app != null && AppDictionary != null)
                AppDictionary.TryGetValue(app, out appInfo);

            return appInfo;
        }

        #endregion
    }
}
