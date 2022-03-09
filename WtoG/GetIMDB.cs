using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Newtonsoft;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Diagnostics;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Specialized;
namespace MediaInfo
{
    public static class GetIMDB
    {
       public static string GetIMDBid(this MovieFile file)
        {

            var id = "";
          


            string keywordString = file.GetFileName() + " IMDB";

            string uriString = "http://www.google.com/search";
            WebClient webClient = new WebClient();

            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("q", keywordString);

            webClient.QueryString.Add(nameValueCollection);
            string output1 = webClient.DownloadString(uriString);


            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(output1);

            List<string> Links = new List<string>();
            foreach (var PartOfLink in document.DocumentNode.SelectNodes("//a"))
            {
                var linka = PartOfLink.Attributes["href"].Value;
                if (linka.Contains("www.imdb"))
                {
                    if (linka.StartsWith("/url?q"))
                    {
                        var ss = linka.Split(new[] { "/&" }, StringSplitOptions.None).FirstOrDefault().Replace("/url?q=", null);
                        if (ss.ToLower().StartsWith("https://www.imdb") && !ss.ToLower().Trim().EndsWith("/"))
                        {

                            id = ss;
                            break;

                        }


                    }


                }
            }



            id = id.Replace("https://www.imdb.com/title/", "");

            return id;




        }

    }
}
