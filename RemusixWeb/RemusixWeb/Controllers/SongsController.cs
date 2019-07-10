//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using RemusixWeb.Models;

//namespace RemusixWeb.Controllers
//{
//    public class SongsController : Controller
//    {
//        private RemusixDBEntities db = new RemusixDBEntities();

//        // GET: Songs
//        //public ActionResult Index()
//        //{
//        //    //var songs = db.Songs.Include(s => s.Album).Include(s => s.Artist).Include(s => s.Genre);
//        //    return View(songs.ToList());
//        //}

//        // GET: Songs/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Song song = db.Songs.Find(id);
//            if (song == null)
//            {
//                return HttpNotFound();
//            }
//            return View(song);
//        }

//        // GET: Songs/Create
//        public ActionResult Create()
//        {
//            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "AlbumName");
//            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName");
//            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName");
//            return View();
//        }

//        // POST: Songs/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "SongID,SongName,YearOfProduction,GenreID,Plays,Rating,SondDescription,AlbumID,ArtistID")] Song song)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Songs.Add(song);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "AlbumName", song.AlbumID);
//            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", song.ArtistID);
//            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName", song.GenreID);
//            return View(song);
//        }

//        // GET: Songs/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Song song = db.Songs.Find(id);
//            if (song == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "AlbumName", song.AlbumID);
//            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", song.ArtistID);
//            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName", song.GenreID);
//            return View(song);
//        }

//        // POST: Songs/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "SongID,SongName,YearOfProduction,GenreID,Plays,Rating,SondDescription,AlbumID,ArtistID")] Song song)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(song).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "AlbumName", song.AlbumID);
//            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", song.ArtistID);
//            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "GenreName", song.GenreID);
//            return View(song);
//        }

//        // GET: Songs/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Song song = db.Songs.Find(id);
//            if (song == null)
//            {
//                return HttpNotFound();
//            }
//            return View(song);
//        }

//        // POST: Songs/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Song song = db.Songs.Find(id);
//            db.Songs.Remove(song);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
