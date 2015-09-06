using System;
using System.Reflection;
using System.Windows;

namespace HttpLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Event Handlers

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string message = "A fatal error occurred, causing " + AssemblyTitle + " to stop. Please report this incident with the information below.";
            message += "\n\nSource: " + e.Exception.Source;
            message += "\n\nMessage: " + e.Exception.Message;
            if (e.Exception.InnerException != null)
                message += "\n\nCause: " + e.Exception.InnerException.Message;

            MessageBox.Show(message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Shutdown();
        }

        #endregion

        #region Public Properties

        public static string AssemblyTitle
        {
            get
            {
                AssemblyTitleAttribute attr = (AssemblyTitleAttribute)GetAssemblyAttribute(typeof(AssemblyTitleAttribute));
                if (attr != null)
                    return attr.Title;
                else
                    return null;
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                AssemblyDescriptionAttribute attr = (AssemblyDescriptionAttribute)GetAssemblyAttribute(typeof(AssemblyDescriptionAttribute));
                if (attr != null)
                    return attr.Description;
                else
                    return null;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                AssemblyCompanyAttribute attr = (AssemblyCompanyAttribute)GetAssemblyAttribute(typeof(AssemblyCompanyAttribute));
                if (attr != null)
                    return attr.Company;
                else
                    return null;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                AssemblyProductAttribute attr = (AssemblyProductAttribute)GetAssemblyAttribute(typeof(AssemblyProductAttribute));
                if (attr != null)
                    return attr.Product;
                else
                    return null;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                AssemblyCopyrightAttribute attr = (AssemblyCopyrightAttribute)GetAssemblyAttribute(typeof(AssemblyCopyrightAttribute));
                if (attr != null)
                    return attr.Copyright;
                else
                    return null;
            }
        }

        public static Version AssemblyVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                if (assembly != null)
                    return assembly.GetName().Version;
                else
                    return null;
            }
        }

        #endregion

        #region Protected Methods

        protected static object GetAssemblyAttribute(Type attributeType)
        {
            object attribute = null;

            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
            {
                object[] attributes = assembly.GetCustomAttributes(attributeType, false);
                if (attributes != null && attributes.Length > 0)
                    attribute = attributes[0];
            }

            return attribute;
        }

        #endregion
    }
}
