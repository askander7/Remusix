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
    
    public partial class Song
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Song()
        {
            this.Posts = new HashSet<Post>();
            this.UserListenSongs = new HashSet<UserListenSong>();
        }
    
        public int SongID { get; set; }
        public string SongName { get; set; }
        public string SongLink { get; set; }
        public int ArtistID { get; set; }
    
        public virtual Artist Artist { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserListenSong> UserListenSongs { get; set; }
    }
}
