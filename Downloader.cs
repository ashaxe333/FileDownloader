using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class Downloader
    {
        private string? url;
        private string? path;
        private string? name;

        private long workerSpace;
        private List<Thread> threads = new List<Thread>();

        private volatile byte[] data;
        private NameValidation nameValidation = new NameValidation();

        public string Url { get => url; set => url = value; }
        public string? Path { get => path; set => path = value; }
        public string? Name { get => name; set => name = value; }

        public Downloader(string url, string path, string name)
        {
            this.Url = url;
            this.Path = path;
            this.Name = name;

            MakeWorkers();
        }

        /// <summary>
        /// method that gets size of file.
        /// </summary>
        /// <returns> size of file. </returns>
        /// <exception cref="Exception"> Throes exception, if file couldn't be found. </exception>
        public long GetFileSize()
        {
            long size = 0;
            try
            {
                //vtvoří http požadavek s metodou HEAD
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);       //!!!Zjistit existenci už v MyConsole!!!
                request.Method = "HEAD";

                //získá response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine(Url);
                    //zkusí získat velikost, a pokud ji dostane, uloží ji a vrátí
                    if (long.TryParse(response.Headers.Get("Content-Length"), out size)) return size;
                    else throw new Exception("file couldn't be found");
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("file couldn't be found", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("file couldn't be found", ex.Message);
            }
            return size;
        }

        /// <summary>
        /// Creates threads for downloading.
        /// </summary>
        public void MakeWorkers()
        {
            long size = GetFileSize();
            workerSpace = size/Program.coreCount;   //!!!nepřesný výpočet. Na konci může být zbytek, který se nikdy nestáhne!!!
            //int workerCount = (int)Math.Ceiling((double)size/workerSpace);
            data = new byte[size];

            for (int i = 0; i < Program.coreCount; i++)
            {
                long start = i * workerSpace;
                long end = Math.Min(start + workerSpace, size); //Vybere menší hodnotu z těch dvou (Ochrana posledního vlákna. Na konci to nikdy nebude přesně)
                threads.Add(new Thread(() => Download(start, end)));
            }

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

            SaveFile();
            //Console.WriteLine($"size: {size}");
            //Console.WriteLine($"workerCount: {threads.Count}");
            //Console.WriteLine($"workerSpace: {workerSpace}");
        }

        /// <summary>
        /// Downloads file paralerly with threads, and store file in array data.
        /// </summary>
        /// <param name="start"> starting byte on file, where threads start downloading. Unique for every thread. </param>
        /// <param name="end"> ending byte on file, where threads end downloading. Unique for every thread. </param>
        public void Download(long start, long end)
        {
            //server pošle jen danný rozsah
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.AddRange(start, end - 1);

            //získám odpověď
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //otevřu stream, kde budu číst data
                using (Stream stream = response.GetResponseStream())
                {
                    int bytesRead;
                    long offset = start;
                    byte[] buffer = new byte[8192];

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        //pole, které kopíruju/odkud začínám v tom poli/místo kam kopíruju/odkud v místě kam kopíruju začínám kopírovat dál/kolik toho zkopíruju
                        Array.Copy(buffer, 0, data, offset, bytesRead);
                        offset += bytesRead;
                    }
                }
            }
        }

        /// <summary>
        /// Saves all bytes from array data to file in Downloads folder on computer
        /// </summary>
        public void SaveFile()
        {
            string fileName = Url.Split('/').Last();

            if (nameValidation.ContainsProhibitedCharacters(fileName) || string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "new_file" + Program.nameCounter.ToString();
                Program.nameCounter++;
            }
            
            if (string.IsNullOrWhiteSpace(path))
            {
                path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName); 
            }
            else
            {
                if (path.ToArray()[path.ToArray().Length-1] == '/' || path.ToArray()[path.ToArray().Length - 1] == '\\') path = path + fileName;
                else path = path + "\\" + fileName;
            }

            /*
            string fullPath = Path.Combine(path, name);

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

            File.WriteAllBytes(path, data);
            Console.WriteLine($"Saved to {path}");
        }
    }
}
