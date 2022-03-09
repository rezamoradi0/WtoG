using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;


namespace MediaInfo
{
    static class MediaInformation
    {
        public static string MediaInfoPath =@"G:\SubtitleBotPlugins\MediaInfo.Windows.x64";




        public static bool GetDurationBatch(List<MovieFile> Movies) {
            foreach (var item in Movies)
            {
                item.MovieFileLength = GetDuration(item);
            }
        return true;
        }
        public static int GetDuration(MovieFile _movieFile) {
            
            
            string info= CMD_GetInfo(_movieFile.MovieName, MovieFile.MovieFolder);
            string[] InfoArray = info.Split(Environment.NewLine.ToCharArray());
            int duration = 0;

           
            foreach (var item in InfoArray)
            {
                if (item.ToLower().Contains("duration")&& item.ToLower().Contains(" ms"))
                {

                    
                    int IndexOfDots=item.IndexOf(':');
                    string dur=item.Remove(0,IndexOfDots+1);
                    dur = dur.Replace(" ", ""); dur = dur.Replace("h", "h,");
                    dur = dur.Replace("min", "min,"); dur = dur.Replace("s", "s,");
                 string[]   HMinSec = dur.Split(',');
                    foreach (var _item in HMinSec)
                    {
                        Console.WriteLine(_item);
                        if (_item.Contains("h"))
                        {
                            int secends =3600*Convert.ToInt32(_item.Replace("h", ""));
                            duration += secends;
                        }
                        else if (_item.Contains("min"))
                        {
                            int secends =60* Convert.ToInt32(_item.Replace("min", ""));
                            duration += secends;
                        }
                        else if (_item.Contains("s") && !_item.Contains("ms"))
                        {
                            int secends =  Convert.ToInt32(_item.Replace("s", ""));
                            duration += secends;
                        }

                    }
                  
                    break;
                }
              
            }
            return duration;
        }

        // این متد رو متد کلاس سی ام دی بهینه کنید
        public static string CMD_GetInfo(string FileName,string FileFolder) {

            string MyCommand = $"mediainfo --full {FileFolder}{FileName}";

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WorkingDirectory = MediaInfoPath;
            cmd.Start();

            cmd.StandardInput.WriteLine(MyCommand);
            cmd.StandardInput.Flush();

            cmd.StandardInput.Close();
            var reader = cmd.StandardOutput;
            string MediaInfoOutPut = reader.ReadToEnd();

            return MediaInfoOutPut;
        }
    }
}
