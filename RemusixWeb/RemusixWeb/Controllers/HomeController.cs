using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RemusixWeb.Models;

    namespace RemusixWeb.Controllers
{
    public class HomeController : Controller
    {
        private RemusixDBEntities db = new RemusixDBEntities();

        //Home 'T' Home action calls SuggestFriends method
        public ActionResult Home()
        {
            //for user and suggest friends
            if (Session["user"] == null) { return RedirectToAction("Login","Users"); }
            string email = Session["user"].ToString();
            User user=db.Users.FirstOrDefault(u => u.Email.Equals(email));
            //check user's status
            if (user.Status.Equals("deactive")) { return RedirectToAction("DeActive", "Home"); }
            //_______________get posts of nearby users_________________//
            if (user.Longitude != null && user.Latitude != null)
            {
                IEnumerable<int> uids = db.Users.Where(u =>u.Longitude!=null&& Math.Abs((double)u.Longitude - (double)user.Longitude) <= 5 &&
                 u.Latitude!=null&&  Math.Abs((double)u.Latitude - (double)user.Latitude) <= 5 && u.UserID != user.UserID).Select(u => u.UserID);
                List<Post> posts = new List<Post>();
                foreach (int uid in uids)
                {
                    posts.AddRange(db.Posts.Where(u => u.UserID == uid));
                }
                ViewBag.posts = posts;
            }
            return View(SuggestFriends(email));
        }

        // contact us 'T'
        public ActionResult ContactUs()
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Users"); }
            string email = Session["user"].ToString();
            string username = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserName;
            ViewBag.messages=db.Messages.Where(m => m.Receiver.Equals(username)).OrderByDescending(m=>m.MessageTime);
            return View(db.Users.FirstOrDefault(u => u.Email.Equals(email)));
        }

        // method to send message from user to admin
        public void SendMessage(string Sender,string Content)
        {
            db.Messages.Add(new Message() { Sender = Sender, Receiver = "admin", MessageContent = Content, MessageTime = DateTime.Now });
            db.SaveChanges();
        }

        /*ConnectWith 'W'*/
        public ActionResult ConnectWith()
        {
            return View();
        }


        //Index 'T'
        public ActionResult Index()
        {
            return View();
        }

        //Elements 'T'
        public ActionResult Elements()
        {
            return View();
        }

        //About 'T'
        public ActionResult About()
        {
            return View();
        }

        //Setting 'T'
        public ActionResult Setting()
        {
            return View();
        }

       
        /*BlockList 'W'*/
        public ActionResult BlockList()
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Users"); }
            return View();
        }

        /*DeActive 'W'*/
        public ActionResult DeActive()
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Users"); }
            string email = Session["user"].ToString();
            return View(db.Users.FirstOrDefault(u => u.Email.Equals(email)));
        }

        // method to active or deactive user account
        [HttpPost]
        public ActionResult UpdateStatus(string empty)
        {
            string email = Session["user"].ToString();
            User user = db.Users.FirstOrDefault(u => u.Email.Equals(email));
            user.ConfirmPassword = user.UserPassword;
            if (user.Status.Equals("active"))
            {
                user.Status = "deactive";
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LogOut", "Users");
            }
            else
            {
                user.Status = "active";
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home", "Home");
            }
        }


        /*Location 'W'*/
        public ActionResult Location()
        {
            return View();
        }

        //return list of friends suggestions
        public List<User> SuggestFriends(string email)
        {
            
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            ViewBag.uid = uid;
            IEnumerable<int> fids = db.Friends.Where(f => f.UserID == uid).Select(f => f.FriendID);
            List<User> sugFriends = new List<User>();
            List<User> users = db.Users.ToList(); int fl = 1;
            foreach (User user in users)
            {
                fl = 1;
                foreach (int fid in fids)
                {
                    if (user.UserID == fid) { fl = 0; break; }
                }
                if (fl == 1) { sugFriends.Add(user); }
                if (sugFriends.Count == 5) { break; }
            }
            return sugFriends;
        }


        // make user like some post
        public ActionResult LikeDislike(int id)
        {
            string email = Session["user"].ToString();
            int UserID = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;

            if (db.UserLikePosts.Any(p => p.PostID == id && p.UserID == UserID))
            {
                db.UserLikePosts.Remove(db.UserLikePosts.FirstOrDefault(p => p.PostID == id && p.UserID == UserID) as UserLikePost);
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            else
            {
                db.UserLikePosts.Add(new UserLikePost() { PostID = id, UserID = UserID,LikeTime=DateTime.Now });
                db.SaveChanges();
                return RedirectToAction("Home");
            }

        }


        //page to show one post
        public ActionResult PostPage(int id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            string email = Session["user"].ToString();
            ViewBag.users = SuggestFriends(email);
            return View(db.Posts.FirstOrDefault(p=>p.PostID==id));
        }
        /*search part*/
        #region search
        public JsonResult Search(string txt)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<User> userlist = db.Users.Where(u => u.UserName.StartsWith(txt)).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }
        #endregion search
    }
}