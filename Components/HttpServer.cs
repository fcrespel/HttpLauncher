using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace HttpLauncher.Components
{
    public class HttpServer : DependencyObject
    {
        #region Private Variables

        private HttpListener listener = new HttpListener();
        private Thread listenerThread;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(long), typeof(HttpServer), new PropertyMetadata(9980L));

        public static readonly DependencyProperty AuthenticatorProperty =
            DependencyProperty.Register("Authenticator", typeof(HttpAuthenticator), typeof(HttpServer), new PropertyMetadata(null));

        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controllers", typeof(Dictionary<string, HttpController>), typeof(HttpServer), new PropertyMetadata(new Dictionary<string, HttpController>()));

        public static readonly DependencyProperty IsListeningProperty =
            DependencyProperty.Register("IsListening", typeof(bool), typeof(HttpServer), new PropertyMetadata(false));

        public long Port
        {
            get { return (long)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        public HttpAuthenticator Authenticator
        {
            get { return (HttpAuthenticator)GetValue(AuthenticatorProperty); }
            set { SetValue(AuthenticatorProperty, value); }
        }

        public Dictionary<string, HttpController> Controllers
        {
            get { return (Dictionary<string, HttpController>)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        public bool IsListening
        {
            get { return (bool)GetValue(IsListeningProperty); }
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (!listener.IsListening)
            {
                Debug.WriteLine("Starting HttpServer...");
                ConfigureHttpListener();
                listener.Start();
                SetValue(IsListeningProperty, true);

                listenerThread = new Thread(new ThreadStart(ProcessingLoop));
                listenerThread.Start();
                Debug.WriteLine("HttpServer started.");
            }
        }

        public void Stop()
        {
            if (listener.IsListening)
            {
                Debug.WriteLine("Stopping HttpServer...");
                listener.Stop();
                SetValue(IsListeningProperty, false);
                Debug.WriteLine("HttpServer stopped.");
            }
        }

        #endregion

        #region Private Methods

        private void ConfigureHttpListener()
        {
            listener.Prefixes.Clear();
            listener.Prefixes.Add("http://+:" + Port + "/");

            if (Authenticator != null && Authenticator.IsEnabled)
            {
                listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
                listener.Realm = "HttpListener";
            }
            else
            {
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            }
        }

        private void ProcessingLoop()
        {
            Debug.WriteLine("Processing loop started");
            while (listener.IsListening)
            {
                try
                {
                    Debug.WriteLine("Waiting for request...");
                    HttpListenerContext context = listener.GetContext();
                    Debug.WriteLine("Dispatching request...");
                    DispatchRequest(context.Request, context.Response, context.User);
                    Debug.WriteLine("Request processed.");
                }
                catch (HttpListenerException e)
                {
                    Debug.WriteLine("Error in processing loop: " + e.ToString());
                }
            }
            Debug.WriteLine("Processing loop stopped");
        }

        private HttpController GetController(HttpListenerRequest request)
        {
            if (!CheckAccess())
                return Dispatcher.Invoke(() => GetController(request));

            HttpController controller = null;
            if (Controllers != null)
                Controllers.TryGetValue(request.Url.AbsolutePath, out controller);
            return controller;
        }

        private bool AuthenticateRequest(HttpListenerRequest request, IPrincipal user)
        {
            if (!CheckAccess())
                return Dispatcher.Invoke(() => AuthenticateRequest(request, user));

            if (Authenticator != null && Authenticator.IsEnabled)
            {
                Debug.WriteLine("Authenticating request...");
                return Authenticator.AuthenticateRequest(request, user);
            }
            else
            {
                return true;
            }
        }

        private void DispatchRequest(HttpListenerRequest request, HttpListenerResponse response, IPrincipal user)
        {
            try
            {
                if (AuthenticateRequest(request, user))
                {
                    HttpController controller = GetController(request);
                    if (controller != null)
                    {
                        Debug.WriteLine("Processing request...");
                        HttpStatusCode status = controller.ProcessRequest(request, response);
                        response.StatusCode = (int)status;
                    }
                    else
                    {
                        Debug.WriteLine("Unknown request path " + request.Url.AbsolutePath);
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    Debug.WriteLine("Access denied to " + request.Url.AbsolutePath);
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error dispatching request: " + e.ToString());
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                response.OutputStream.Close();
            }
        }

        #endregion
    }
}
