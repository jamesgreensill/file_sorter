using Newtonsoft.Json;

namespace Sorter.Helpers
{
    public static class DebugHelper
    {
        public static void PrintException(Exception? ex)
        {
            if (ex == null) return;
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

    public static class JSONHelper
    {
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    public static class FileHelper
    {
        public static string? Read(string path)
        {
            // read string contents of a file
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return null;
        }

        public static bool Write(string data, string directory)
        {
            if (!File.Exists(directory))
            {
                File.WriteAllText(directory, data);
                return true;
            }

            return false;
        }
    }
};