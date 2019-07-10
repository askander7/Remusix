using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RemusixApi.Models;
namespace RemusixApi.Controllers
{
    public class SongsController : ApiController
    {
        private RemusixDBEntities db = new RemusixDBEntities();

        // create song
        [HttpPost]
        public IHttpActionResult AddSong(Song song)
        {
            if (ModelState.IsValid)
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return Ok(db.Songs.FirstOrDefault(s=>s.SongName.Equals(song.SongName)&&s.ArtistID==song.ArtistID).SongID);
            }
            return BadRequest("invalid song");
        }


        //________________________more____________________//
        //// update song
        //[HttpPost]
        //public IHttpActionResult ChangeSong(Song song)
        //{
        //    if (db.Songs.Find(song.SongID) == null) return BadRequest("no song");

        //    db.Entry(song).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return Ok("modified!");
        //}


        ////return most played songs
        //[HttpGet]
        //public List<CustomSong> MostPlayedSong()
        //{
        //    var q = (from s in db.Songs
        //            join ar in db.Artists on s.ArtistID equals ar.ArtistID
        //            select new
        //            {
        //                s.SongID,
        //                s.SongName,
        //                ar.ArtistName
        //            }).Take(20);
        //    List<CustomSong> CustomSongs = new List<CustomSong>();
        //    foreach (var item in q)
        //    {
        //        CustomSongs.Add(new CustomSong()
        //        {
        //            SongID=item.SongID,
        //            SongName = item.SongName,
        //            ArtistName = item.ArtistName
        //        });
        //    }
          
        //    return CustomSongs;
        //}

        ////return most rated songs
        //[HttpGet]
        //public List<CustomSong> MostRatedSong()
        //{
        //    var q = (from s in db.Songs
        //             join ar in db.Artists on s.ArtistID equals ar.ArtistID
        //             select new
        //             {
        //                 s.SongID,
        //                 s.SongName,
        //                 ar.ArtistName
        //             }).Take(20);
        //    List<CustomSong> CustomSongs = new List<CustomSong>();
        //    foreach (var item in q)
        //    {
        //        CustomSongs.Add(new CustomSong()
        //        {
        //            SongID = item.SongID,
        //            SongName = item.SongName,
        //            ArtistName = item.ArtistName
        //        });
        //    }

        //    return CustomSongs;
        //}

        ////return song by id
        //[HttpGet]
        //public CustomSong GetSong(int s_songid)
        //{
        //    var q = from s in db.Songs
        //             join ar in db.Artists on s.ArtistID equals ar.ArtistID
        //             where s.SongID==s_songid
        //             select new
        //             {
        //                 s.SongID,
        //                 s.SongName,
        //                 ar.ArtistName
        //             };
        //    CustomSong CustomSong = null;
        //    foreach (var item in q)
        //    {
        //        CustomSong = new CustomSong()
        //        {
        //            SongID = item.SongID,
        //            SongName = item.SongName,
        //            ArtistName = item.ArtistName,
        //        };
        //    }

        //    return CustomSong;
        //}
    }
}
