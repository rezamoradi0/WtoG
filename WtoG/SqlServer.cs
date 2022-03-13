using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using WtoG;
using System.Net;
namespace MediaInfo
{
    public static class SqlServer
    {
        public static SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source= database.db; Version = 3; New = True; Compress = True; ");

        public static bool _InsertMovieBatch(this List<MovieFile> Movies)
        {
            foreach (var Movie in Movies)
            {
                Movie.IMDBID = Movie.GetIMDB_();
                GetOMDB.CallApi(Movie);
                Movie._InsertMovie();
                if (Movie.GetFileType() == "Movie")
                {
                    Movie._InsertMoviePro();
                }
                else if (Movie.GetFileType() == "Serial")
                {
                    Movie._InsertSeriePro();
                }

            }
            return true;
        }
        //translate
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
        public static string Translate(string EnText)
        {
            string WebAdd = "http://bitiy.ir/Translator/t.php?Req=" + EnText;
            string TranslatedText = Get(WebAdd);
            return TranslatedText;


        }


        //translate

        public static bool _InsertMovieNoBatch(this MovieFile Movie)
        {
           
                Movie.IMDBID = Movie.GetIMDB_();
                GetOMDB.CallApi(Movie);
                Movie._InsertMovie();
                if (Movie.GetFileType() == "Movie")
                {
                    Movie._InsertMoviePro();
                }
                else if (Movie.GetFileType() == "Serial")
                {
                    Movie._InsertSeriePro();
                }

            
            return true;
        }
        public static void _InsertSeriePro(this MovieFile movieFile)
        {
            bool HaveSeries = _CheckHaveSerie(movieFile);
            if (!HaveSeries)
            {
                bool a = _InsertSerieDB(movieFile);
            }
            else {
                _UpdateSerieDB(movieFile);
             

            }
            Console.WriteLine(HaveSeries);
        }
        public static bool _InsertSerieDB(MovieFile movie)
        {




            string insertCmd = "INSERT INTO Series  (ImdbId, Title, Year, Stars, Writer, Director, Genre, Plot, FaPlot, Seasons, Languge, Country, PosterLink, Metascore, ImdbRating, ImdbVotes) VALUES('#ImdbId', '#Title' , #Year, '#Stars','#Writer' ,'#Director' ,'#Genre' ,'#Plot' ,'#FaPlot' ,'#SEASONS' ,'#Languge' ,'#Country' ,'#PosterLink' ,'#Metascore' ,'#ImdbRating' ,'#ImdbVotes')";
            insertCmd = insertCmd.Replace("#ImdbId", movie.IMDBInfo.imdbID);
            insertCmd = insertCmd.Replace("#Title", movie.IMDBInfo.Title.Replace("'", "''"));
            insertCmd = insertCmd.Replace("#Year", movie.IMDBInfo.Year.Substring(0, 4));
            insertCmd = insertCmd.Replace("#Stars", _InsertStar(movie.IMDBInfo.Actors));
            //  insertCmd = insertCmd.Replace("#Stars", movie.IMDBInfo.Actors);
            insertCmd = insertCmd.Replace("#Writer", _InsertStar(movie.IMDBInfo.Writer));
            insertCmd = insertCmd.Replace("#Director", _InsertStar(movie.IMDBInfo.Director));
            insertCmd = insertCmd.Replace("#Genre", _InsertGenre(movie.IMDBInfo.Genre.Replace("-", " ")));
            insertCmd = insertCmd.Replace("#Plot", movie.IMDBInfo.Plot.Replace("'", "").Replace(",", " "));
            string FarsiPlot = Translate(movie.IMDBInfo.Plot.Replace("'", "").Replace(",", " "));
            //   insertCmd = insertCmd.Replace("#FaPlot", movie.IMDBInfo.Plot.Replace("'", "").Replace(",", " "));
            insertCmd = insertCmd.Replace("#FaPlot", FarsiPlot);

            insertCmd = insertCmd.Replace("#SEASONS", _InsertEpisode(movie.MovieUploadPath, movie));
            insertCmd = insertCmd.Replace("#Languge", _InsertLanguage(movie.IMDBInfo.Language));
            insertCmd = insertCmd.Replace("#Country", _InsertCountrys(movie.IMDBInfo.Country));
            insertCmd = insertCmd.Replace("#PosterLink", movie.IMDBInfo.Poster);
            insertCmd = insertCmd.Replace("#Metascore", movie.IMDBInfo.Metascore);
            insertCmd = insertCmd.Replace("#ImdbRating", movie.IMDBInfo.imdbRating);
            insertCmd = insertCmd.Replace("#ImdbVotes", movie.IMDBInfo.imdbVotes);


            _InsertData(insertCmd);
            Console.WriteLine("FINISHED SEries");
            
            return true;
        }
        public static bool _UpdateSerieDB(MovieFile movie)
        {
            _InsertEpisode(movie.MovieUploadPath, movie);

          //  string SeasonID = "";
          //  bool HAVESeason=_CheckHaveSeason(movie, ref SeasonID);


            //  if (HAVESeason)


            return true;
        }
        //public static bool SELECTSeason(ref string SeasinID, MovieFile movie) {
        //    string SeasonFolder = movie.MovieUploadPath;
        //    SeasonFolder = SeasonFolder.Substring(0, SeasonFolder.LastIndexOf('/') - 1);
        

        
        
        //}
        public static string _InsertEpisode(string Link, MovieFile movie)
        {
          
            string LinkID = Link;
            string SeasonID = "";
            bool HaveIt = _CheckHaveLink(ref LinkID);
            if (!HaveIt)
          
            {
                string HOSTAdd = movie.MovieUploadPath.Substring(0, movie.MovieUploadPath.ToLower().IndexOf("/s"));
                string MoviePATH = movie.MovieUploadPath.Remove(0, movie.MovieUploadPath.ToLower().IndexOf("/s"));
                string SQLCmd = "INSERT INTO Links (DLink, ContactID, Host, GapLink, Size, FileLength, Encoder, Quality, FakeLink, DLPath ,SourceQuality) VALUES ('DownlLink' ,'IMDBID','HOSTAdd','GAPLINK','SIZE','DURATION','ENCODER', 'QUALITY','FAKELINK', 'DLPATH', 'SOURCEQuality');";


                SQLCmd = SQLCmd.Replace("DownlLink", movie.MovieUploadPath);
                SQLCmd = SQLCmd.Replace("IMDBID", movie.IMDBID);
                SQLCmd = SQLCmd.Replace("HOSTAdd", UploadManager.UploadPathHttp);
                SQLCmd = SQLCmd.Replace("GAPLINK", movie.GAPLINK);
                SQLCmd = SQLCmd.Replace("SIZE", movie.GetFileSize().ToString());
                SQLCmd = SQLCmd.Replace("DURATION", movie.MovieFileLength.ToString());
                SQLCmd = SQLCmd.Replace("ENCODER", movie.GetEncoder(movie.MovieName));
                SQLCmd = SQLCmd.Replace("FAKELINK", movie.FakeLink);
                SQLCmd = SQLCmd.Replace("DLPATH", MoviePATH);
                SQLCmd = SQLCmd.Replace("QUALITY", movie.GetQuality());
                SQLCmd = SQLCmd.Replace("SOURCEQuality", movie.GetSourceQuality(movie.MovieName));
                _InsertData(SQLCmd);
                _CheckHaveLink(ref LinkID);


            }

            bool HaveSeason = _CheckHaveSeason(movie, ref SeasonID);
            if (!HaveSeason)
            {
                string SeasonFolder = movie.MovieUploadPath;
                SeasonFolder = SeasonFolder.Substring(0, SeasonFolder.LastIndexOf('/') - 1);
                string SQLCmdX = "INSERT INTO Season (SerieId , EpisodesId , Qulity , Encoder, FolderUrl) VALUES ('SERIESID' ,'EPISODESID' , 'QUALITY' , 'ENCODER' ,'FOLDERURL');";
                SQLCmdX = SQLCmdX.Replace("SERIESID", movie.IMDBID);
                SQLCmdX = SQLCmdX.Replace("EPISODESID","");
                SQLCmdX = SQLCmdX.Replace("QUALITY", (movie.GetQuality() +""+movie.GetSourceQuality(movie.MovieUploadPath)));
                SQLCmdX = SQLCmdX.Replace("ENCODER", movie.GetEncoder(movie.MovieUploadPath));
                SQLCmdX = SQLCmdX.Replace("FOLDERURL", SeasonFolder);
                _InsertData(SQLCmdX);
                _CheckHaveSeason(movie, ref SeasonID);


            }


            string Episode_ID = "";
            string EpisodNumber = movie.GetEpisodeNumber();

            bool HaveEpisode = _CheckHaveEpisode(EpisodNumber, LinkID, ref Episode_ID , SeasonID);
            if (!HaveEpisode)
            {
                string SQLCmd1 = "INSERT INTO Episode (SeasonID , EpisodeNumber , LinkID) VALUES (SID ,ENUM , LID);";
                SQLCmd1 = SQLCmd1.Replace("SID", SeasonID);
                SQLCmd1 = SQLCmd1.Replace("ENUM", EpisodNumber);
                SQLCmd1 = SQLCmd1.Replace("LID", LinkID);
                _InsertData(SQLCmd1);
                _CheckHaveEpisode(EpisodNumber, LinkID, ref Episode_ID, SeasonID);


                //----------------------------------------------------

                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                string COMMAND = "SELECT EpisodesId, * from Season Where rowid='SEASONROWID';";
                COMMAND = COMMAND.Replace("SEASONROWID", SeasonID);
                

                sqlite_cmd.CommandText = COMMAND;
                Console.WriteLine(COMMAND);
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                string LastEpisodes = "";
                while (sqlite_datareader.Read())
                {
                    string myreader = sqlite_datareader.GetString(0).ToString();
                    LastEpisodes = myreader;


                }


                //----------------------------------------------------------
                if (Episode_ID.Length >= 1)
                {
                    Episode_ID = LastEpisodes + "," + Episode_ID;
                }
                if (Episode_ID.IndexOf(",")==0)
                {
                    Episode_ID = Episode_ID.Remove(0, 1);
                }


                string UpdateCommand = "UPDATE Season SET EpisodesId = 'EpiID' WHERE rowid='SEASONID';";
                UpdateCommand = UpdateCommand.Replace("EpiID", Episode_ID);
                UpdateCommand = UpdateCommand.Replace("SEASONID", SeasonID);
                _InsertData(UpdateCommand);

            }




            return SeasonID;



        }



        public static void _InsertMoviePro(this MovieFile movieFile)
        {
            bool HaveMovie = _CheckHaveMovie(movieFile);
            if (!HaveMovie)
            {
                bool a = _InsertMovieDB(movieFile);
            }
            else {
                _UpdateMoveDB(movieFile);

            }
            Console.WriteLine(HaveMovie);
        }

        public static bool _UpdateMoveDB(MovieFile movie) {

            string IMDBId = movie.IMDBID;
            string LinkID = _InsertLink(movie.MovieUploadPath, movie);
            if (_GetAllLinks(ref IMDBId))
            {
                string LinkIDs = IMDBId;
                string[] ALlLinks = LinkIDs.Split(',');


                
                bool HaveThislink = false;
                foreach (string Link in ALlLinks)
                {
                    if (Convert.ToInt32(LinkID) == Convert.ToInt32(Link))
                    {
                        HaveThislink = true;
                        break;
                    }
                }
                if (HaveThislink)
                {
                    return true;
                }
                else
                {
                    string NewLinks = LinkIDs + "," + LinkID;
                    string UpdateCommand = "UPDATE Movies SET Links = 'LINKS' WHERE ImdbId='IMDBID';";
                    UpdateCommand = UpdateCommand.Replace("LINKS", NewLinks);
                    UpdateCommand = UpdateCommand.Replace("IMDBID", movie.IMDBID);
                    _InsertData(UpdateCommand);
                    return true;

                }
            }
            else {
               
                string UpdateCommand = "UPDATE Movies SET Links = 'LINKS' WHERE ImdbId='IMDBID';";
                UpdateCommand = UpdateCommand.Replace("LINKS", LinkID);
                UpdateCommand = UpdateCommand.Replace("IMDBID", movie.IMDBID);
                _InsertData(UpdateCommand);
                return true;
            }

        }
        public static bool _GetAllLinks(ref string MovieIMDBID)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string COMMAND = "SELECT Links, * from Movies Where ImdbId='IMDBID';";

            COMMAND = COMMAND.Replace("IMDBID", MovieIMDBID);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0).ToString();
                MovieIMDBID = myreader;

                return true;


            }


            return false;



        }
        public static bool _InsertMovie(this MovieFile Movie)
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {

                //is open 
            }
            string insertCmd = "INSERT INTO Files  (Duration, MovieName, SubtitlePath, MovieYear, FileType, Seasion, Episode, MovieFullName, UploadPath, FileSize, IMDBID, Quality ,GapLink) VALUES(DURATION, 'MOVIENAME' , 'SUBTITLEPATH',YEAR ,'FILETYPE' ,'SEASION' ,'EPISODE' ,'MOVIEFULLNAME' ,'UPLOADPATH' ,'FILESIZE' ,'_IMDBID_' ,'QUALITY' ,'GAPLINK')";
            insertCmd = insertCmd.Replace("DURATION", Movie.MovieFileLength.ToString());
            insertCmd = insertCmd.Replace("MOVIENAME", Movie.MovieNameOrginal());
            insertCmd = insertCmd.Replace("SUBTITLEPATH", Movie.GetBestSubtitle().SubtitlePath);
            insertCmd = insertCmd.Replace("MOVIEFULLNAME", Movie.MovieName);
            insertCmd = insertCmd.Replace("YEAR", Movie.GetYear().ToString());
            insertCmd = insertCmd.Replace("FILETYPE", Movie.GetFileType());
            insertCmd = insertCmd.Replace("SEASION", Convert.ToInt32(Movie.GetSeasionNumber()).ToString());
            insertCmd = insertCmd.Replace("EPISODE", Movie.GetEpisodeNumber());
            insertCmd = insertCmd.Replace("UPLOADPATH", Movie.MovieUploadPath);
            insertCmd = insertCmd.Replace("FILESIZE", Movie.GetFileSize().ToString());
            insertCmd = insertCmd.Replace("_IMDBID_", Movie.GetIMDB_());
            insertCmd = insertCmd.Replace("QUALITY", Movie.GetQuality());
            insertCmd = insertCmd.Replace("GAPLINK", Movie.GAPLINK);
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = insertCmd;
            sqlite_cmd.ExecuteNonQuery();

         
            return true;


        }
        public static bool _CheckHaveEpisode( string EpisodeNUM,string LINK_ID  , ref string E_ID ,string SeasonID)
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {

                //is open 
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            //  string COMMAND = "SELECT * FROM Movies Where Title='MOVIENAME' AND Year=MOVIEYEAR ;";
            string COMMAND = "SELECT rowid, * FROM Episode Where EpisodeNumber=URL AND LinkID=LINKID AND SeasonID=SEASONS;";
           
            COMMAND = COMMAND.Replace("URL", EpisodeNUM);
            COMMAND = COMMAND.Replace("LINKID", LINK_ID);
            COMMAND = COMMAND.Replace("SEASONS", SeasonID);
            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                E_ID = myreader;
                return true;

            }

            return false;



        }
        public static bool _CheckHaveSeason(this MovieFile movieFile ,ref string S_ID)
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {

                //is open 
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            //  string COMMAND = "SELECT * FROM Movies Where Title='MOVIENAME' AND Year=MOVIEYEAR ;";
            string COMMAND = "SELECT rowid, * FROM Season Where FolderUrl='URL' ;";
            string SeasonFolder = movieFile.MovieUploadPath;
            SeasonFolder = SeasonFolder.Substring(0, SeasonFolder.LastIndexOf('/') - 1);


            COMMAND = COMMAND.Replace("URL", SeasonFolder);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                S_ID = myreader;
                return true;

            }

            return false;



        }
        public static bool _CheckHaveSerie(this MovieFile movieFile)
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {

                //is open 
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            //  string COMMAND = "SELECT * FROM Movies Where Title='MOVIENAME' AND Year=MOVIEYEAR ;";
            string COMMAND = "SELECT * FROM Series Where ImdbId='ID' ;";

            COMMAND = COMMAND.Replace("ID", movieFile.IMDBID);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                return true;

            }

            return false;



        }
        public static bool _CheckHaveMovie(this MovieFile movieFile)
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {

                //is open 
            }
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
          //  string COMMAND = "SELECT * FROM Movies Where Title='MOVIENAME' AND Year=MOVIEYEAR ;";
            string COMMAND = "SELECT * FROM Movies Where ImdbId='ID' ;";

            COMMAND = COMMAND.Replace("ID",movieFile.IMDBID);
     
            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                return true;

            }
           
            return false;



        }

        public static bool _InsertMovieDB(MovieFile movie)
        {




            string insertCmd = "INSERT INTO Movies  (ImdbId, Title, Year, Stars, Writer, Director, Genre, Plot, FaPlot, Links, Languge, Country, PosterLink, Metascore, ImdbRating, ImdbVotes) VALUES('#ImdbId', '#Title' , #Year, '#Stars','#Writer' ,'#Director' ,'#Genre' ,'#Plot' ,'#FaPlot' ,'#Links' ,'#Languge' ,'#Country' ,'#PosterLink' ,'#Metascore' ,'#ImdbRating' ,'#ImdbVotes')";
            insertCmd = insertCmd.Replace("#ImdbId", movie.IMDBInfo.imdbID);
            insertCmd = insertCmd.Replace("#Title", movie.IMDBInfo.Title.Replace("'","''"));
            insertCmd = insertCmd.Replace("#Year", movie.IMDBInfo.Year.Substring(0, 4));
            insertCmd = insertCmd.Replace("#Stars", _InsertStar(movie.IMDBInfo.Actors));
            //  insertCmd = insertCmd.Replace("#Stars", movie.IMDBInfo.Actors);
            insertCmd = insertCmd.Replace("#Writer", _InsertStar(movie.IMDBInfo.Writer));
            insertCmd = insertCmd.Replace("#Director", _InsertStar(movie.IMDBInfo.Director));
            insertCmd = insertCmd.Replace("#Genre", _InsertGenre(movie.IMDBInfo.Genre.Replace("-", " ")));
            insertCmd = insertCmd.Replace("#Plot", movie.IMDBInfo.Plot.Replace("'", "").Replace(",", " "));
            string FarsiPlot = Translate(movie.IMDBInfo.Plot.Replace("'", "").Replace(",", " "));
            insertCmd = insertCmd.Replace("#FaPlot", FarsiPlot);
            insertCmd = insertCmd.Replace("#Links", _InsertLink(movie.MovieUploadPath, movie));
            insertCmd = insertCmd.Replace("#Languge", _InsertLanguage(movie.IMDBInfo.Language));
            insertCmd = insertCmd.Replace("#Country", _InsertCountrys(movie.IMDBInfo.Country));
            insertCmd = insertCmd.Replace("#PosterLink", movie.IMDBInfo.Poster);
            insertCmd = insertCmd.Replace("#Metascore", movie.IMDBInfo.Metascore);
            insertCmd = insertCmd.Replace("#ImdbRating", movie.IMDBInfo.imdbRating);
            insertCmd = insertCmd.Replace("#ImdbVotes", movie.IMDBInfo.imdbVotes);

        
            _InsertData(insertCmd);
            return true;
        }
        public static void _InsertData(string SqlCommand)
        {
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {

                //is open 
            }
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = SqlCommand;
            sqlite_cmd.ExecuteNonQuery();
          




        }

        public static string _InsertLink(string Link, MovieFile movie)
        {
            string LinkID = Link;
            bool HaveIt = _CheckHaveLink(ref LinkID);
            if (HaveIt)
            {
                return LinkID;
            }
            else
            {
                string HOSTAdd = "";
                string MoviePATH = "";
                if (movie.GetFileType() == "Movie")
                {
                    HOSTAdd = movie.MovieUploadPath.Substring(0, movie.MovieUploadPath.ToLower().IndexOf("/m"));
                    MoviePATH = movie.MovieUploadPath.Remove(0, movie.MovieUploadPath.ToLower().IndexOf("/m"));

                }
                else {
                    HOSTAdd = movie.MovieUploadPath.Substring(0, movie.MovieUploadPath.ToLower().IndexOf("/s"));
                    MoviePATH = movie.MovieUploadPath.Remove(0, movie.MovieUploadPath.ToLower().IndexOf("/s"));

                }
                string SQLCmd = "INSERT INTO Links (DLink, ContactID, Host, GapLink, Size, FileLength, Encoder, Quality, FakeLink, DLPath ,SourceQuality) VALUES ('DownlLink' ,'IMDBID','HOSTAdd','GAPLINK','SIZE','DURATION','ENCODER', 'QUALITY','FAKELINK', 'DLPATH', 'SOURCEQuality');";


                SQLCmd = SQLCmd.Replace("DownlLink", movie.MovieUploadPath);
                SQLCmd = SQLCmd.Replace("IMDBID", movie.IMDBID);
                SQLCmd = SQLCmd.Replace("HOSTAdd", UploadManager.UploadPathHttp);
                SQLCmd = SQLCmd.Replace("GAPLINK", movie.GAPLINK);
                SQLCmd = SQLCmd.Replace("SIZE", movie.GetFileSize().ToString());
                SQLCmd = SQLCmd.Replace("DURATION", movie.MovieFileLength.ToString());
                SQLCmd = SQLCmd.Replace("ENCODER", movie.GetEncoder(movie.MovieName));
                SQLCmd = SQLCmd.Replace("FAKELINK", movie.FakeLink);
                SQLCmd = SQLCmd.Replace("DLPATH", MoviePATH);
                SQLCmd = SQLCmd.Replace("QUALITY", movie.GetQuality());
                SQLCmd = SQLCmd.Replace("SOURCEQuality", movie.GetSourceQuality(movie.MovieName));
                _InsertData(SQLCmd);
                _CheckHaveLink(ref LinkID);
                    

                return LinkID;

            }
        }
        public static bool _CheckHaveLink(ref string TheLink)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string COMMAND = "SELECT rowid, * from Links Where DLink='DownLoadLink';";
         
            COMMAND = COMMAND.Replace("DownLoadLink", TheLink);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                TheLink = myreader;

                return true;


            }


            return false;



        }
        public static string _InsertLanguage(string _Language)
        {

            string[] Languages = _Language.Split(',');
            string AllLanguages = "";
            foreach (var item in Languages)
            {
                if (item.Length > 2)
                {
                    string Language_ = item;
                    while (Language_.IndexOf(' ') == 0)
                    {
                        Language_ = Language_.Remove(0, 1);
                    }
                    while (Language_.LastIndexOf(' ') == Language_.Length - 1)
                    {
                        Language_ = Language_.Remove(Language_.Length - 1, 1);
                    }
                    bool HaveIt = _CheckHaveLanguage(ref Language_);
                    if (HaveIt)
                    {
                        AllLanguages += Language_ + ",";
                    }
                    else
                    {
                        string SQLCmd = "INSERT INTO Language (EnglishName , PersianName) VALUES ('EngNAME' ,'PERSIONNAME');";
                        SQLCmd = SQLCmd.Replace("EngNAME", Language_);
                        _InsertData(SQLCmd);
                        _CheckHaveLanguage(ref Language_);


                        AllLanguages += Language_ + ",";




                    }
                }


            }
            AllLanguages = AllLanguages.Substring(0, AllLanguages.Length - 1);
            return AllLanguages;
        }
        public static bool _CheckHaveLanguage(ref string LanguageName)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string COMMAND = "SELECT rowid, * from Language Where EnglishName='EngNAME';";
          
            COMMAND = COMMAND.Replace("EngNAME", LanguageName);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                LanguageName = myreader;

                return true;


            }


            return false;



        }
        public static string _InsertCountrys(string _Country)
        {

            string[] Countrys = _Country.Split(',');
            string AllCountrys = "";
            foreach (var item in Countrys)
            {
                if (item.Length > 2)
                {
                    string Country_ = item;
                    while (Country_.IndexOf(' ') == 0)
                    {
                        Country_ = Country_.Remove(0, 1);
                    }
                    while (Country_.LastIndexOf(' ') == Country_.Length - 1)
                    {
                        Country_ = Country_.Remove(Country_.Length - 1, 1);
                    }
                    bool HaveIt = _CheckHaveCountry(ref Country_);
                    if (HaveIt)
                    {
                        AllCountrys += Country_ + ",";
                    }
                    else
                    {
                        string SQLCmd = "INSERT INTO Country (EnglishName , PersianName) VALUES ('EngNAME' ,'PERSIONNAME');";
                        SQLCmd = SQLCmd.Replace("EngNAME", Country_);
                        _InsertData(SQLCmd);
                        _CheckHaveCountry(ref Country_);


                        AllCountrys += Country_ + ",";




                    }
                }


            }
            AllCountrys = AllCountrys.Substring(0, AllCountrys.Length - 1);
            return AllCountrys;
        }
        public static bool _CheckHaveCountry(ref string CountrayName)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string COMMAND = "SELECT rowid, * from Country Where EnglishName='EngNAME';";
           
            COMMAND = COMMAND.Replace("EngNAME", CountrayName);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                CountrayName = myreader;

                return true;


            }


            return false;



        }
        public static string _InsertStar(string _Stars)
        {

            string[] Stars = _Stars.Split(',');
            string AllStars = "";
            foreach (var item in Stars)
            {
                if (item.Length > 4)
                {
                    string Star_ = item;
                    while (Star_.IndexOf(' ') == 0)
                    {
                        Star_ = Star_.Remove(0, 1);
                    }
                    while (Star_.LastIndexOf(' ') == Star_.Length - 1)
                    {
                        Star_ = Star_.Remove(Star_.Length - 1, 1);
                    }
                    bool HaveIt = _CheckHaveStar(ref Star_);
                    if (HaveIt)
                    {
                        AllStars += Star_ + ",";
                    }
                    else
                    {
                        string SQLCmd = "INSERT INTO Stars (Name , ImageLink) VALUES ('NAME' ,'IMGLINK');";
                        SQLCmd = SQLCmd.Replace("NAME", Star_);
                        _InsertData(SQLCmd);
                        _CheckHaveStar(ref Star_);


                        AllStars += Star_ + ",";




                    }
                }


            }
            if (AllStars.Length<1)
            {
                return " ";
            }
            AllStars = AllStars.Substring(0, AllStars.Length - 1);
            return AllStars;
        }
        public static bool _CheckHaveStar(ref string StarName)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string COMMAND = "SELECT rowid, * from Stars Where Name='STARNAME';";
           
            COMMAND = COMMAND.Replace("STARNAME", StarName);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                StarName = myreader;

                return true;


            }


            return false;



        }

        public static string _InsertGenre(string _Genres)
        {

            string[] Genres = _Genres.Split(',');
            string AllGenres = "";
            foreach (var item in Genres)
            {
                if (item.Length > 2)
                {
                    string Genre_ = item;
                    while (Genre_.IndexOf(' ') == 0)
                    {
                        Genre_ = Genre_.Remove(0, 1);
                    }
                    while (Genre_.LastIndexOf(' ') == Genre_.Length - 1)
                    {
                        Genre_ = Genre_.Remove(Genre_.Length - 1, 1);
                    }
                    bool HaveIt = _CheckHaveGenre(ref Genre_);
                    if (HaveIt)
                    {
                        AllGenres += Genre_ + ",";
                    }
                    else
                    {
                        string SQLCmd = "INSERT INTO Genre (EnglishName , PersianName) VALUES ('ENGNAME' ,'PERNAME');";
                        SQLCmd = SQLCmd.Replace("ENGNAME", Genre_);
                        _InsertData(SQLCmd);
                        _CheckHaveGenre(ref Genre_);
                        

                        AllGenres += Genre_ + ",";




                    }
                }


            }
            AllGenres = AllGenres.Substring(0, AllGenres.Length - 1);
            return AllGenres;
        }
        public static bool _CheckHaveGenre(ref string GenreName)
        {
           
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string COMMAND = "SELECT rowid, * from Genre Where EnglishName='GENRENAME';";
          
            COMMAND = COMMAND.Replace("GENRENAME", GenreName);

            sqlite_cmd.CommandText = COMMAND;
            Console.WriteLine(COMMAND);
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetInt64(0).ToString();
                GenreName = myreader;
                
                return true;


            }

     
            return false;



        }

    }
}
