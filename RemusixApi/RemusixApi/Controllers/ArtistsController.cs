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
    public class ArtistsController : ApiController
    {
        private RemusixDBEntities db = new RemusixDBEntities();

                // create artist
        [HttpPost]
        public IHttpActionResult AddArtist(Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return Ok(db.Artists.Count());
            }
            return BadRequest("invalid artist");
        }


        //______________________more______________________//
        ////return artist by id
        //[HttpGet]
        //public CustomArtist GetArtist(int s_artistid)
        //{
        //    var q = from a in db.Artists
        //            where a.ArtistID==s_artistid
        //             select new
        //             {
        //                 a.ArtistID,
        //                 a.ArtistName,
        //                 a.ArtistPhoto,
        //             };
        //    CustomArtist CustomArtist = null;
        //    foreach (var item in q)
        //    {
        //        CustomArtist = new CustomArtist()
        //        {
        //            ArtistID = item.ArtistID,
        //            ArtistName = item.ArtistName,
        //            ArtistPhoto = item.ArtistPhoto,
        //        };
        //    }

        //    return CustomArtist;
        //}




        //// update artist
        //[HttpPost]
        //public IHttpActionResult ChangeArtist(Artist artist)
        //{
        //    if (db.Artists.Find(artist.ArtistID) == null) return BadRequest("no artist");

        //    db.Entry(artist).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return Ok("modified!");
        //}

    }
}
