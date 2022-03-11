using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using WtoG;
namespace MediaInfo
{
    public class MovieFile
    {
        //close Folder with BackSlash /
        public static string MovieFolder { get; set; }
        public static string OutPutMovieFolder { get; set; }

        public static string SubtitleSaveFolder = @"C:\SubtitleBotPlugins\AllSubtitle\";
        public string MovieName { get; set; }
        //     public int MovieLength { get; set; }
        public int MovieFileLength { get; set; }
        public float MovieSize { get; set; }

        public string MovieUploadPath { get; set; }
        public string MovieOutPathMkv { get; set; }
        public IMDBInfo IMDBInfo { get; set; }
        public static string LastMovieName { get; set; }
        public static string LastIMDBID { get; set; }
        public string IMDBID { get; set; }
        public string FileType = "unknow";
        public string MovieYear = "0";
        public string GAPLINK { set; get; }
        public string FakeLink { set; get; }
        public List<SubtitleFile> Subtitles { get; set; }
        public SubtitleFile ChosedSubtitle=new SubtitleFile();
        public MovieFile() { 
        Subtitles = new List<SubtitleFile>();
           OutPutMovieFolder= "C:\\SubtitleBotPlugins\\OutPutFolder\\";
            MovieFolder = @"C:\SubtitleBotPlugins\OriginalMovies\";


        }
        public  string CreateOutPutPath() {
            string output = OutPutMovieFolder+ ReplaceName(MovieName);
            MovieOutPathMkv = output;
            return output;


        }
        static string ReplaceName(string name) {

            string MovieName = name;
            MovieName = MovieName.Replace("[OkMovie]", "-DeltaMovieS");
            MovieName = MovieName.Replace("[OkMovie.xyz]", "-DeltaMovieS");
            MovieName = MovieName.Replace("OkMovie.xyz", "DeltaMovieS");
            MovieName = MovieName.Replace(".OkMovie", ".DeltaMovieS");
            MovieName = MovieName.Replace("OkMovie", ".DeltaMovieS");
            MovieName = MovieName.Replace("okMovie", ".DeltaMovieS");
            MovieName = MovieName.Replace("Okmovie", ".DeltaMovieS");
            MovieName = MovieName.Replace("okmovie", ".DeltaMovieS");
            MovieName = MovieName.Replace("[@archive_series]", "-DeltaMovieS");
            MovieName = MovieName.Replace("@Archive_series]", "-DeltaMovieS");
            MovieName = MovieName.Replace("@archive_series", "DeltaMovieS");
            MovieName = MovieName.Replace("@Archive_Series", "DeltaMovieS");
            MovieName = MovieName.Replace("@archive_Series", "DeltaMovieS");
            MovieName = MovieName.Replace(".archive_series", ".DeltaMovieS");
            MovieName = MovieName.Replace("archive_series", ".DeltaMovieS");

            // Mobo
            MovieName = MovieName.Replace("(1)", "");
            MovieName = MovieName.Replace("Delta", "MetaL");
            MovieName = MovieName.Replace(".per", "");
            MovieName = MovieName.Replace(".und", "");
            MovieName = MovieName.Replace("mobomovie", "");
            MovieName = MovieName.Replace(" ", ".");
            MovieName = MovieName.Replace("-MovieApp.OnGooglePlay", "");
            MovieName = MovieName.Replace("-MovieApp.OnGooglPlay", "");
            MovieName = MovieName.Replace(".-", ".");
            MovieName = MovieName.Replace(".MovieApp", "");
            MovieName = MovieName.Replace(".Movieapp", "");
            MovieName = MovieName.Replace("_.", ".");
            MovieName = MovieName.Replace("%20", "");
            MovieName = MovieName.Replace("_", ".");

            //DibaMovie
            MovieName = MovieName.Replace("(DibaMovie)", "");
            MovieName = MovieName.Replace("DibaMovie", "");
            MovieName = MovieName.Replace("dibamovie", "");
            MovieName = MovieName.Replace("Diba", "");
            MovieName = MovieName.Replace("diba", "");


            //topmoviez
            MovieName = Regex.Replace(MovieName, "TopMoviez", "", RegexOptions.IgnoreCase);
            MovieName = Regex.Replace(MovieName, "TopMoviez", "", RegexOptions.IgnoreCase);
            MovieName = Regex.Replace(MovieName, "TopMovies", "", RegexOptions.IgnoreCase);
            MovieName = Regex.Replace(MovieName, "TopMovie", "", RegexOptions.IgnoreCase);





            //seasons

            MovieName = MovieName.Replace("s01e", "S01E").Replace("s02e", "S02E").Replace("s03e", "S03E").Replace("s04e", "S04E").Replace("s05e", "S05E").Replace("s06e", "S06E").Replace("s07e", "S07E").Replace("s08e", "S08E").Replace("s09e", "S09E");
            MovieName = MovieName.Replace("s10e", "S10E").Replace("s11e", "S11E").Replace("s12e", "S12E").Replace("s13e", "S13E").Replace("s14e", "S14E").Replace("s15e", "S15E").Replace("s16e", "S16E").Replace("s17e", "S17E").Replace("s18e", "S18E").Replace("s19e", "S19E");
            MovieName = MovieName.Replace("s20e", "S20E").Replace("s21e", "S21E").Replace("s22e", "S22E").Replace("s23e", "S23E").Replace("s24e", "S24E").Replace("s25e", "S25E").Replace("s26e", "S26E").Replace("s27e", "S27E").Replace("s28e", "S28E").Replace("s29e", "S29E");



            MovieName = MovieName.Replace("psa", "PSA");
            MovieName = MovieName.Replace("rmt", "RMT");
            MovieName = MovieName.Replace("rmteam", "RMT"); MovieName = MovieName.Replace("SoftSub", "Sub");
            MovieName = Regex.Replace(MovieName, "DigiMoviez", "DeltaMovieS", RegexOptions.IgnoreCase);
            if (!MovieName.ToLower().Contains("Delta"))
            {
                MovieName = MovieName.Replace(".mkv", "-DeltaMovieS.mkv");

            }
            MovieName = MovieName.Replace("DeltaMovieS", "");
            MovieName = MovieName.Replace("DeltaMovies", "");
            MovieName = MovieName.Replace(".mkv", ".DeltaMovieS.mkv");
            MovieName = MovieName.Replace(".-.", ".");
            return MovieName;

        }
        public string CreateInPutPath()
        {
            string input = MovieFolder + MovieName;
            return input;


        }
        public SubtitleFile GetBestSubtitle() {
            if (Subtitles.Count == 0)
            {
             //   Console.WriteLine("No Subtitle For : {0}", MovieName);
               // Console.ReadLine();
                return null;
            }
            else if (Subtitles.Count == 1)
            {
                return Subtitles[0];
            }
            else { 
          SubtitleFile subtitleFile = new SubtitleFile();
                foreach (var Sub in Subtitles)
                {
                    if (Sub.Format==".srt"&&Sub.Languge=="per")
                    {
                        subtitleFile=Sub;
                        ChosedSubtitle = Sub;
                        return Sub;
                        break;
                    }else if (Sub.Format == ".srt" && Sub.Languge != "eng")
                    {
                        if (subtitleFile.Languge!= "per")
                        {
                            subtitleFile = Sub;
                            ChosedSubtitle = Sub;
                            return Sub;
                            break;
                        }
                      
                    }

                }
                ChosedSubtitle = subtitleFile;
                return subtitleFile;

            }
        }

        public string GetFileType() {
            if (
                MovieName.ToLower().Contains("s01e") || MovieName.ToLower().Contains("s02e") || MovieName.ToLower().Contains("s03e") || MovieName.ToLower().Contains("s04e") || MovieName.ToLower().Contains("s05e") || MovieName.ToLower().Contains("s06e") || MovieName.ToLower().Contains("s07e") || MovieName.ToLower().Contains("s08e") || MovieName.ToLower().Contains("s09e") || MovieName.ToLower().Contains("s10e") || MovieName.ToLower().Contains("s11e") || MovieName.ToLower().Contains("s12e") || MovieName.ToLower().Contains("s13e") || MovieName.ToLower().Contains("s14e") || MovieName.ToLower().Contains("s15e") || MovieName.ToLower().Contains("s16e") || MovieName.ToLower().Contains("s17e") || MovieName.ToLower().Contains("s18e") || MovieName.ToLower().Contains("s19e") || MovieName.ToLower().Contains("s20e") || MovieName.ToLower().Contains("s21e")
               || MovieName.ToLower().Contains("s22e") || MovieName.ToLower().Contains("s23e") || MovieName.ToLower().Contains("s24e") || MovieName.ToLower().Contains("s25e") || MovieName.ToLower().Contains("s26e") || MovieName.ToLower().Contains("s27e") || MovieName.ToLower().Contains("s28e") || MovieName.ToLower().Contains("s29e") || MovieName.ToLower().Contains("s30e") || MovieName.ToLower().Contains("s31e") || MovieName.ToLower().Contains("s32e") || MovieName.ToLower().Contains("s33e") || MovieName.ToLower().Contains("s34e") || MovieName.ToLower().Contains("s35e") || MovieName.ToLower().Contains("s36e") || MovieName.ToLower().Contains("s37e") || MovieName.ToLower().Contains("s38e") || MovieName.ToLower().Contains("s39e") || MovieName.ToLower().Contains("s40e") || MovieName.ToLower().Contains("s41e") || MovieName.ToLower().Contains("s42e")
                )
            {
                FileType = "Serial";
                return "Serial";
            }
            FileType = "Movie";
            return "Movie";
        }
        public string GetSeasionNumber() {
            string seasionNumber = "Empity";
            if (GetFileType()=="Serial")
            {

                if (MovieName.ToLower().Contains("s01e"))
                    seasionNumber = "01";
                else if (MovieName.ToLower().Contains("s02e"))
                    seasionNumber = "02";
                else if (MovieName.ToLower().Contains("s03e"))
                    seasionNumber = "03";
                else if (MovieName.ToLower().Contains("s04e"))
                    seasionNumber = "04";
                else if (MovieName.ToLower().Contains("s05e"))
                    seasionNumber = "05";
                else if (MovieName.ToLower().Contains("s06e"))
                    seasionNumber = "06";
                else if (MovieName.ToLower().Contains("s07e"))
                    seasionNumber = "07";
                else if (MovieName.ToLower().Contains("s08e"))
                    seasionNumber = "08";
                else if (MovieName.ToLower().Contains("s09e"))
                    seasionNumber = "09";
                else if (MovieName.ToLower().Contains("s10e"))
                    seasionNumber = "10";
                else if (MovieName.ToLower().Contains("s11e"))
                    seasionNumber = "11";
                else if (MovieName.ToLower().Contains("s12e"))
                    seasionNumber = "12";
                else if (MovieName.ToLower().Contains("s13e"))
                    seasionNumber = "13";
                else if (MovieName.ToLower().Contains("s14e"))
                    seasionNumber = "14";
                else if (MovieName.ToLower().Contains("s15e"))
                    seasionNumber = "15";
                else if (MovieName.ToLower().Contains("s16e"))
                    seasionNumber = "16";
                else if (MovieName.ToLower().Contains("s17e"))
                    seasionNumber = "17";
                else if (MovieName.ToLower().Contains("s18e"))
                    seasionNumber = "18";
                else if (MovieName.ToLower().Contains("s19e"))
                    seasionNumber = "19";
                else if (MovieName.ToLower().Contains("s20e"))
                    seasionNumber = "20";
                else if (MovieName.ToLower().Contains("s21e"))
                    seasionNumber = "21";
                else if (MovieName.ToLower().Contains("s22e"))
                    seasionNumber = "22";
                else if (MovieName.ToLower().Contains("s23e"))
                    seasionNumber = "23";
                else if (MovieName.ToLower().Contains("s24e"))
                    seasionNumber = "24";
                else if (MovieName.ToLower().Contains("s25e"))
                    seasionNumber = "25";
                else if (MovieName.ToLower().Contains("s26e"))
                    seasionNumber = "26";
                else if (MovieName.ToLower().Contains("s27e"))
                    seasionNumber = "27";
                else if (MovieName.ToLower().Contains("s28e"))
                    seasionNumber = "28";
                else if (MovieName.ToLower().Contains("s29e"))
                    seasionNumber = "29";
                else if (MovieName.ToLower().Contains("s30e"))
                    seasionNumber = "30";
                else if (MovieName.ToLower().Contains("s31e"))
                    seasionNumber = "31";
                else if (MovieName.ToLower().Contains("s32e"))
                    seasionNumber = "32";
                else if (MovieName.ToLower().Contains("s33e"))
                    seasionNumber = "33";
                else if (MovieName.ToLower().Contains("s34e"))
                    seasionNumber = "34";
                else if (MovieName.ToLower().Contains("s35e"))
                    seasionNumber = "35";
                else if (MovieName.ToLower().Contains("s36e"))
                    seasionNumber = "36";
                else if (MovieName.ToLower().Contains("s37e"))
                    seasionNumber = "37";
                else if (MovieName.ToLower().Contains("s38e"))
                    seasionNumber = "38";
                else if (MovieName.ToLower().Contains("s39e"))
                    seasionNumber = "39";
                else if (MovieName.ToLower().Contains("s40e"))
                    seasionNumber = "40";

                return seasionNumber;
            }
            return "0";
        }
        public string GetQuality() {

            string Quality = "";
            if (MovieName.ToLower().Contains("720x265") || MovieName.ToLower().Contains("720px265") || MovieName.ToLower().Contains("x265") && MovieName.ToLower().Contains("720p") || MovieName.ToLower().Contains("hevc") && MovieName.ToLower().Contains("720p"))
                Quality = "720x265";
            else if (MovieName.ToLower().Contains("1080x265") || MovieName.ToLower().Contains("1080x265") || MovieName.ToLower().Contains("x265") && MovieName.ToLower().Contains("1080p") || MovieName.ToLower().Contains("hevc") && MovieName.ToLower().Contains("1080p"))
                Quality = "1080x265";
            else if (MovieName.ToLower().Contains("720p"))
                Quality = "720p";
            else if (MovieName.ToLower().Contains("480p"))
                Quality = "480p";
            else if (MovieName.ToLower().Contains("1080p"))
                Quality = "1080p";
            return Quality;
        }
        public string GetEpisodeNumber() {
            if (GetFileType() == "Serial")
            {
                string seasionNumber = GetSeasionNumber();
                string Episode = "";
                int IndexOfEpisode = MovieName.IndexOf(seasionNumber) + 3;
                Episode = MovieName.Substring(IndexOfEpisode,2);
              int EpisodeNumber=Convert.ToInt32(Episode);
           
           return EpisodeNumber.ToString();
            }
            return "Empity";
        }
        public int GetYear() {
            if (GetFileType() == "Movie") { 
             string AllYears = "2024,2023,2022,2021,2020,2019,2018,2017,2016,2015,2014,2013,2012,2011,2010,2009,2008,2007,2006,2005,2004,2003,2002,2001,2000,1999,1998,1997,1996,1995,1994,1993,1992,1991,1990,1989,1988,1987,1986,1985,1984,1983,1982,1981,1980,1979,1978,1977,1976,1975,1974,1973,1972,1971,1970,1969,1968,1967,1966,1965,1964,1963,1962,1961,1960,1959,1958,1957,1956,1955,1954,1953,1952,1951,1950,1949,1948,1947,1946,1945,1944,1943,1942,1941,1940,1939,1938";

            string[] years = AllYears.Split(',');
                int i = 0;
            foreach (var year in years)
            {
                if (MovieName.Contains("." + year.Replace(",", "") + "."))
                {
                        if (i==0)
                        {
                            MovieYear = year.Replace(",", "");
                            i++;
                        }
                        else if (i==1)
                        {
                            MovieYear = year.Replace(",", "");
                            return Convert.ToInt32(MovieYear);
                        }

                       

                    }
            }


            }
            return Convert.ToInt32(MovieYear);


        }
        public string MovieNameOrginal() {
            string Name = "Empity";
            if (GetFileType()== "Serial")
            {
                int Season = Convert.ToInt32(GetSeasionNumber());
                int IndexOfSeason = MovieName.IndexOf(GetSeasionNumber());
                 Name = MovieName.Substring(0, IndexOfSeason - 2);
                return Name;

            }
            else if  (GetFileType() == "Movie")
                {
                int MovieYear =GetYear();
                int IndexOfMovieYear = MovieName.IndexOf(GetYear().ToString());
                Name = MovieName.Substring(0, IndexOfMovieYear - 1);
                return Name;
            }
            return Name;


        }


        /// <summary>
        /// این متد برای دریافت اسم فیلم یا سریال هست برای سرچ در گوگل استفاده میشه
        /// </summary>
        /// <returns>خروجی اسم سریال یا اسم فیلم به همراه سال هست</returns>
        public string GetFileName() {
            if (GetFileType()== "Serial")
            {
                string SeasonNumb=GetSeasionNumber();
               int indexOfSeasonNumb=MovieName.IndexOf(SeasonNumb);

                string FileName = MovieName.Substring(0, indexOfSeasonNumb - 1);
                return FileName;
            }
            else if (GetFileType() == "Movie")
            {
                string MovieYear =GetYear().ToString();
                int indexOfMovieYear = MovieName.IndexOf(MovieYear);

                string FileName = MovieName.Substring(0, indexOfMovieYear - 1);
                FileName += " " + GetYear().ToString();
                return FileName;
            }
            return MovieName;
        }
        public string GetFileName(int a)
        {
            if (GetFileType() == "Serial")
            {
                string SeasonNumb = GetSeasionNumber();
                int indexOfSeasonNumb = MovieName.IndexOf(SeasonNumb);

                string FileName = MovieName.Substring(0, indexOfSeasonNumb - 1);
                return FileName;
            }
            else if (GetFileType() == "Movie")
            {
                string MovieYear = GetYear().ToString();
                int indexOfMovieYear = MovieName.IndexOf(MovieYear);

                string FileName = MovieName.Substring(0, indexOfMovieYear - 1);
              
                return FileName;
            }
            return MovieName;
        }
        public int GetFileSize() {
          long file_Byte = new  FileInfo(MovieFolder + MovieName).Length;
            int fileSize = (int)file_Byte / 1048576;
            MovieSize = fileSize;
            return fileSize;
        }

        public string GetIMDB_() { 
        
            string FileName = GetFileName();
            if (FileName == LastMovieName)
            {
                return LastIMDBID;
            }
            else {
                Console.WriteLine("GetIMDBID GOOGLE !");
                LastIMDBID= GetIMDB.GetIMDBid(this);
                LastMovieName = FileName;
                return LastIMDBID;
            }
        }

        public string GetEncoder(string Link)
        {
            foreach (string l in this.EncoderPSA)
            {
                foreach (string i8 in this.EncoderRARBG)
                {
                    if (Link.ToLower().Contains(i8.ToLower()))
                    {
                        return "RARBG";
                    }
                }
                foreach (string i9 in this.EncoderSPARK)
                {
                    if (Link.ToLower().Contains(i9.ToLower()))
                    {
                        return "SPARK";
                    }
                }
                if (Link.ToLower().Contains(l.ToLower()))
                {
                    return "PSA";
                }
            }
            foreach (string m in this.EncoderRMT)
            {
                if (Link.ToLower().Contains(m.ToLower()))
                {
                    return "RMT";
                }
            }
            foreach (string n in this.EncodersMkvcage)
            {
                if (Link.ToLower().Contains(n.ToLower()))
                {
                    return "MKVCage";
                }
            }
            foreach (string i2 in this.EncoderYIFY)
            {
                if (Link.ToLower().Contains(i2.ToLower()))
                {
                    return "YIFY";
                }
            }
            foreach (string i in this.EncoderGanool)
            {
                if (Link.ToLower().Contains(i.ToLower()))
                {
                    return "Ganool";
                }
            }
            foreach (string i3 in this.EncoderMETCON)
            {
                if (Link.ToLower().Contains(i3.ToLower()))
                {
                    return "METCON";
                }
            }
            foreach (string i4 in this.EncoderGECKOS)
            {
                if (Link.ToLower().Contains(i4.ToLower()))
                {
                    return "GECKOS";
                }
            }
            foreach (string i5 in this.EncoderPaHe)
            {
                if (Link.ToLower().Contains(i5.ToLower()))
                {
                    return "PaHe";
                }
            }
            foreach (string j in this.EncoderHet)
            {
                if (Link.ToLower().Contains(j.ToLower()))
                {
                    return "Het";
                }
            }
            /*  foreach (string k in this.EncoderION)
              {
                  if (Link.ToLower().Contains(k.ToLower()))
                  {
                      return "ION";
                  }
              }*/


            foreach (string i6 in this.EncoderHetSub)
            {
                if (Link.ToLower().Contains(i6.ToLower()))
                {
                    return "HetSub";
                }
            }
            foreach (string i7 in this.EncoderTBS)
            {
                if (Link.ToLower().Contains(i7.ToLower()))
                {
                    return "TBS";
                }
            }

            return " ";
        }

        public string GetSourceQuality(string Link)
        {
            string qual = "";
            foreach (string i in this.QualityBluRay)
            {
                if (Link.ToLower().Contains(i.ToLower()))
                {
                    qual = "BluRay";
                }
            }
            foreach (string j in this.QualityDVDRip)
            {
                if (Link.ToLower().Contains(j.ToLower()))
                {
                    qual = "DVDRip";
                }
            }
            foreach (string k in this.QualityHDRip)
            {
                if (Link.ToLower().Contains(k.ToLower()))
                {
                    qual = "HDRip";
                }
            }
            foreach (string l in this.Qualitywebdl)
            {
                if (Link.ToLower().Contains(l.ToLower()))
                {
                    qual = "WebDL";
                }
            }
            foreach (string m in this.Qualitywebrip)
            {
                if (Link.ToLower().Contains(m.ToLower()))
                {
                    qual = "WebRip";
                }
            }
            foreach (string n in this.Qualityxvid)
            {
                if (Link.ToLower().Contains(n.ToLower()))
                {
                    qual = "Xvid";
                }
            }
            foreach (string i2 in this.QualityHDTV)
            {
                if (Link.ToLower().Contains(i2.ToLower()))
                {
                    qual = "HDTV";
                }
            }
            if (Link.ToLower().Contains("x265"))
            {
                qual += " x265";
            }
            if (Link.ToLower().Contains("10bit"))
            {
                qual += " 10bit";
            }
            return qual;
        }











        private List<string> EncoderRMT = new List<string>
        {
            "rmt",
            "rmteam",
            "RMT",
            "RMZ",
            "rmz"
        };

        // Token: 0x04000004 RID: 4
        private List<string> EncoderPSA = new List<string>
        {
            "PSA",
            "PSARip",
            "psa"
        };

        // Token: 0x04000005 RID: 5
        private List<string> EncodersMkvcage = new List<string>
        {
            "Mkvcage"
        };

        // Token: 0x04000006 RID: 6
        private List<string> EncoderGanool = new List<string>
        {
            "Ganool"
        };

        // Token: 0x04000007 RID: 7
        private List<string> EncoderHet = new List<string>
        {
            "Het",
            "HetTeam"
        };

        // Token: 0x04000008 RID: 8
        private List<string> EncoderYIFY = new List<string>
        {
            "YIFY",
            "Yts"
        };

        // Token: 0x04000009 RID: 9
        private List<string> EncoderION = new List<string>
        {
            "ION"
        };

        // Token: 0x0400000A RID: 10
        private List<string> EncoderMETCON = new List<string>
        {
            "METCON"
        };

        // Token: 0x0400000B RID: 11
        private List<string> EncoderGECKOS = new List<string>
        {
            "GECKOS"
        };

        // Token: 0x0400000C RID: 12
        private List<string> EncoderPaHe = new List<string>
        {
            "PaHe",
            "Pahe","pahe"
        };

        // Token: 0x0400000D RID: 13
        private List<string> EncoderHetSub = new List<string>
        {
            "Het.Sub"
        };

        // Token: 0x0400000E RID: 14
        private List<string> EncoderTBS = new List<string>
        {
            "tbs",
            "tbs"
        };

        // Token: 0x0400000F RID: 15
        private List<string> EncoderRARBG = new List<string>
        {
            "RARBG",
            "rarbg"
        };

        // Token: 0x04000010 RID: 16
        private List<string> EncoderSPARK = new List<string>
        {
            "SPARK",
            "spark"
        };

        // Token: 0x04000011 RID: 17
        private List<string> CodecHEVC_or_x265 = new List<string>
        {
            "HEVC",
            "x265",
            "x256"
        };

        // Token: 0x04000012 RID: 18
        private List<string> Codecx264 = new List<string>
        {
            "x264"
        };

        // Token: 0x04000013 RID: 19
        private List<string> QualityBluRay = new List<string>
        {
            "bluray",
            "brrip",
            ".br.",
            "BRRip",
            "BluRay",
            "BrRip" ,"blu-ray"
        };

        // Token: 0x04000014 RID: 20
        private List<string> Qualitywebrip = new List<string>
        {
            ".web.",
            "webrip",
            "WEBRip"
        };

        // Token: 0x04000015 RID: 21
        private List<string> Qualitywebdl = new List<string>
        {
            "webdl",
            "web-dl"
        };

        // Token: 0x04000016 RID: 22
        private List<string> Qualityxvid = new List<string>
        {
            "xvid"
        };

        // Token: 0x04000017 RID: 23
        private List<string> QualityDVDRip = new List<string>
        {
            "dvdrip",
            "dvdscr" ,
            "dvd"
        };

        // Token: 0x04000018 RID: 24
        private List<string> QualityHDRip = new List<string>
        {
            "hdrip"
        };

        // Token: 0x04000019 RID: 25
        private List<string> QualityHDTV = new List<string>
        {
            "HDTV",
            "hdtv"
        };


    }
}
