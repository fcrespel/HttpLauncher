using System.ComponentModel;
using System.IO;
namespace HttpLauncher.Models
{
    public class AppInfo : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Private Fields

        private string name;
        private string path;
        private string args;
        private string workDir;

        #endregion

        #region Public Properties

        public string Name
        {
            get { return name; }
            set {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Path
        {
            get { return path; }
            set {
                path = value;
                RaisePropertyChanged("Path");
            }
        }

        public string Args
        {
            get { return args; }
            set {
                args = value;
                RaisePropertyChanged("Args");
            }
        }

        public string WorkDir
        {
            get { return workDir; }
            set {
                workDir = value;
                RaisePropertyChanged("WorkDir");
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IDataErrorInfo Implementation

        public string Error
        {
            get { return null;  }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(name))
                            return "Name is required";
                        break;
                    case "Path":
                        if (string.IsNullOrEmpty(path))
                            return "Application path is required";
                        else if (!File.Exists(path))
                            return "Application path does not exist";
                        break;
                    case "WorkDir":
                        if (!string.IsNullOrEmpty(workDir) && !Directory.Exists(path))
                            return "Working directory does not exist";
                        break;
                }
                return null;
            }
        }

        #endregion
    }
}
