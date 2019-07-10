using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemusixApi.Models
{
    public class CustomPost
    {
        public int PostID { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public string ArtistPhoto { get; set; }
        public string UserName { get; set; }
        public string Photo { get; set; }
        public Nullable<System.DateTime> PostTime { get; set; }
    }
}