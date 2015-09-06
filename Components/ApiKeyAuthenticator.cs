using System;
using System.Net;
using System.Security.Principal;
using System.Windows;

namespace HttpLauncher.Components
{
    public class ApiKeyAuthenticator : DependencyObject, HttpAuthenticator
    {
        public static readonly DependencyProperty ApiKeyProperty =
            DependencyProperty.Register("ApiKey", typeof(string), typeof(ApiKeyAuthenticator), new PropertyMetadata(null));

        public string ApiKey
        {
            get { return (string)GetValue(ApiKeyProperty); }
            set { SetValue(ApiKeyProperty, value); }
        }

        public bool IsEnabled
        {
            get { return ApiKey != null && ApiKey.Length != 0; }
        }

        public bool AuthenticateRequest(HttpListenerRequest request, IPrincipal user)
        {
            if (IsEnabled)
            {
                return user.Identity.Name.Equals(ApiKey, StringComparison.InvariantCulture);
            }
            else
            {
                return true;
            }
        }
    }
}
