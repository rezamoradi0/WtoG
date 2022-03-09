using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WtoG
{
    public static class Socket
    {
        static Thread a;
        static int Server_PORT_NO;
        //const string SERVER_IP = "5.56.134.143";
        static string SERVER_IP;
        static int Listener_PORT;
        static string Listener_IP;
        public static void TurnOnSocket(string ServerIp, string ServerPort)
        {
            Form1 f1 = new Form1();

            Listener_PORT = int.Parse(ServerPort);
            Listener_IP = ServerIp.Replace("\r\n", null).Replace("\r", null).Replace("\n", null);
            // GetUpdates();
            a = new Thread(new ThreadStart(GetUpdates));
            a.Start();
        }
        public static void GetUpdates()
        {


        LABEL1:
            TcpListener listener = new TcpListener(IPAddress.Parse(Listener_IP), Listener_PORT);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            // MessageBox.Show(Environment.NewLine + ("Received : " + dataReceived));

            //..........
            Form1 f1 = new Form1();
            f1.ReciviveLink(dataReceived, "");
            //..........

            client.Close();
            listener.Stop();
            goto LABEL1;
        }
        public static void SendToOtherServer(string TextToSend, string ToSendPort, string ToSendIP)
        {
            Form1 f1 = new Form1();
            Server_PORT_NO = int.Parse(ToSendPort.Replace("\r\n", null).Replace("\r", null).Replace("\n", null));
            SERVER_IP = ToSendIP.Replace("\r\n", null).Replace("\r", null).Replace("\n", null);
            TcpClient client = new TcpClient(SERVER_IP, Server_PORT_NO);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(TextToSend);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            client.Close();
        }

































        //const int Server_PORT_NO = 9000;
        ////const string SERVER_IP = "5.56.134.143";
        //const string SERVER_IP = "127.0.0.1";
        //void SendToOtherServer(string TextToSend)
        //{
        //    TcpClient client = new TcpClient(SERVER_IP, Server_PORT_NO);
        //    NetworkStream nwStream = client.GetStream();
        //    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(TextToSend);
        //    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        //    client.Close();
        //}

        //const int Listener_PORT = 9001;
        //const string Listener_IP = "127.0.0.1";
        //public void GetUpdates()
        //{
        //LABEL1:
        //    TcpListener listener = new TcpListener(IPAddress.Parse(Listener_IP), Listener_PORT);
        //    listener.Start();
        //    TcpClient client = listener.AcceptTcpClient();
        //    NetworkStream nwStream = client.GetStream();
        //    byte[] buffer = new byte[client.ReceiveBufferSize];
        //    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
        //    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        //    MessageBox.Show(Environment.NewLine + ("Received : " + dataReceived));

        //    //..........
        //    // ReciviveLink(dataReceived, "");
        //    //..........

        //    client.Close();
        //    listener.Stop();
        //    goto LABEL1;
        //}
















        //void Trainbit()
        //{
        //    webControl1.WebView.Url = "https://trainbit.com/files/";
        //    timerTrainbit = new System.Windows.Forms.Timer();
        //    timerTrainbit.Tick += timerTrainbit_Tick;
        //    timerTrainbit.Interval = 1000;
        //    //timerTrainbit.Enabled = true;
        //    //timerTrainbit.Start();
        //}

        //bool FirstTimer = true;
        //void timerTrainbit_Tick(object sender, EventArgs e)
        //{
        //    if (FirstTimer)
        //    {
        //        webControl1.WebView.EvalScript("var File = document.querySelectorAll('[class=\"file\"]')[0].querySelector('a');");
        //        FirstTimer = false;
        //        TimerLastName = webControl1.WebView.EvalScript("File.innerText").ToString();
        //        TimerCheckName = TimerLastName;
        //        Debug.WriteLine("faaaaaaaaaaaaaaaaaaaaalse");

        //    }
        //    else
        //    {
        //        webControl1.WebView.EvalScript(" File = document.querySelectorAll('[class=\"file\"]')[0].querySelector('a');");
        //        //webControl1.WebView.EvalScript(" File = document.querySelector('[id=\"files-list\"]').querySelectorAll(\"li\")[3].querySelector('a');");

        //    }


        //    TimerCheckName = webControl1.WebView.EvalScript("File.innerText").ToString();
        //    Debug.WriteLine("TimerLastName : " + TimerLastName + "  TimerCheckName : " + TimerCheckName);
        //    if (TimerLastName != TimerCheckName)
        //    {
        //        TimerLastName = TimerCheckName;
        //        var link = webControl1.WebView.EvalScript("File.href").ToString();
        //        RedirectToDownload(link, TimerCheckName);
        //        webControl1.WebView.EvalScript("document.querySelector('[onclick=\"core.plugins.dialog.close('remoteupload-dialog');\"]').click();");
        //        remoteupload_dialog_Show = false;
        //    }


        //}
        //System.Windows.Forms.Timer timerTrainbit;
        //string TimerCheckName;
        //string TimerLastName;
        //bool remoteupload_dialog_Show;
        //void OpenTrainbitRemoteUpload(string DownloadLink)
        //{
        //    if (remoteupload_dialog_Show == false)
        //    {
        //        webControl1.WebView.EvalScript("document.querySelector('li[id=\"remoteupload_action\"]').click();");
        //        remoteupload_dialog_Show = true;
        //    }
        //    webControl1.WebView.EvalScript("document.querySelector('[id=\"t_uploadlinks\"]').innerText='" + DownloadLink + "';");
        //    webControl1.WebView.EvalScript("document.querySelector('[id=\"b_uploadurl\"]').click();");
        //    //Uri uri = new Uri(DownloadLink);
        //    //var FileName = System.IO.Path.GetFileName(uri.AbsolutePath);
        //    //TimerCheckName = FileName;
        //    timerTrainbit.Enabled = true;
        //    timerTrainbit.Start();
        //}











    }

}
