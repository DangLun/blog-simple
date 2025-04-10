namespace Contract.Constants
{
    public static class Const
    {
        public const string UPLOAD_DIRECTORY = "Contract\\Assets\\Uploads";
        public const string REQUEST_PATH_STATIC_FILE = "uploads";

        #region
        public static string GetSolutionDir()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] dirs = baseDir.Split("\\");

            string slnDir = string.Empty;

            foreach (string dir in dirs)
            {
                slnDir = Path.Combine(slnDir, dir);
                if (dir.Equals("src")) break;
            }
            return slnDir;
        }
        #endregion  
    }
}
