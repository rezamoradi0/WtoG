using EO.WebBrowser;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaInfo;

namespace WtoG
{
    public partial class Form1 : Form
    {

        HtmlAgilityPack.HtmlDocument document;

        public static bool  SelectedFile = false;
        int PostId = 0;
        string PublicPath;
        string DlFolderPath;
        List<string> Waitingline = new List<string>();
        System.Windows.Forms.Timer timer;
        List<string> FilesToDelete = new List<string>();
     
      public static Form1 AForm;
        public Form1()
        {
            InitializeComponent();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Trainbit();
            FormLoadGap();
           UploadManager.MyForm = this;
            
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());

        }
        public int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }
        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "\n\n" + GetLineNumber(e.Exception));
        }
        void FormLoadGap()
        {
          //  timer = new System.Windows.Forms.Timer();
         //   timer.Tick += timer_Tick;
        //    timer.Interval = 1000;
         //   timer.Enabled = true;
         //   timer.Start();
            webControl2.WebView.FileDialog +=WebView_FileDialog;
            webControl2.WebView.Url = "https://web.gap.im/#/im?p=%40deltamoviesbot";

        }
        void WebView_FileDialog(object sender, FileDialogEventArgs e)
        {
            e.Handled = true;
            e.Continue(PublicPath);

            SelectedFile = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ReciviveLink("http://dl.worldsubtitle.us/Serial/1398/Peaky.Blinders.S05E03_WorldSubtitle.zip", "");



            //ReciviveLink("http://dl.sourcebaran.com/download/80/sourcebaran.rar", "");



            //Thread a = new Thread(new ThreadStart(GetUpdates));
            //a.Start();

            //var htmlCode = "";
            //WebRequest req = HttpWebRequest.Create("https://www.myip.com");
            //WebProxy myproxy = new WebProxy("5.56.134.143",8795);
            //myproxy.Credentials = new NetworkCredential("DeltaGroup", "dodota5tanist");
            // myproxy.BypassProxyOnLocal = false;
            // req.Proxy = myproxy;
            // req.Method = "GET";


            //using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            //{
            //    htmlCode = reader.ReadToEnd();
            //}
            //Debug.WriteLine(htmlCode);

            //var sss =FardaDL.GetSearchPage("https://www1.farda-dl.net/?s=ssadsaedwq3er3");
            //if (sss == null)
            //{
            //    MessageBox.Show("Null");
            //}
            //else
            //{
            //    MessageBox.Show("not");
            //}

            //var src = webControl2.WebView.EvalScript("isprogresON");

            //ReciviveLink("http://s3.picofile.com/file/8371046850/Capture.PNG", "");
        }
        public void ReciviveLink(string LinkToDownload, string UserId)
        {
            //DownloadFile
            new Thread(delegate()
            {

                DownloadFile(LinkToDownload);
            }).Start();
        }
        void DownloadFile(string DownloadLink)
        {
            //string Path = "";
            //var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Path = desktop;
            //string filenamess = desktop + "\\gravatar.PNG";
            Uri uri = new Uri(DownloadLink);
            string fileName = System.IO.Path.GetFileName(uri.AbsolutePath);
            MessageBox.Show(DlFolderPath + "\\" + fileName);

            WebClient client = null;
            client = new WebClient();
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            //client.DownloadFileCompleted += client_DownloadFileCompleted;
            using (client)
            {

                client.DownloadFile(DownloadLink, DlFolderPath + "\\" + fileName);


            }



            Debug.WriteLine("DownloadFinished");

            //After end of Download , UploadFile
            UploadFile(DlFolderPath + "\\" + fileName, fileName);
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label8.Text = e.ProgressPercentage + "";
        }
     public  void UploadFile(string Path, string FileName)
        {
            PublicPath = Path;
           
           

            /*   webControl2.WebView.EvalScript("var target = document.querySelectorAll('[class=\"icon-attachment\"]')[1];var pos = target.getBoundingClientRect();");
               string a = @"var getAbsPosition = function(el){
       var el2 = el;
       var curtop = 0;
       var curleft = 0;
       if (document.getElementById || document.all) {
           do  {
               curleft += el.offsetLeft-el.scrollLeft;
               curtop += el.offsetTop-el.scrollTop;
               el = el.offsetParent;
               el2 = el2.parentNode;
               while (el2 != el) {
                   curleft -= el2.scrollLeft;
                   curtop -= el2.scrollTop;
                   el2 = el2.parentNode;
               }
           } while (el.offsetParent);

       } else if (document.layers) {
           curtop += el.y;
           curleft += el.x;
       }
       return [curtop, curleft];
   };";
               string b = "var b=getAbsPosition(target);";
              // webControl2.WebView.EvalScript(a);
               //webControl2.WebView.EvalScript(b);
              // MessageBox.Show("stop");
              // MessageBox.Show(webControl2.WebView.EvalScript("b").ToString());
              // var y = int.Parse(webControl2.WebView.EvalScript("b[0]").ToString().Split('.')[0]);
              // var x = int.Parse(webControl2.WebView.EvalScript("b[1]").ToString().Split('.')[0]);
               //var y = int.Parse(webControl1.WebView.EvalScript("pos.y").ToString().Split('.')[0]);
               //var x = int.Parse(webControl1.WebView.EvalScript("pos.x").ToString().Split('.')[0]);
              // webControl2.WebView.SendMouseEvent(MouseEventType.Click, new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
             */
            webControl2.WebView.SendMouseEvent(MouseEventType.Click, new MouseEventArgs(MouseButtons.Left, 1, 19, 173, 0));
            //while (!SelectedFile)
            //{

            ////}
            //System.Threading.Timer timer = new System.Threading.Timer(delegate (object stateInfo)
            //{
            //    var s = webControl2.WebView.EvalScript("var send = document.querySelector('[class=\"btn btn-primary\"]');send.click(); var sendpos = send.getBoundingClientRect();");
               
            //}, null, 0, 500);

            System.Threading.Timer timer1 = new System.Threading.Timer(delegate (object stateInfo)
            {
                var s = webControl2.WebView.EvalScript("var send = document.querySelector('[class=\"btn btn-primary\"]');send.click();");

            }, null, 0, 500);
            wait(500);
            //  Thread.Sleep(500);
            System.Threading.Timer timer2 = new System.Threading.Timer(delegate (object stateInfo)
            {
                var s = webControl2.WebView.EvalScript("send.click();");

            }, null, 0, 500);
            wait(500);
            //   Thread.Sleep(500);
            System.Threading.Timer timer3 = new System.Threading.Timer(delegate (object stateInfo)
            {
                var s = webControl2.WebView.EvalScript("var sendpos = send.getBoundingClientRect();");

            }, null, 0, 500);
            wait(500);
            //SelectedFile = false;
            // var s = webControl2.WebView.EvalScript("var send = document.querySelector('[class=\"btn btn-primary\"]');send.click(); var sendpos = send.getBoundingClientRect();");


            //var sendposy = int.Parse(webControl1.WebView.EvalScript("sendpos.y").ToString().Split('.')[0]);
            //var sendposx = int.Parse(webControl1.WebView.EvalScript("sendpos.x").ToString().Split('.')[0]);
            //webControl1.WebView.SendMouseEvent(MouseEventType.Click, new MouseEventArgs(MouseButtons.Left, 1, sendposx, sendposy, 0));
            //webControl1.WebView.SendKeyEvent(true, KeyCode.Space);


            //After UploadFile Check To Uploaded
            //  CheckToUpload(FileName);
            //Thread.Sleep(1500);
        }
        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        void CheckToUpload(string Name)
        {
            Waitingline.Add(Name);
            PostId++;
        }

        void timer_Tick(object sender, EventArgs e)
        {
          //  CheckingUpload();
        }
        void CheckingUpload()
        {
            /*
            int i = 0;

            if (FilesToDelete.Count > 0)
            {
                foreach (var del in FilesToDelete)
                {
                    Waitingline.Remove(del);
                }
                FilesToDelete = new List<string>();
                // MessageBox.Show("Del");
            }

            if (Waitingline.Count > 0)
            {

                foreach (var File in Waitingline)
                {
                    string htmlCode = "";
                    WebRequest req = HttpWebRequest.Create("https://gap.im/deltamoviesbot/" + (PostId - i));
                    req.Method = "GET";
                    using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                    {
                        htmlCode = reader.ReadToEnd();
                    }




                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(htmlCode);
                    var post = doc.DocumentNode.SelectNodes("//nav[@id='posts-container']");
                    if (post != null)
                        if (post.FirstOrDefault() != null)
                        {
                            FilesToDelete.Add(File);
                            ExtractLink(post.FirstOrDefault());

                        }

                    i++;
                }

            }
            */
        }
        void ExtractLink(HtmlNode node)
        {
            var FileCheck = node.SelectNodes(".//a[@target='_blank']");
            var File = new HtmlNode(HtmlNodeType.Text, new HtmlAgilityPack.HtmlDocument(), 1);
            if (FileCheck != null)
            {
                File = FileCheck.FirstOrDefault();
                var FileName = File.InnerText;
                var FileLink = File.Attributes["href"].Value;
                SendFileToBot(FileName, FileLink);
            }
            else
            {
                //File = node.SelectNodes(".//video").FirstOrDefault();
                //var FileLink = File.SelectNodes(".//source").FirstOrDefault().Attributes["src"].Value;
                //Uri uri = new Uri(FileLink);
                //var FileName = System.IO.Path.GetFileName(uri.AbsolutePath);
                //SendFileToBot(FileName, FileLink);
            }
        }

        void SendFileToBot(string Name, string Link)
        {

            // MessageBox.Show(Link);
            Debug.WriteLine(Link);
            Socket.SendToOtherServer(Link, textBox6.Text, textBox4.Text);
        }

        void RedirectToDownload(string DownloadLink, string Name)
        {
            MessageBox.Show(DownloadLink + "\n" + Name);

        }

        string MKVToolNixCMD = @"""MKVPath"" --ui-language en --output ^""MovieTargetPathAndName^"" --no-subtitles --no-attachments --no-global-tags --no-chapters --language 0:und --default-track 0:yes --language 1:eng --track-name ^""1:TrackNAME^"" ^""^(^"" ^""OriginFilePath^"" ^""^)^"" --attachment-name cover.png --attachment-mime-type image/jpeg --attach-file ^""CoverPath^"" --title MovieTitle --track-order 0:0,0:1";
        string MKVToolNixPath = null;
        public string EditeMovie(string MovieOriginPath, string MovieTargetPath, string TrackNAME, string MovieTitle, string CoverPath, string MovieTag)
        {
            string CMD = null;
            MKVToolNixPath = null;
            CMD = MKVToolNixCMD;
            CMD = CMD.Replace("TrackNAME", TrackNAME);
            CMD = CMD.Replace("CoverPath", CoverPath);
            CMD = CMD.Replace("MovieTitle", MovieTag);
            CMD = CMD.Replace("MKVPath", MKVToolNixPath);
            CMD = CMD.Replace("OriginFilePath", MovieOriginPath);
            CMD = CMD.Replace("MovieTargetPathAndName", MovieTargetPath);


            string Darsad = null;
            string TimeToCreate = null;
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(CMD);
            cmd.StandardInput.Flush();

            cmd.StandardInput.Close();
            var reader = cmd.StandardOutput;

            while (!reader.EndOfStream)
            {
                // the point is that the stream does not end until the process has 
                // finished all of its output.
                var nextLine = reader.ReadLine();
                if (nextLine.Contains("Progress:"))
                {
                    Darsad = nextLine;
                }
                else if (nextLine.Contains("Multiplexing too"))
                {
                    TimeToCreate = nextLine.Replace("Multiplexing too", ""); ;
                }

            }


            cmd.WaitForExit();
            return "Complite : " + Darsad + " In " + TimeToCreate;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Last Post Id Is Empety", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (int.TryParse(textBox1.Text, out PostId) == false)
            {
                MessageBox.Show("Last Post Id Is not Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DlFolderPath == null)
            {
                MessageBox.Show("Path Is Empety", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("IP or Port Is Empety", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Socket.TurnOnSocket(textBox3.Text, textBox5.Text);
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            label8.ForeColor = System.Drawing.Color.Green;
            label8.Text = "Working Now ...";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string folderPath = string.Empty;

            using (FolderBrowserDialog fdb = new FolderBrowserDialog())
            {
                if (fdb.ShowDialog() == DialogResult.OK)
                {
                    folderPath = fdb.SelectedPath;
                    DlFolderPath = fdb.SelectedPath;

                }
            }

            if (folderPath != string.Empty)
            {
                DlFolderPath = folderPath;
                textBox2.Text = folderPath;
            }
            else
            {
                //DlFolderPath = null;
                MessageBox.Show("Path Is Empety", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void webControl2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //UploadFile(@"G:\SubtitleBotPlugins\OutPutFolder\Arcane.S01E01.480p.WEB-DL.Sub.WebDL-.DeltaMovieS.mkv", "test.mkv");
            List<Link> links = new List<Link>();
            links = DownloadManager.GetLinksFromText();
            
            bool test = links.AddToQueu();


            MessageBox.Show("Finished All Files Remove DownloadLink Txt File And Replace With NewLinks ");



        }

        public bool Uploaded(string MoveName,ref string GapLink) {




            string AddressTxt = "http://deltagap.ir/DeltaMoviesBot/lastFile.txt";
            string TextFile;
            string[] DATA;
            string LINK="";
            string NAME = "";


            string SIZEmb;

            wait(500);
      
            
            while (NAME != MoveName)
            {
                 TextFile = Get(AddressTxt);
               DATA = TextFile.Split(',');
                 LINK = DATA[0];
                NAME = DATA[1];
                SIZEmb = DATA[2];
                //   MessageBox.Show(LINK);
              wait(5000);

            }
            
                GapLink = LINK;
            
                return true;
            


            return false;
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
