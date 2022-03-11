using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace MediaInfo
{
    public static class GetOMDB
    {
        static string[] AllApi = { "81f36b6", "ad45c727", "6c9d8f99", "de1f6e94" };
        static int UsedApi = 0;
        static string ApiCommand = "https://www.omdbapi.com/?apikey=APITOKEN&i=IMDBID&plot=full";
        public static string ApiId() {
            int temp = UsedApi;
            if (UsedApi >= AllApi.Length)
            {
                UsedApi = 0;
                temp = UsedApi;
            }
            else {
                UsedApi++;
            }
            

            return AllApi[temp];


        }
        public static bool CallApi(this MovieFile Movie) {
            string Command = ApiCommand.Replace("APITOKEN", ApiId());
            Command = Command.Replace("IMDBID", Movie.IMDBID);
            

            dynamic stuff = JsonConvert.DeserializeObject(Get(Command));
            IMDBInfo iMDBInfo=new IMDBInfo();
            iMDBInfo.Title = stuff.Title;
            iMDBInfo.Year = stuff.Year;
            iMDBInfo.Released = stuff.Released;
            iMDBInfo.Genre = stuff.Genre;
            iMDBInfo.Director = stuff.Director;
            iMDBInfo.Director = iMDBInfo.Director.Replace("'", "");
            iMDBInfo.Writer = stuff.Writer;
            iMDBInfo.Writer= iMDBInfo.Writer.Replace("'", "");
            iMDBInfo.Actors = stuff.Actors;
            iMDBInfo.Actors = iMDBInfo.Actors.Replace("'", "");
            iMDBInfo.Plot = stuff.Plot;
            iMDBInfo.Language = stuff.Language;
            iMDBInfo.Country = stuff.Country;
            iMDBInfo.Awards = stuff.Awards;
            iMDBInfo.Poster = stuff.Poster;
            iMDBInfo.Metascore = stuff.Metascore;
            iMDBInfo.imdbRating = stuff.imdbRating;
            iMDBInfo.imdbVotes = stuff.imdbVotes;
            iMDBInfo.imdbID = stuff.imdbID;
            iMDBInfo.Type = stuff.Type;
            iMDBInfo.totalSeasons = stuff.totalSeasons;

            Movie.IMDBInfo = iMDBInfo;

         return true;

        }
        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
          
           
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
