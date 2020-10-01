using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IoTExcercise.Utils
{
    public class FilePathsUtils
    {
        public static string GenerateDirectoryPath(string deviceId, string sensor)
        {
            var path = Path.Combine(deviceId, sensor);
            path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return path;
        }
        public static string GenerateCsvDateFilePath(string directoryPath, DateTime date)
        {
            string path;
            path = Path.Combine(directoryPath, date.ToString("yyyy-MM-dd") + ".csv");
            path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return path;
        }
        public static string GenerateZipFilePath(string directory, string zipFileName)
        {
            var path = Path.Combine(directory, zipFileName + ".zip");
            path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return path;
        }
    }
}
