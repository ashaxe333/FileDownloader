using System.IO;

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
                string mode1 = Console.ReadLine();
                switch (mode1) {

                    case "1":
                        Console.WriteLine("Enter url adress of file to download: ");
                        string url = Console.ReadLine();
                        string path = GetPath();
                        d = new Downloader(url, path);
                        break;

                    case "2":
                        repeat = false;
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
                
            }
        }

        /// <summary>
        /// Recieve path in which user wants file to be saved.
        /// </summary>
        /// <returns> path to folder </returns>
        public static string GetPath()
        {
            bool repeat2 = true;
            string? path = "";

            while (repeat2)
            {
                Console.WriteLine("Choose folder? [y/n]");
                string mode2 = Console.ReadLine();
                switch (mode2)
                {
                    case "y":
                        Console.WriteLine("Enter path: ");
                        path = Console.ReadLine();
                        if (!Directory.Exists(path))
                        {
                            if (!CreateFolder(path)) 
                            { 
                                path = "";
                                continue;
                            }
                        }
                        repeat2 = false;
                        break;

                    case "n":
                        Console.WriteLine("File will be saved in 'Downloads'");
                        repeat2 = false;
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
            return path;
        }

        /// <summary>
        /// If folder in path does not exists, it will be, or not, be created here.
        /// </summary>
        /// <param name="path"> Path from user </param>
        /// <returns> if folder was created or not </returns>
        public static bool CreateFolder(string path)
        {
            bool repeat3 = true;
            bool created = false;
            while (repeat3)
            {
                Console.WriteLine("Folder does not exists. Create? [y/n]");
                string mode3 = Console.ReadLine();
                switch (mode3)
                {
                    case "y":
                        Directory.CreateDirectory(path);
                        repeat3 = false;
                        created = true;
                        break;
                    case "n":
                        repeat3 = false;
                        created = false;
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
            return created;
        }

    }
}
