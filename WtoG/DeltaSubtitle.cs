using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Chilkat;
using System.Threading;
using System.Windows.Forms;

namespace MediaInfo
{
    internal class DeltaSubtitle
    {
       
        public static int result =0;
        const string DeltaSubPath = @"C:\SubtitleBotPlugins\DeltaSub.srt";

        public  bool SubtitleDeltaBatch(List<MovieFile> Movies)
        {

            int movieCount = Movies.Count;
            foreach (var Movie in Movies)
            {
               
             Thread SubtitleThread = new Thread(() =>
                {
                   Subtitle(Movie.GetBestSubtitle());
                });
                SubtitleThread.Start();
                // string result= Subtitle(Movie.GetBestSubtitle());
            }
            while (result!= movieCount) {
                Thread.Sleep(2000);
            }
            result = 0;
            return true;
        }


        /// <summary>
        /// زیرنویس را به این متد بدید و زیرنویس به صورت خودکار تغییر میکنه 
        /// </summary>
        /// <param name="Subtitle">فایل زیرنویس</param>
        /// <returns> Finshed را به عنوان پایان کار بازمیگرداند </returns>
        public string Subtitle(SubtitleFile Subtitle)
        {

          //  System.Windows.Forms.MessageBox.Show(Subtitle.SubtitlePath);
            string[] subtitleLines = File.ReadAllLines(Subtitle.SubtitlePath);
            string[] EditedSubtitle = DeltaSub(subtitleLines);
            string Save_Path = Set_Path(Subtitle.SubtitlePath);
            Subtitle.SubtitlePath = Save_Path;


            string Result = writeSubAsync(EditedSubtitle, Save_Path);
            return Result;

        }
        public static string writeSubAsync(string[] Lines, string save_path)
        {

            File.WriteAllLines(save_path, Lines);
            UTF8TOBOM(save_path);
            result++;
            return "Finshed";

        }
        public static string writeSubAsync(string AllText, string save_path)
        {

           
            File.WriteAllText(save_path, AllText);
            UTF8TOBOM(save_path);
            return "Finshed";

        }
        public string[] DeltaSub(string[] MovieSub) {
            string[] deltaMoviesSub = File.ReadAllLines(DeltaSubPath);

            string[] ForEditSub = MovieSub;
            ForEditSub = ReplaceSub(ForEditSub);
            string[] newSub = new string[deltaMoviesSub.Length + ForEditSub.Length + 5];
            int a = 0;
            int Counter = 0;
           
            foreach (var item in deltaMoviesSub)
            {
              
                string _Line = LineCounter(ref Counter, item);
                newSub[a] = _Line;
                a++;
            }
            newSub[a] = Environment.NewLine;
            a++;
            foreach (var item in ForEditSub)
            {
                if (Counter > 260 && Counter < 800 && item.ToLower().Contains("deltamovies"))
                {

                }
                else {
                    string _Line = LineCounter(ref Counter, item);
                    newSub[a] = _Line;
                    a++;
                }
            
            }
            
            return newSub;
        }
        public string LineCounter(ref int Count, string Line) {
            try
            {
                Convert.ToInt32(Line);
                Count++;
                return Count.ToString();
            }
            catch (Exception)
            {
                return Line;
                
            }

        }
        public string Set_Path(string filePath) {

            // این ساب نیم هست دیگه تغییر نمیتونم بدم از کد های قبلی هستش

            string SubtitleName = filePath.Replace(".srt", ".DeltaMovieS.srt");

            SubtitleName = SubtitleName.Replace("ZarFilm", "DeltaMovieS");
            SubtitleName = SubtitleName.Replace("Zarfilm", "DeltaMovieS");
                   SubtitleName = SubtitleName.Replace("zarFilm", "DeltaMovieS");
            SubtitleName = SubtitleName.Replace("zarfilm", "DeltaMovieS");

            SubtitleName = SubtitleName.Replace("[OkMovie]", "-DeltaMovieS");
            SubtitleName = SubtitleName.Replace("[OkMovie.xyz]", "-DeltaMovieS");
            SubtitleName = SubtitleName.Replace("OkMovie.xyz", "DeltaMovieS");
            SubtitleName = SubtitleName.Replace(".OkMovie", ".DeltaMovieS");
            SubtitleName = SubtitleName.Replace("OkMovie", ".DeltaMovieS");
            SubtitleName = SubtitleName.Replace("okMovie", ".DeltaMovieS");
            SubtitleName = SubtitleName.Replace("Okmovie", ".DeltaMovieS");
            SubtitleName = SubtitleName.Replace("okmovie", ".DeltaMovieS");
            SubtitleName = SubtitleName.Replace("[@archive_series]", "-DeltaMovieS");
            SubtitleName = SubtitleName.Replace("@Archive_series]", "-DeltaMovieS");
            SubtitleName = SubtitleName.Replace("@archive_series", "DeltaMovieS");
            SubtitleName = SubtitleName.Replace("@Archive_Series", "DeltaMovieS");
            SubtitleName = SubtitleName.Replace("@archive_Series", "DeltaMovieS");
            SubtitleName = SubtitleName.Replace(".archive_series", ".DeltaMovieS");
            SubtitleName = SubtitleName.Replace("archive_series", ".DeltaMovieS");

            // Mobo
            SubtitleName = SubtitleName.Replace("(1)", "");
            SubtitleName = SubtitleName.Replace("MetaL", "Delta");
            SubtitleName = SubtitleName.Replace(".per", "");
            SubtitleName = SubtitleName.Replace(".und", "");
            SubtitleName = SubtitleName.Replace("mobomovie", "");
            SubtitleName = SubtitleName.Replace(" ", ".");
            SubtitleName = SubtitleName.Replace("-MovieApp.OnGooglePlay", "");
            SubtitleName = SubtitleName.Replace("-MovieApp.OnGooglPlay", "");
            SubtitleName = SubtitleName.Replace(".-", ".");
            SubtitleName = SubtitleName.Replace(".MovieApp", "");
            SubtitleName = SubtitleName.Replace(".Movieapp", "");
            SubtitleName = SubtitleName.Replace("_.", ".");
            SubtitleName = SubtitleName.Replace("%20", "");
            SubtitleName = SubtitleName.Replace("_", ".");

            //DibaMovie
            SubtitleName = SubtitleName.Replace("(DibaMovie)", "");
            SubtitleName = SubtitleName.Replace("DibaMovie", "");
            SubtitleName = SubtitleName.Replace("dibamovie", "");
            SubtitleName = SubtitleName.Replace("Diba", "");
            SubtitleName = SubtitleName.Replace("diba", "");


            //topmoviez
            SubtitleName = Regex.Replace(SubtitleName, "TopMoviez", "", RegexOptions.IgnoreCase);
            SubtitleName = Regex.Replace(SubtitleName, "TopMoviez", "", RegexOptions.IgnoreCase);
            SubtitleName = Regex.Replace(SubtitleName, "TopMovies", "", RegexOptions.IgnoreCase);
            SubtitleName = Regex.Replace(SubtitleName, "TopMovie", "", RegexOptions.IgnoreCase);





            //seasons

            SubtitleName = SubtitleName.Replace("s01e", "S01E").Replace("s02e", "S02E").Replace("s03e", "S03E").Replace("s04e", "S04E").Replace("s05e", "S05E").Replace("s06e", "S06E").Replace("s07e", "S07E").Replace("s08e", "S08E").Replace("s09e", "S09E");
            SubtitleName = SubtitleName.Replace("s10e", "S10E").Replace("s11e", "S11E").Replace("s12e", "S12E").Replace("s13e", "S13E").Replace("s14e", "S14E").Replace("s15e", "S15E").Replace("s16e", "S16E").Replace("s17e", "S17E").Replace("s18e", "S18E").Replace("s19e", "S19E");
            SubtitleName = SubtitleName.Replace("s20e", "S20E").Replace("s21e", "S21E").Replace("s22e", "S22E").Replace("s23e", "S23E").Replace("s24e", "S24E").Replace("s25e", "S25E").Replace("s26e", "S26E").Replace("s27e", "S27E").Replace("s28e", "S28E").Replace("s29e", "S29E");


           

         SubtitleName = SubtitleName.Replace("psa", "PSA");
            SubtitleName = SubtitleName.Replace("rmt", "RMT");
            SubtitleName = SubtitleName.Replace("rmteam", "RMT"); SubtitleName = SubtitleName.Replace("SoftSub", "Sub");
          
            SubtitleName = Regex.Replace(SubtitleName, "DigiMoviez", "DeltaMovieS", RegexOptions.IgnoreCase);
            if (!SubtitleName.ToLower().Contains("Navar"))
            {
                SubtitleName = SubtitleName.Replace(".mkv", "-MetaLMovieS.mkv");

            }
            SubtitleName = SubtitleName.Replace(".srt.", ".srt");
            return SubtitleName;
        }
        /// <summary>
        /// این متد برای تبدیل فرمت UTF-8 به BOM هست
        /// </summary>
        /// <param name="SavedPath"> محل زیرنویس برای تبدیل</param>
        public  static void UTF8TOBOM(string SavedPath) {
            
            Charset charset = new Charset();
            charset.FromCharset = "UTF-8";
            charset.ToCharset = "BOM-UTF-8";
            bool a = charset.ConvertFile(SavedPath, SavedPath);
            Console.WriteLine(a+" File Converted To UTF8-DOM");
        }

        public static string[] ReplaceSub(string[] SubLines) {
            string[] newSub = new string[SubLines.Length];
            string FullSubString = "";

            foreach (var item in SubLines)
            {
                FullSubString += item + "~";
            }
         
                newSub = WordToReplace(FullSubString);
            
            return newSub;
        }
        static string[] WordToReplace(string Line) {
            string output1 = Line;

            output1 = output1.Replace("www.ZarFilm.CoM", "DeltaMovieS");
            output1 = output1.Replace("@ZarFilm_com", "@DeltaMovieS");
            output1 = output1.Replace("زرفيلم", "دلتا موویز");
            output1 = output1.Replace("زر فيلم", "دلتا موویز");
            output1 = output1.Replace("ZarFilm", "DeltaMovieS");
            output1 = output1.Replace("زز فيلم", "دلتا موویز");
            output1 = output1.Replace("www.ZarFilm.com", "DeltaMovieS");
            output1 = output1.Replace("@zarfilm_com", "@DeltaMovieS");
            output1 = output1.Replace("زر فيلم", "دلتا موویز");
            output1 = output1.Replace("@Zarfilm_com", "@DeltaMovieS");
            output1 = output1.Replace("WwW.ZarFilm.CoM", "DeltaMovieS");
            output1 = output1.Replace("WwW.ZarFilm.CoM", "DeltaMovieS");
            output1 = output1.Replace("zarFilm", "DeltaMovieS");
            output1 = output1.Replace("zar film", "DeltaMovieS");
            output1 = output1.Replace("Zar Film", "DeltaMovieS");
            output1 = output1.Replace("Zar film", "DeltaMovieS");
            output1 = output1.Replace("zar Film", "DeltaMovieS");
            output1 = output1.Replace("Zar-Film", "DeltaMovieS");
            output1 = output1.Replace("Zar-film", "DeltaMovieS");
            output1 = output1.Replace("zar-film", "DeltaMovieS");
            output1 = output1.Replace("zar-Film", "DeltaMovieS");
            output1 = output1.Replace("کافیست 1 بار از سایت ما دانلود کنید مطمعن باشید کاربر همیشگی ما میشوید", "با ما هم آهنگ فیلم ببینید ! دلتا موویز");
            output1 = output1.Replace("::. ارائه شده توسط وبسايت موبوفيلم .::", "دانلود شده از وبسایت دلتا موویز");
            output1 = output1.Replace("WwW.MoboMovie.net", "MetalMovieS");
            output1 = output1.Replace("مرجع دانلود فيلم و سريال با لينک مستقيم", "دانلود فیلم و سریال با زیرنویس چسبیده از دلتا موویز");
            output1 = output1.Replace("موبوفيلم", "دلتا موویز");
            output1 = output1.Replace("موبو فيلم", "دلتا موویز");
            output1 = output1.Replace("MoboMovie.net", "DeltaMovieS");
            output1 = output1.Replace("MoboMovie.pw", "DeltaMovieS");
            output1 = output1.Replace("MoboMovie.xyz", "DeltaMovieS");
            output1 = output1.Replace("MoboMovie.com", "DeltaMovieS");
            output1 = output1.Replace("MOBOMOVIE.NET", "DeltaMovieS");
            output1 = output1.Replace("MoboMovie.info", "DeltaMovieS");
            output1 = output1.Replace("MOBOMOVIES", "DeltaMovieS");
            output1 = output1.Replace("MOBOMOVIE", "DeltaMovieS");
            output1 = output1.Replace("MOBOFILMS", "DeltaMovieS");
            output1 = output1.Replace("MOBOMOVIES", "DeltaMovieS");
            output1 = output1.Replace("Mobomovie", "DeltaMovieS");
            output1 = output1.Replace("mobomovie", "DeltaMovieS");
            output1 = output1.Replace(" اولين سايت زيرنويس چسبيده در ايران ايده اي نو را تجربه کنيد", "دانلود رایگان فیلم و سریال با زیرنویس فارسی چسبیده از اپیکیشن دلتاموویز - @DeltaMovieS");
            output1 = output1.Replace("اولين سايت زيرنويس چسبيده در ايران ايده اي نو را تجربه کنيد", "دانلود رایگان فیلم و سریال با زیرنویس فارسی چسبیده از اپیکیشن دلتاموویز - @DeltaMovieS");
            output1 = output1.Replace("اولین سایت زیرنویس چسبیده در ایران ایده ای نو را تجربه کنید", "دانلود رایگان فیلم و سریال با زیرنویس فارسی چسبیده از اپیکیشن دلتاموویز - @DeltaMovieS");
            output1 = output1.Replace("موبو فیلم", "دلتا موویز");
            output1 = output1.Replace("موبو", "دلتا");





            output1 = output1.Replace("Download: MovieApp", "DeltaMovieS");
            output1 = output1.Replace("Free On Google Playe", "Www.DeltaMovieS");
            output1 = output1.Replace("MovieApp.Apk", "دلتا موویز");
            output1 = output1.Replace("@#Movie_Apk ما", "WwW.DeltaMovieS");
            output1 = output1.Replace("@#Movie_Apk", "@DeltaMovieS / #DeltaMovieS");
            output1 = output1.Replace("MetaLMovieS", "DeltaMovieS");
            output1 = output1.Replace("@#MetaLMovieS", "@DeltaMovieS");
            output1 = output1.Replace("Delta", "Metal");
            output1 = output1.Replace("دیجی", "دلتا");
            output1 = output1.Replace("متال", "دلتا");
            output1 = output1.Replace("متالموویز", "ذلتا موویز");
            output1 = output1.Replace("MetaL-Movies", "DeltaMovieS");
            output1 = output1.Replace("MetaL-MovieS", "DeltaMovieS");
            output1 = output1.Replace("اپیکیشن", "وبسایت");

            output1 = output1.Replace("@#MovieApp", "WwW.DeltaMovieS");
            output1 = output1.Replace("@#Movie_App", "WwW.DeltaMovieS");
            output1 = output1.Replace("@MovieApp", "WwW.DeltaMovieS");
            output1 = output1.Replace("#MovieApp", "WwW.DeltaMovieS");

            output1 = output1.Replace("@#Movie_Apk ما", "WwW.DeltaMovieS");
            output1 = output1.Replace("@#Movie_Apk", "@DeltaMovieS / #DeltaMovieS");

            Regex.Replace(output1, "mobomovies", "DeltaMovieS", RegexOptions.IgnoreCase);
            Regex.Replace(output1, "mobomovie", "DeltaMovieS", RegexOptions.IgnoreCase);

            output1 = output1.Replace(".::WWW.MetalMovieS::.", "•• WwW.DeltaMovieS ••");

            output1 = output1.Replace(".::WWW.MetalMovieS::.", "DeltaMovieS |  دلتاموویز");

            output1 = output1.Replace("   .:: ارائه شده توسط وب سايت ديجي موويز ::.", "ارائه شده توسط دلتا موویز");
            output1 = output1.Replace("•• WwW.MetaLMovieS ••", "@DeltaMovieS | دلتاموویز");
            output1 = output1.Replace("MetaLMovieS", "DeltaMovieS | دلتاموویز");
            output1 = output1.Replace("متال موویز", "DeltaMovieS | دلتاموویز");
            output1 = output1.Replace("ديجي", "دلتا");
            output1 = output1.Replace("DIGIMOVIEZ", "DeltaMovieS");
            output1 = output1.Replace("DIGIMOVIES", "DeltaMovieS");
            output1 = Regex.Replace(output1, @"DigiMoviez.Com", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, @"DigiMoviez.Co", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "DigiMoviez", "DeltaMovieS", RegexOptions.IgnoreCase);


            //top movies

            output1 = Regex.Replace(output1, "WwW.TopMoviez.net", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "TopMoviez", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "TopMovies", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "TopMovie", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "topmovieznet", "دلتاموویز | DeltaMovieS", RegexOptions.IgnoreCase);

            output1 = Regex.Replace(output1, "تاپ مُویز", "DeltaMovieS | دلتا مُویز", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "تاپ مویز", "DeltaMovieS | دلتا مُویز", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "تاپ موویز", "DeltaMovieS | دلتا مُویز", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "تاپ مُویز", "DeltaMovieS | دلتا مُویز", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "تاپ مووی", "DeltaMovieS | دلتا مُویز", RegexOptions.IgnoreCase);
            output1 = Regex.Replace(output1, "تاپ مُوی", "DeltaMovieS | دلتا مُویز", RegexOptions.IgnoreCase);
          

            try
            {
                output1 = Regex.Replace(output1, @"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+[\w\-\.,@?^=%&amp;:\/~‌ \+#]*[\w\-\@?^=%&amp‌ ;\/~\+#]", "Www.MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", "Www.MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:t.me.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram.me.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:instagram |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:insta |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram : |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram :|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram: |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram:|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram :|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:telegram|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:t.me |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:t.me/ |[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);
                output1 = Regex.Replace(output1, @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:@|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", " MetalMovies", RegexOptions.IgnoreCase);


            }
            catch (Exception)
            {

              
            }
            output1 = output1.Replace("MetalMovieS", "DeltaMovieS");
            output1 = output1.Replace("MetalMovies", "DeltaMovieS");
            
            string[] outPutArray = output1.Split('~');
            return outPutArray;

        }
    }
}
