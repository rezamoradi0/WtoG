using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using MediaInfo;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms.ComponentModel;
namespace WtoG
{
  public  static class UploadManager
    {
       public static Form1 MyForm = null;
        static string UploadPathUrl = "ftp://asiatech.parsaspace.com:21/";
      public  static string UploadPathHttp = "http://asiatech.parsaspace.com/";
        static string FtpUser = "deltadl.parsaspace.com";
        static string FtpPass = "xxVJnHfhaMKDy2b";
        public static string FakeDLServer="http://deltadl.ir";
        public static bool CreateUploadBatch(this List<MovieFile> Movies) {
            foreach (var Movie in Movies)
            {
               
                string a ="Uploaded : " + Movie.CreateUpload(); 
            }
        return true;
        }
        public static bool CreateUpload(this MovieFile file) {
            if (file.GetFileType()== "Movie")
            {
             string MovieuploadPath=  CreateMoviePath(file);
                string M = UploadFile(file,MovieFile.MovieFolder + file.MovieName, UploadPathUrl+MovieuploadPath + file.MovieName);
        
              ///  Console.WriteLine( M);


            }
           else if (file.GetFileType() == "Serial")
            {
                string EpisodeuploadPath =CreateSeriesPath(file);
                string M = UploadFile(file,MovieFile.MovieFolder + file.MovieName, UploadPathUrl + EpisodeuploadPath + file.MovieName);
               /// Console.WriteLine(M);


            }




            return true;
        }


        public static string CreateSeriesPath(MovieFile Episode)
        {
           
            string FirstAlphabet= Episode.MovieName.Substring(0,1).ToUpper();
            string SerialSeason="S"+ Episode.GetSeasionNumber();
            int IndexOfSeason = Episode.MovieName.IndexOf(SerialSeason);
            string SerialName = Episode.MovieName.Substring(0, IndexOfSeason - 1);
            string SerialQuality=Episode.GetQuality();
            string[] path=new string[4] { FirstAlphabet,SerialName, SerialSeason, SerialQuality };
            string pathUrl = "Series/";
            foreach (var item in path)
            {
                pathUrl += item+"/";
                WebRequest webPathRequest = WebRequest.Create(UploadPathUrl + pathUrl);
                webPathRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                webPathRequest.Credentials = new NetworkCredential(FtpUser, FtpPass);
                try
                {
                    using (var resp = (FtpWebResponse)webPathRequest.GetResponse())
                    {


                       string a =resp.StatusCode.ToString();
                       
                    }
                }
                catch (Exception)
                {

                  ///  Console.WriteLine("Cant Create Path ! Mybe Existed");
                   
                }
            }
            return pathUrl;

        }
            public static string CreateMoviePath(MovieFile Movie) {


            string directory = "Movies/"+Movie.MovieName.Substring(0,1).ToUpper();
            WebRequest webPathRequest1 = WebRequest.Create(UploadPathUrl + directory + "/");
            webPathRequest1.Method = WebRequestMethods.Ftp.MakeDirectory;
            webPathRequest1.Credentials = new NetworkCredential(FtpUser, FtpPass);

            try
            {
                using (var resp = (FtpWebResponse)webPathRequest1.GetResponse())
                {


                   string a =resp.StatusCode.ToString();
                 
                }
            }
            catch (Exception)
            {

              ///  Console.WriteLine("Cant Create Path ! Mybe Existed");
            }

            directory +="/"+Movie.MovieName.Substring(0, Movie.MovieName.IndexOf(Movie.GetYear().ToString())+4)+"/";
           
           /// Console.WriteLine(directory);
           
            WebRequest webPathRequest2 = WebRequest.Create(UploadPathUrl+ directory);
            webPathRequest2.Method = WebRequestMethods.Ftp.MakeDirectory;
            webPathRequest2.Credentials = new NetworkCredential(FtpUser, FtpPass);
            try
            {
                using (var resp = (FtpWebResponse)webPathRequest2.GetResponse())
                {


                  string  a=resp.StatusCode.ToString();
                    return directory;
                }
            }
            catch (Exception)
            {

               /// Console.WriteLine("Cant Create Path ! Mybe Existed");
                return directory;
            }

            return "Error";
        }

        public static string UploadFile(MovieFile file,string filePath, string UploadPath) {

           
          


          

            string FileNameFull = file.MovieOutPathMkv.Remove(0, file.MovieOutPathMkv.LastIndexOf('\\')+1);
            //    MessageBox.Show(FileNameFull);

            //   MyForm.UploadFile(filePath, file.GetFileName());
          //  MessageBox.Show(file.MovieOutPathMkv);
          //  Clipboard.SetText(file.MovieOutPathMkv);
           // MessageBox.Show(FileNameFull);
        //    Clipboard.SetText(FileNameFull);
            MyForm.UploadFile(file.MovieOutPathMkv, FileNameFull);
            string _GAPLINK = "";
            MyForm.Uploaded(FileNameFull, ref _GAPLINK);


          //  MessageBox.Show(FileNameFull+"Test");
            /*
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(UploadPath);
            request.Credentials = new NetworkCredential(FtpUser, FtpPass);

            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream fileStream = File.OpenRead(filePath))
            using (Stream ftpStream = request.GetRequestStream())
            {
             
                byte[] buffer = new byte[10240];
                int read;
                string temp = "";
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ftpStream.Write(buffer, 0, read);
                    float uploadPercent = (fileStream.Position*100) / fileStream.Length;
                    string Progress = uploadPercent.ToString();
                   
                    if ( uploadPercent%10==0)
                    {
                       
                        if (temp!=Progress)
                        {

                            ///Console.Clear();    
                            ///Console.WriteLine("Uploaded : %" +Progress);
                            temp = Progress;
                        }
                       
                        
                    }
                  
                }
                file.MovieUploadPath = UploadPath.Replace(UploadPathUrl, UploadPathHttp);
            }
            */
            file.MovieUploadPath = UploadPath.Replace(UploadPathUrl, UploadPathHttp);
            file.GAPLINK = _GAPLINK;
            file.FakeLink= RidirectGap(file);
            return filePath;

        }
        public static string RidirectGap(MovieFile movie) {

            string MoviePATH = "";
            if (movie.GetFileType() == "Movie")
            {
                MoviePATH = movie.MovieUploadPath.Remove(0, movie.MovieUploadPath.ToLower().IndexOf("/m"));

            }
            else
            {
                MoviePATH = movie.MovieUploadPath.Remove(0, movie.MovieUploadPath.ToLower().IndexOf("/s"));

            }
            string WPPath = MoviePATH;
            bool Redirected = GetRedirect(MoviePATH, movie.GAPLINK);
            string FAKEPATH = FakeDLServer + MoviePATH;

            return FAKEPATH;


        }
        public static bool GetRedirect(string FakePath,string DownloadGap)
        {
         //   string SqlCOMMAND = @"INSERT INTO 'wp_redirection_items' ('id', 'url', 'match_url', 'match_data', 'regex', 'position', 'last_count', 'last_access', 'group_id', 'status', 'action_type', 'action_code', 'action_data', 'match_type', 'title') VALUES (NULL, 'UploadPath', 'UploadPath', NULL, '0', '0', '0', '1970-01-01 00:00:00.000000', '1', 'enabled', 'url', '301', 'GAPLINK', 'url', NULL);";

            string SqlCOMMAND = "INSERT INTO `wp_redirection_items` (`id`, `url`, `match_url`, `match_data`, `regex`, `position`, `last_count`, `last_access`, `group_id`, `status`, `action_type`, `action_code`, `action_data`, `match_type`, `title`) VALUES (NULL, 'UploadPath', 'UploadPath', NULL, '0', '0', '0', '1970-01-01 00:00:00.000000', '1', 'enabled', 'url', '301', 'GAPLINK', 'url', NULL)";
            SqlCOMMAND = SqlCOMMAND.Replace("UploadPath", FakePath);
            SqlCOMMAND = SqlCOMMAND.Replace("GAPLINK", DownloadGap);
            string PHPAdd = "http://bitiy.ir/robot.php?Req=";


            string Res=GetOMDB.Get(PHPAdd + SqlCOMMAND);
            if (Res == "true") {
                return true;
            }
            MessageBox.Show("ERROR ON RIDIRECT "+ Res + PHPAdd + SqlCOMMAND);
            return false;
        }

    }
}
