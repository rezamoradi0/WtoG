using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MediaInfo
{
    public static class CMD
    {
        public static string RunCMD(string Command, string RunPath)
        {


            // Console.WriteLine(Command); Console.WriteLine(RunPath);
            //  Console.ReadLine(); 
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WorkingDirectory = RunPath;
            cmd.Start();

            cmd.StandardInput.WriteLine(Command);
            cmd.StandardInput.Flush();

            cmd.StandardInput.Close();
            var reader = cmd.StandardOutput;

            string CMDOutPut = reader.ReadToEnd();

            return CMDOutPut;
        }
        public static void RunCMDIDM(string Command, string RunPath)
        {


            // Console.WriteLine(Command); Console.WriteLine(RunPath);
            //  Console.ReadLine(); 
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WorkingDirectory = RunPath;
            cmd.Start();

            cmd.StandardInput.WriteLine(Command);
            cmd.StandardInput.Flush();

            cmd.StandardInput.Close();
            var reader = cmd.StandardOutput;

            string CMDOutPut = "";

          
        }
    }
}
