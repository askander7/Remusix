using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemusixApi.Models
{
    public class CustomSong
    {
        public int SongID { get; set; }
        public string SongName { get; set; }
        public string SongLink { get; set; }
        public string ArtistName { get; set; }
    }
}