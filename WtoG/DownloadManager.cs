using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using WtoG;
namespace MediaInfo
{
    public static class DownloadManager
    {

        public static int QueuCount = 8;
       public static bool InProgress = false;
        public static string IDMPath = @"C:\Program Files (x86)\Internet Download Manager\";
      public  static string DonwloadLinksTxt = @"C:\SubtitleBotPlugins\DownloadLinks.txt";
        public static string WrongTxt = @"C:\SubtitleBotPlugins\WrongLinks.txt";
        public static List<Link> GetLinksFromText() {
            List<Link> links = new List<Link>();
        
            string[] LINKS = File.ReadAllLines(DonwloadLinksTxt);
            foreach (var item in LINKS)
            {
                Link _LINK = new Link();
                _LINK.Url = item;
                links.Add(_LINK);
               
            }
        return links;
        
        }
        public static bool RemoteFileExists(string _url)
        {
           
            try
            {
                string url = _url.Replace("https", "http");
             

                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                request.UseDefaultCredentials = true;
                request.PreAuthenticate = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
             //   System.Windows.Forms.MessageBox.Show("Returns TRUE if the Status code == 200");
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
             //   System.Windows.Forms.MessageBox.Show("Any exception will returns false.");
                //Any exception will returns false.
                return false;
            }
        }
        
        public static bool AddToQueu(this List<Link> AllLink)
        {
          
            int i = 0;
            int allLinkNum = AllLink.Count;
            while (AllLink.Count>0)
            {
               
                if (RemoteFileExists(AllLink[0].Url)&& GetFileSize(AllLink[0].Url)<1450)
                {
                   
                    if (!Idm_open())
                    {
                        StartIDM();
                        UploadManager.MyForm.wait(3000);
                       // Task.Delay(3000);
                      //  Thread.Sleep(3000);
                    }
                 
                    AddIDM(AllLink[0].Url);
                    AllLink.RemoveAt(0);
                    UploadManager.MyForm.wait(3000);
                    //  Task.Delay(3000);
                }
                else {
                    string WrongUrl=AllLink[0].Url;
                    
                    using (StreamWriter w = File.AppendText(WrongTxt))
                    {
                        w.WriteLine(WrongUrl);
                    }
                    AllLink.RemoveAt(0);
                }

                if (i== QueuCount -1|| i== allLinkNum-1)
                {
                    
                  
                    StartQueu();
                    InProgress = true;
                    while (Idm_open())
                    {
                        UploadManager.MyForm.wait(3000);
                        // Thread.Sleep(3000);
                        //   Console.WriteLine("In Idm open");
                        // wait here
                    }
                   // System.Windows.Forms.MessageBox.Show("In JobQeue");
                    c.Job();
                    while (InProgress)
                    {
                        
                        //Console.WriteLine("In Progress");
                    }

                    allLinkNum -= QueuCount;
                      i = 0;
                }
               // Console.WriteLine(i);
                i++;
            }
            return true;

        }


        public static void AddIDM(string URL) {
            
            string AddLinkCommand = @"idman.exe /n /q /a /d FileLink  /f FileName";
            string[] MovieParts = URL.Split('/');
            string MovieName=MovieParts[MovieParts.Length-1].Replace("%28", ".").Replace("%29", ".").Replace("%20", ".").Replace("[", ".").Replace("]", ".");
            MovieName = MovieName.Replace("..", ".");

               AddLinkCommand = AddLinkCommand.Replace("FileName", MovieName);
            AddLinkCommand= AddLinkCommand.Replace("FileLink", URL);
           // Console.WriteLine(AddLinkCommand);
          
            CMD.RunCMDIDM(AddLinkCommand, IDMPath);
           
        
        }

        public static void StartQueu() {

            string startCommand = @"idman.exe /s";
            CMD.RunCMDIDM(startCommand,IDMPath);
        }
        public static void StartIDM()
        {

            string startCommand = @"idman.exe ";
            CMD.RunCMDIDM(startCommand, IDMPath);
        }
        public static bool Idm_open()
        {
            Process[] myProcesses = Process.GetProcesses();
            bool is_OPEN = false;


            foreach (Process P in myProcesses)
            {
                
            //    if (P.ProcessName.ToLower().Contains("idm") || P.ProcessName.ToLower().Contains("idm.exe") || P.ProcessName.Contains("Internet Download Manager 6.29") || P.ProcessName.Contains("Internet Download Manager (IDM)") /*&&true|| P.MainWindowTitle.Contains(".720p") || P.MainWindowTitle.Contains(".x265") || P.MainWindowTitle.Contains(".x264") || P.MainWindowTitle.Contains(".480p") || P.MainWindowTitle.Contains(".1080p") || P.MainWindowTitle.Contains(".2160p") || P.MainWindowTitle.Contains(".1440p") || P.MainWindowTitle.Contains(".720p") || P.MainWindowTitle.ToLower().Contains("webdl") || P.MainWindowTitle.ToLower().Contains("bluray") || P.MainWindowTitle.Contains(".480.") || P.MainWindowTitle.Contains(".1080.") || P.MainWindowTitle.Contains(".2160.")*/)
              
                if(P.ProcessName.ToLower()=="idman")
                {
                    is_OPEN = true;

                  //  System.Windows.Forms.MessageBox.Show(P.ProcessName.ToLower());
                    return is_OPEN;
                }
               

            }
            return false;
        }

        public static int GetFileSize(string _url)
        {
           // return 1;
            string url = _url.Replace("https", "http");
            long result = -1;

            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
           
            req.Method = "HEAD";
            using (System.Net.WebResponse resp = req.GetResponse())
            {
                if (long.TryParse(resp.Headers.Get("Content-Length"), out long ContentLength))
                {
                    result = ContentLength/1024;
                    result = result / 1024;
                }
            }

            return Convert.ToInt32( result);
        }
    }
}
