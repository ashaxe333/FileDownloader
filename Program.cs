namespace FileDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool repeat = true;
            Downloader d;
            while (repeat)
            {
                Console.WriteLine("1 - download\n2 - exit");
                string mode = Console.ReadLine();
                switch (mode) {
                    case "1":
                        Console.WriteLine("Enter url adress of file to download: ");
                        string url = Console.ReadLine();
                        d = new Downloader(url);
                        break;
                    case "2":
                        repeat = false;
                        break;
                    default:
                        throw new Exception("Invalid input");
                }
                
            }
        }
    }
}
