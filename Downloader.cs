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
        private long workerSpace = 10_000_000;
        private List<Thread> threads = new List<Thread>();
        private byte[] data;

        public string Url { get => url; set => url = value; }

        public Downloader(string url)
        {
            Url = url;

            MakeWorkers();
        }

        /// <summary>
        /// method that gets size of file
        /// </summary>
        /// <returns> size of file </returns>
        /// <exception cref="Exception"> Throes exception, if file couldn't be found </exception>
        public long GetFileSize()
        {
            long size = 0;

            //vtvoří http požadavek s metodou HEAD
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "HEAD";

            //získá response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //zkusí získat velikost, a pokud ji dostane, uloží ji a vrátí
                if (long.TryParse(response.Headers.Get("Content-Length"), out size)) return size;
                else throw new Exception("file couldn't be found");
            }
        }

        /// <summary>
        /// Creates threads for downloading
        /// </summary>
        public void MakeWorkers()
        {
            long size = GetFileSize();
            int workerCount = (int)Math.Ceiling((double)size/workerSpace);
            data = new byte[size];

            for (int i = 0; i < workerCount; i++)
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

            Console.WriteLine(size);
            Console.WriteLine(workerCount);
            Console.WriteLine(workerSpace);
        }

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
                        //pole, které kopíruju/odkud začínám v tom poli/místo kam kopíruju/odkud místě kam kopíruju začínám kopírovat dál/kolik toho zkopíruju
                        Array.Copy(buffer, 0, data, offset, bytesRead);
                        offset += bytesRead;
                    }
                }
            }
            
        }
    }
}
