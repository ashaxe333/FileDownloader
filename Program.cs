using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FileDownloader
{
    internal class Program
    {
        // NÁPADY:
        // 1) Uživatel si může název souboru zadat sám + !může si říct, jaký formát to bude mít!
        // 2) Program bude moct přijímat řetezec více odkazů naráz (oddělené třeba čárkou), a bude moct stahovat více věcí z jednoho příkazu
        // 3) Přendad dialogovou část do jiné třídy, kterou zde jen vytvořím, a už pojede
        // 4) Shrnutí co kam bude uloženo
        // 5) Líp vychytávat podmínku neexistujícího linku

        public static int nameCounter = 1;
        public static int coreCount;

        static void Main(string[] args)
        {
            try
            {
                coreCount = Environment.ProcessorCount;
                Console.WriteLine("Corecount: " + coreCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't get actuall core count -> coreCount = 4", ex);
                coreCount = 4;
            }

            MyConsole();
        }

        public static void MyConsole()
        {
            bool repeat = true;
            Downloader d;

            while (repeat)
            {
                Console.WriteLine("1 - download\n2 - exit");
                string mode1 = Console.ReadLine().Trim();
                switch (mode1)
                {

                    case "1":
                        string url = GetUrl();
                        string? path = GetFolderPath();
                        //string? name = GetFileName();
                        string? name = "";
                        d = new Downloader(url, path, name);
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
        public static string GetUrl()
        {
            bool repeat = true;
            string? url = "";

            while (repeat)
            {
                Console.WriteLine("Enter url adress of file to download: ");
                url = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(url))
                {
                    repeat = false;
                    Console.WriteLine("url cannot be empty string");
                }
                else break;
            }

            return url;
        }

        /// <summary>
        /// Recieve path in which user wants file to be saved.
        /// </summary>
        /// <returns> path to folder </returns>
        public static string GetFolderPath()
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
                        
                        if (string.IsNullOrEmpty(path))
                        {
                            Console.WriteLine("path cannot be empty string");
                            continue;
                        }
                        
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
                        Directory.CreateDirectory(path);    //File.Create(path);
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
        public static string GetFileName()
        {
            bool repeat4 = true;
            string? name = "";
            string? type = "";

            while (repeat4)
            {
                Console.WriteLine("Name file? [y/n]");
                string mode4 = Console.ReadLine().Trim();
                switch (mode4)
                {
                    case "y":
                        //zatím budu uživateli věřit, že to pojmenuje zprávně
                        Console.WriteLine("File name: ");
                        name = Console.ReadLine();
                        type = GetFileType();
                        break;

                    case "n":
                        Console.WriteLine("File will be named as New_File<number>");
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
            return name+type;
        }

        public static string GetFileType()
        {
            bool repeat4 = true;
            string? type = "";

            while (repeat4)
            {
                Console.WriteLine("Choose type (.png, .bin, .pdf, etc...)? [y/n]");
                string mode4 = Console.ReadLine().Trim();
                switch (mode4)
                {
                    case "y":
                        //zatím budu uživateli věřit, že zadá zprávně existující typ
                        Console.WriteLine("Type (in .<type> format): ");
                        type = Console.ReadLine();
                        break;

                    case "n":
                        Console.WriteLine("File type will be same as in url");
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
            return type;
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
