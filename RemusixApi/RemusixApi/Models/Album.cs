//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RemusixApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Album
    {
        public int AlbumID { get; set; }
        public string AlbumName { get; set; }
        public Nullable<System.DateTime> YearOfProduction { get; set; }
        public Nullable<int> Plays { get; set; }
        public Nullable<int> Rating { get; set; }
        public string AlbumDescription { get; set; }
        public int ArtistID { get; set; }
    
        public virtual Artist Artist { get; set; }
    }
}