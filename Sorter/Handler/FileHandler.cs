namespace Sorter.Handler
{
    public struct HandlerArgs
    {
        public Dictionary<string, string> ExtensionMaps;
    }

    /// <summary>
    /// This class will take in a directory and handle the file adequately depending on its file extension.
    /// </summary>
    public class FileHandler
    {
        private HandlerArgs _handlerArgs;

        public FileHandler(HandlerArgs args)
        {
            _handlerArgs = args;
        }

        public static implicit operator FileHandler(HandlerArgs fileConfig) => new(fileConfig);

        public void Handle(string eFullPath, WatcherChangeTypes eChangeType)
        {
            string fileExtension = $"*{Path.GetExtension(eFullPath)}";
            string destinationPath = _handlerArgs.ExtensionMaps[fileExtension];

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            File.Move(eFullPath, Path.Combine(destinationPath, Path.GetFileName(eFullPath)));
        }
    }
};