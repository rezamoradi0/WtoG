using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

    
namespace MediaInfo
{
    public static class MkvMerage
    {
        static string MkvMeragePath = "C:/SubtitleBotPlugins\\mkvtoolnix\\mkvmerge.exe";
        static string CoverFullPath = "C:\\SubtitleBotPlugins\\cover.jpg";
        static string MkvMerageFolder = @"C:\SubtitleBotPlugins\mkvtoolnix";
        static string MKVCommandTxt = @"C:\SubtitleBotPlugins\MKVCMD.txt";

        public static bool SoftSobMovieBatch(this List<MovieFile> Movies)
        {
            foreach (var movie in Movies)
            {
                movie.SoftSobMovie();
            }
            return true;
        }


    public static bool SoftSobMovie(this MovieFile movie)
        {
            //  string OutPut_Path= movie.CreateOutPutPath()+ movie.GetBestSubtitle().GetNameOfSubtitle().Replace(".srt",".mkv");
            string OutPut_Path = movie.CreateOutPutPath();
              string cmd = CreateCommandLine(OutPut_Path, movie.CreateInPutPath(), movie.GetBestSubtitle().SubtitlePath);
           string Result= CMD.RunCMD(cmd, MkvMerageFolder);
            if (Result.Contains("Progress: 100%")&&Result.Contains("Muxing took"))
            {
                return true;
            }
            return false;
        }
        // مهم نیست سی ام دی در پوشه ام کی وی مرج باز بشود یا غیر از این باشد
        // برای احتیاط مسیر باز شدن سی ام دی رو پوشه ام کی وی مرج میدیم
        // زیرنویس نباید اعداد منفی داشته باشه ام کی وی مرج توانایی نداره انجام بده 
        /// <summary>
        /// با استفاده از این متد زیرنویس چسبیده فیلم را درست کنید توجه کنید اسم زیرنویس و اسم خروچی بجز پسوند باید یکسان باشد
        /// </summary>
        /// <param name="outPutPath">پوشه خروجی</param>
        /// <param name="InPutPath">فایل ورودی</param>
        /// <param name="SubtitleFile">فایل زیرنویس</param>
        /// <returns></returns>
        static string CreateCommandLine(string outPutPath,string InPutPath,string SubtitleFile) {
            // Command = @"MKVMERAGEPATH ^""--ui-language^"" ^""en^"" ^""--output^"" ^""OUTPUTPATHANDFILENAME^"" ^""--language^"" ^""0:und^"" ^""--default-track^"" ^""0:yes^"" ( ^""EDITEDSUBTITLEPATHANDNAME^"" ) ^""--no-subtitles^"" ^""--language^"" ^""1:und^"" ^""--default-track^"" ^""1:yes^"" ^""--language^"" ^""2:eng^"" ^""--default-track^"" ^""2:yes^"" ( ^""ORGINIALPATHANDFILENAME^"" ) ^""--attachment-name^"" ^""cover.jpg^"" ^""--attachment-mime-type^"" ^""image/jpeg^"" ^""--attach-file^"" ^""COVERPATHANDNAME^"" ^""--title^"" ^""DeltaMovieS^"" ^""--track-order^"" ^""0:0,1:1,1:2^""";
            //  string Command = @"""MKVMERAGEPATH"" --ui-language en --output ^""OUTPUTPATHANDFILENAME^"" --language 0:und --track-name 0:DeltaMovieS ^""^(^"" ^""EDITEDSUBTITLEPATHANDNAME^"" ^""^)^"" --no-subtitles --language 0:und --track-name 0:DeltaMovieS --default-track 0:yes --language 1:und --track-name 1:DeltaMovieS --default-track 1:yes ^""^(^"" ^""ORGINIALPATHANDFILENAME^"" ^""^)^"" --attachment-name cover.jpg --attachment-mime-type image/jpeg --attach-file ^""COVERPATHANDNAME^"" --title DeltaMovieS --track-order 0:0,1:0,1:1";
          

            string Command= File.ReadAllText(MKVCommandTxt);
           
               Command = Command.Replace("MKVMERAGEPATH", MkvMeragePath);
            Command = Command.Replace("OUTPUTPATHANDFILENAME", outPutPath);
            Command = Command.Replace("EDITEDSUBTITLEPATHANDNAME", SubtitleFile);
            Command = Command.Replace("ORGINIALPATHANDFILENAME", InPutPath);
            Command = Command.Replace("COVERPATHANDNAME", CoverFullPath);

           // Console.WriteLine(Command);
           // Console.ReadLine();
            return Command;
        }
    }
}
