using Sorter.Handler;
using Sorter.Observers;

namespace Sorter;

public struct SorterArgs
{
    public HandlerArgs handlerArgs;
    public List<string> DirectoriesToObserve;
}

public class FileSorter
{
    public FileHandler? Handler;

    private readonly DirectoryObserver[] _observers;

    private readonly SorterArgs _args;

    public FileSorter(string configDirectory = "")
    {
        if (string.IsNullOrEmpty(configDirectory))
        {
            if (File.Exists(@"./config.json"))
            {
                configDirectory = @"./config.json";
            }
            else return;
        }

        string? _argsJson = Helpers.FileHelper.Read(configDirectory);

        if (string.IsNullOrEmpty(_argsJson))
        {
            Console.WriteLine($"Failed to load config file @ {configDirectory}.");
            return;
        }

        _args = Helpers.JSONHelper.Deserialize<SorterArgs>(_argsJson);

        Handler = _args.handlerArgs;

        if (_args.DirectoriesToObserve != null)
        {
            _observers = new DirectoryObserver[_args.DirectoriesToObserve.Count];
            for (int i = 0; i < _observers.Length; i++)
            {
                _observers[i] = new DirectoryObserver(_args.DirectoriesToObserve[i], NotifyFilters.Attributes
                    | NotifyFilters.CreationTime
                    | NotifyFilters.DirectoryName
                    | NotifyFilters.FileName
                    | NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.Security
                    | NotifyFilters.Size);
            }
        }
    }

    public void Start()
    {
        foreach (var observer in _observers)
        {
            observer.OnCreated += OnFileCreated;
            observer.OnDeleted += OnFileDeleted;
            observer.OnRenamed += OnFileRenamed;
            observer.OnChanged += OnFileChanged;

            foreach (var ex in _args.handlerArgs.ExtensionMaps)
            {
                observer.AddFilter(ex.Key);
            }
        }
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        Handler?.Handle(e.FullPath, e.ChangeType);
    }

    private void OnFileRenamed(object sender, RenamedEventArgs e)
    {
        Handler?.Handle(e.FullPath, e.ChangeType);
    }

    private void OnFileDeleted(object sender, FileSystemEventArgs e)
    {
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        //Handler?.Handle(e.FullPath, e.ChangeType);
    }
}