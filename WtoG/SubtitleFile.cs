using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaInfo
{
    public class SubtitleFile

    {
        public string SubtitlePath { get; set; }
        public string Languge { get; set; }
        public string Format { get; set; }

        public string GetNameOfSubtitle() {
            string Name = SubtitlePath.Remove(0, SubtitlePath.LastIndexOf("\\")+1);
        return Name;
        }
    
    }
}
