namespace FileDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter url adress of file to download: ");
            string url = Console.ReadLine();
            Downloader d = new Downloader(url);
        }
    }
}
