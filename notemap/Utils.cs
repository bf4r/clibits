namespace notemap;

public static class Utils
{
    public static string GetFullPath(string path)
    {
        var fullPath = "";
        if (Path.IsPathRooted(path))
        {
            fullPath = path;
        }
        else if (path.StartsWith("~/"))
        {
            if (OperatingSystem.IsLinux())
                fullPath = $"/home/{Environment.UserName}/{path.Substring(2)}";
            else if (OperatingSystem.IsWindows())
                fullPath = $"C:\\Users\\{Environment.UserName}\\{path.Substring(2)}";
        }
        else
        {
            string workingDirectory = Directory.GetCurrentDirectory();
            fullPath = Path.Combine(workingDirectory, path);
        }
        return fullPath;
    }
}
