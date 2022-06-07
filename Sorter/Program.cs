namespace Sorter
{
    internal static class Program
    {
        private static void Main()
        {
            FileSorter sorter = new(@"./config.json");
            sorter.Start();

            Console.Read();
        }
    }
}