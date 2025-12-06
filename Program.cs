using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FileDownloader
{
    internal class Program
    {
        // NÁPADY:
        // 1) Uživatel si může název souboru zadat sám + !může si říct, jaký formát to bude mít!
        // 2) Program bude moct přijímat řetezec více odkazů naráz (oddělené třeba čárkou), a bude moct stahovat více věcí z jednoho příkazu
        // 3) Trochu přeorganizovat, a líp vychytávat podmínku neexistujícího linku

        public static int nameCounter = 1;

        static void Main(string[] args)
        {
            bool repeat = true;
            Downloader d;

            while (repeat)
            {
                Console.WriteLine("1 - download\n2 - exit");
                string mode1 = Console.ReadLine().Trim();
                switch (mode1) {

                    case "1":
                        Console.WriteLine("Enter url adress of file to download: ");
                        string url = Console.ReadLine().Trim();
                        string path = GetPath();
                        //string name = NameFile();
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
                string mode2 = Console.ReadLine().Trim();
                switch (mode2)
                {
                    case "y":
                        Console.WriteLine("Enter folder path: ");
                        path = Console.ReadLine().Trim();
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
                string mode3 = Console.ReadLine().Trim();
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
        /*
        public static string NameFile()
        {
            bool repeat4 = true;
            string? name = "";

            while (repeat4)
            {
                Console.WriteLine("Name file? [y/n]");
                string mode4 = Console.ReadLine();
                switch (mode4)
                {
                    case "y":
                        //zatím budu uživateli věřit, že to pojmenuje zprávně
                        name = Console.ReadLine();
                        break;

                    case "n":

                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
            return name;
        }
        */
        /*
        string folderPath = "...";    // cesta složky
        string userFileName = "soubor.txt"; // jméno, které zadal uživatel
        string fullPath = Path.Combine(folderPath, userFileName);

        if (File.Exists(fullPath))
        {
            Console.WriteLine("Soubor s tímto názvem už existuje!");
            return;
        }
        else
        {
            Console.WriteLine("Soubor je volný, můžeš pokračovat se stahováním.");
        }
        */
    }
}
