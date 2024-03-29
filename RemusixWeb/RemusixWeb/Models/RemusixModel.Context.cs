﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RemusixWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RemusixDBEntities : DbContext
    {
        public RemusixDBEntities()
            : base("name=RemusixDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<UserCommentPost> UserCommentPosts { get; set; }
        public virtual DbSet<UserFollowArtist> UserFollowArtists { get; set; }
        public virtual DbSet<UserLikePost> UserLikePosts { get; set; }
        public virtual DbSet<UserListenSong> UserListenSongs { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
