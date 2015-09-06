namespace HttpLauncher.Models
{
    public class AppInfo
    {
        private string name;
        private string path;
        private string args;
        private string workDir;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Args
        {
            get { return args; }
            set { args = value; }
        }

        public string WorkDir
        {
            get { return workDir; }
            set { workDir = value; }
        }
    }
}
