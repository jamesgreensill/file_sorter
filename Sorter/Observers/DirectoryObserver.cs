using Sorter.Helpers;

namespace Sorter.Observers
{
    public class DirectoryObserver
    {
        private readonly FileSystemWatcher? _directoryWatcher;
        private readonly string? _directory;

        public FileSystemEventHandler? OnChanged;
        public FileSystemEventHandler? OnCreated;
        public FileSystemEventHandler? OnDeleted;
        public RenamedEventHandler? OnRenamed;

        /// <summary>
        /// Construct a DirectoryObserver with a string as the file location.
        /// </summary>
        /// <param name="directoryToObserve"></param>
        /// <param name="filter"></param>
        public DirectoryObserver(string directoryToObserve, NotifyFilters filter = NotifyFilters.Attributes)
        {
            if (!Directory.Exists(directoryToObserve))
            {
                Console.WriteLine($"Failed to create observer @ {directoryToObserve}. As it does not exist.");
                return;
            }

            _directory = directoryToObserve;

            _directoryWatcher = new(directoryToObserve);
            _directoryWatcher.NotifyFilter = filter;

            _directoryWatcher.Changed += (sender, args) => OnChanged?.Invoke(sender, args);
            _directoryWatcher.Created += (sender, args) => OnCreated?.Invoke(sender, args);
            _directoryWatcher.Deleted += (sender, args) => OnDeleted?.Invoke(sender, args);
            _directoryWatcher.Renamed += (sender, args) => OnRenamed?.Invoke(sender, args);
            _directoryWatcher.Error += OnError;

            _directoryWatcher.Filter = "";
            _directoryWatcher.EnableRaisingEvents = true;
        }

        public void AddFilter(string fileExtension)
        {
            Console.WriteLine($"Adding Extension Filter: {fileExtension} to observer watching directory: {_directory}.");
            _directoryWatcher?.Filters.Add(fileExtension);
        }

        public void WatchSubDirectories(bool flag) => _directoryWatcher!.IncludeSubdirectories = flag;

        protected void OnError(object sender, ErrorEventArgs e) => DebugHelper.PrintException(e.GetException());
    }
}