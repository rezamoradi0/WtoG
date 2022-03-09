using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaInfo
{
    static class SubtitleEdit
    {
        static string SubtitleEditPath = @"C:\Program Files\Subtitle Edit";
        public static bool SubRipBatch(this List<MovieFile> listOFMovies) {
            string MoviesPath = "";
            foreach (var Movie in listOFMovies)
            {
                MoviesPath += MovieFile.MovieFolder + Movie.MovieName+",";
            }
            MoviesPath= MoviesPath.Substring(0, MoviesPath.Length-1);
          
            
            string Command = $"SubtitleEdit /convert {MoviesPath} Subrip";
           string Result= CMD.RunCMD(Command,SubtitleEditPath);
            if (Result.Contains("file(s) converted")) 
            {
                AddSubToMovieFile(Result,listOFMovies);
            }
            
            return true;
        }
        static void AddSubToMovieFile(string SubtitleEditRes, List<MovieFile> Movies) {
            SubtitleEditRes = SubtitleEditRes.Replace("\r\n", "*");
            SubtitleEditRes = SubtitleEditRes.Replace("done.", "");
            SubtitleEditRes = SubtitleEditRes.Remove(0, SubtitleEditRes.IndexOf("1: "));
            string[] Lines = SubtitleEditRes.Split('*');

            // List<SubtitleFile> SubtitlesList = new List<SubtitleFile>();
          

            foreach (var item in Lines)
            {
                Console.WriteLine("this is a item "+item);
               
                int indexOf1 = item.IndexOf(": ");
                string temp = "";
                if (indexOf1 >= 0) {
                    try
                    {
                        temp = item.Remove(0, indexOf1 - 2);
                    }
                    catch (Exception)
                    {
                        temp = item.Remove(0, indexOf1 - 1);
                    }
                    string FileIndex = temp.Remove(3);
                    FileIndex = FileIndex.Replace(":", "");
                    int MovieIndex = Convert.ToInt32(FileIndex);





                    if (item.Contains(".mkv"))
                    {


                        Movies[MovieIndex - 1].Subtitles.Add(RippedSubtiteName(item));
                    }
                }
               
              
              
            }

          
            

        }

       public static SubtitleFile RippedSubtiteName(string LineInfo) {

            SubtitleFile Subtitle=new SubtitleFile();
          
            if (LineInfo.Contains(".per."))
            {
                Subtitle.Languge = "per";

            }
            else if (LineInfo.Contains(".und."))
            {
                Subtitle.Languge = "und";
            }
           else if (LineInfo.Contains(".eng."))
            {
                Subtitle.Languge = "eng";
            }
            int IndexOFArrow = LineInfo.IndexOf(" -> ");
            string SubPath=LineInfo.Remove(0,IndexOFArrow+4);
            SubPath = SubPath.Replace("...", "");
            if (SubPath.Contains(".srt"))
            {
                Subtitle.Format = "srt";
            }else if (SubPath.Contains(".vob"))
            {
                Subtitle.Format = "vob";
            }else 
            {
                Subtitle.Format = "und";
            }
          
            Subtitle.SubtitlePath = SubPath;

           
            
            return Subtitle;
        }
       
    }
}
