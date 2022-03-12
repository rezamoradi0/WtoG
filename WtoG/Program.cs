using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaInfo;
namespace WtoG
{
   public static class c
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static void Job()
        {
            DownloadManager.InProgress = true;

          //  Console.OutputEncoding = System.Text.Encoding.UTF8;
            DeltaSubtitle DS = new DeltaSubtitle();

            List<MovieFile> files = new List<MovieFile>();
            files = FielsWorks.GetMovies();
            SubtitleEdit.SubRipBatch(files);
         //   MessageBox.Show("Subrip Passed next DeltaSub");
            DS.SubtitleDeltaBatch(files);
         //   MessageBox.Show("DeltaSub Passed next MediaInformation");
            MediaInformation.GetDurationBatch(files);
         //  MessageBox.Show("MediaInformation Passed next SoftSobMovieBatch");
            files.SoftSobMovieBatch();
         //   MessageBox.Show("SoftSobMovieBatch Passed next MoveSubtitlesBatch");

            files.MoveSubtitlesBatch();
            files.CreateUploadBatch();
            files._InsertMovieBatch();
           // Console.WriteLine("Finished");

            //  files.InsertMovieBatch();

            FielsWorks.DeleteAllFiles();
            DownloadManager.InProgress = false;
        //    Console.ReadLine();

        }

    }
}
