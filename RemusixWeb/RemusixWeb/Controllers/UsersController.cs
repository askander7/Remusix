using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Tokens;
using RemusixWeb.Models;

namespace RemusixWeb.Controllers
{
    [HandleError]
    public class UsersController : Controller
    {
        private RemusixDBEntities db = new RemusixDBEntities();

        //SignUp 'T'
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User user)
        {
            user.UserPassword = CreateJwrPackage(user.UserPassword);
            user.ConfirmPassword = user.UserPassword;
            if (ModelState.IsValid)
            {
                //check if user already exist by email
                if (db.Users.Any(u => u.Email.Equals(user.Email))) {ViewBag.checkEmail = "email already exsist";return View(user); }
                user.Status = "active";
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }


        // 'T' check existence of user by email
        public int CheckEmail(string Email, string UserPassword)
        {
            if (UserPassword != null) {UserPassword = CreateJwrPackage(UserPassword); }
            
            int state;
            if (Session["user"] != null)
            {
                string email = Session["user"].ToString();
                if (UserPassword == null) { state = db.Users.Any(u => u.Email.Equals(Email) && !u.Email.Equals(email)) ? 0 : 1; }
                else { state = db.Users.Any(u => u.Email.Equals(Email) && u.UserPassword.Equals(UserPassword)) ? 0 : 1; }
                return state;
            }
            if (UserPassword == null) { state = db.Users.Any(u => u.Email.Equals(Email)) ? 0 : 1; }
            else { state = db.Users.Any(u => u.Email.Equals(Email) && u.UserPassword.Equals(UserPassword)) ? 0 : 1; }
            return state;
        }

        // 'T' check existence of user by username
        public int CheckUserName(string UserName)
        {
            if (Session["user"] != null)
            {
                string email = Session["user"].ToString();
                string username = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserName;
                return db.Users.Any(u => u.UserName.Equals(UserName) && !u.UserName.Equals(username)) ? 0 : 1;
            }

            return db.Users.Any(u => u.UserName.Equals(UserName)) ? 0 : 1;
        }

        //check old password of student
        public int getoldpass(string passEmail, string OldPassword)
        {
            if (OldPassword != null) { OldPassword = CreateJwrPackage(OldPassword); }
            int state = db.Users.Any(u => u.Email.Equals(passEmail) && u.UserPassword.Equals(OldPassword)) ? 1 : 0;
            return state;
        }

        //Login 'T'
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string userpassword)
        {
            userpassword = CreateJwrPackage(userpassword);
            if (db.Users.Any(u => u.Email.Equals(email)))
            {
                if (db.Users.Any(u => u.Email.Equals(email)&&u.UserPassword.Equals(userpassword)))
                {
                    User user = db.Users.FirstOrDefault(u => u.Email.Equals(email) && u.UserPassword.Equals(userpassword));
                    Session["user"] = user.Email;
                    if (user.Status.Equals("deactive")) { return RedirectToAction("DeActive", "Home"); }
                    return RedirectToAction("Home", "Home");
                }ViewBag.passErr = "wrong password"; return View();                
            }ViewBag.emailErr = "not registered"; return View();
        }

        //logout 'T'
        [HttpGet]
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login");
        }

        //Profile 'T'
        public ActionResult UserProfile()
        {
            if (Session["user"] != null)
            {
                string email = Session["user"].ToString();
                int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
                //number of user's friends
                ViewBag.fNum = db.Friends.Where(u=>u.UserID==uid).Count();

                //user's posts
                ViewBag.posts = db.Posts.Where(u => u.UserID == uid);
                ViewBag.pnum= db.Posts.Where(u => u.UserID == uid).Count();

                //number of user's liked posts
                IEnumerable<int> pids = db.Posts.Where(u => u.UserID == uid).Select(p => p.PostID);
                int pcount = 0;
                foreach (int pid in pids)
                {
                   pcount+=db.UserLikePosts.Where(p => p.PostID == pid).Count();
                }
                ViewBag.likes = pcount;

                return View(db.Users.Find(uid));
            }
            return RedirectToAction("Login");
        }


        // display profile of some user
        public ActionResult Details(int? id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            if (uid == id) { return RedirectToAction("UserProfile"); }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            //number of user's friends
            ViewBag.fNum = db.Friends.Where(u => u.UserID == id).Count();

            //user's posts
            ViewBag.posts = db.Posts.Where(u => u.UserID == id);
            ViewBag.pnum = db.Posts.Where(u => u.UserID == id).Count();

            //number of user's liked posts
            IEnumerable<int> pids = db.Posts.Where(u => u.UserID == id).Select(p => p.PostID);
            int pcount = 0;
            foreach (int pid in pids)
            {
                pcount += db.UserLikePosts.Where(p => p.PostID == pid).Count();
            }
            ViewBag.likes = pcount;

            //check is friend or not
            ViewBag.isFriend = db.Friends.Any(u => u.UserID == uid && u.FriendID == id) ? true : false;
            return View(user);
        }

        //Information 'T'
        public ActionResult Information()
        {
            if (Session["user"] != null)
            {
                string email = Session["user"].ToString();
                int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
                return View(db.Users.Find(uid));
            }
            return RedirectToAction("Login");
        }

        //Add Friend 'T'
        [HttpPost]
        public ActionResult AddFriend(int fid,string a,string c)
        {
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            db.Friends.Add(new Friend { UserID = uid, FriendID = fid });
            db.SaveChanges();
            return RedirectToAction(a,c);
        }

        //Notification 'T' calls suggest friends method from home
        public ActionResult Notification()
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            ViewBag.uid = uid;

            //get notifications and number
            ViewBag.notifications = GetNotis();
            ViewBag.notinum = db.Notifications.Where(n => n.UserID != uid).Count();

            return View(new HomeController().SuggestFriends(email));
        }

        // get notifications custom
        public List<CustomNoti> GetNotis()
        {
            List<CustomNoti> customNotis = new List<CustomNoti>();
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            IEnumerable<Notification> notifications = db.Notifications.Where(f => f.UserID != uid);
            foreach (Notification notification in notifications)
            {
                customNotis.Add(new CustomNoti()
                {
                    UserID= notification.UserID,
                    nUserID=notification.nUserID,
                    UserName = db.Users.FirstOrDefault(u => u.UserID == notification.UserID).UserName,
                    nUserName = db.Users.FirstOrDefault(u => u.UserID == notification.nUserID).UserName,
                    Email = db.Users.FirstOrDefault(u => u.UserID == notification.UserID).Email,
                    Photo = db.Users.FirstOrDefault(u => u.UserID == notification.UserID).Photo,
                    PostID =notification.PostID,
                    Action = notification.Action,
                    ActionTime = notification.ActionTime,
                    Status = notification.Status

                });
            }
            return customNotis;
        }

        //Friends 'T'
        public ActionResult Friends()
        {
            if (Session["user"] != null)
            {
                string email = Session["user"].ToString();
                int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
                 //get user friends
                IEnumerable<int> fids = db.Friends.Where(u => u.UserID == uid).Select(u=>u.FriendID);
                List<User> users = new List<User>();
                foreach (int fid in fids)
                {
                    users.Add(db.Users.FirstOrDefault(u => u.UserID == fid));
                }
                ViewBag.Friends = users;
                ViewBag.fNum = fids.Count();
                //_______________________________________
                //user's posts
                ViewBag.pnum = db.Posts.Where(u => u.UserID == uid).Count();

                //number of user's liked posts
                IEnumerable<int> pids = db.Posts.Where(u => u.UserID == uid).Select(p => p.PostID);
                int pcount = 0;
                foreach (int pid in pids)
                {
                    pcount += db.UserLikePosts.Where(p => p.PostID == pid).Count();
                }
                ViewBag.likes = pcount;

                return View(db.Users.Find(uid));
            }
            return RedirectToAction("Login");
        }

        //Likes 'T'
        public ActionResult Likes()
        {
            if (Session["user"] != null)
            {
                string email = Session["user"].ToString();
                int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;

                ViewBag.fNum = db.Friends.Where(u => u.UserID == uid).Count();
                return View(db.Users.Find(uid));
            }
            return RedirectToAction("Login");
        }

        //Trend 'T' calls suggest friends method from home
        public ActionResult Trend()
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            ViewBag.uid = uid;
            GetTrends();
            return View(new HomeController().SuggestFriends(email));
        }

       // method to get trends from deezer
       public List<oArtist> GetTrends()
        {

            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.deezer.com/chart"));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            ViewBag.stream = WebResp.GetResponseStream();
            ViewBag.data = new StreamReader(ViewBag.stream, System.Text.Encoding.UTF8).ReadToEnd();
            ViewBag.data = JsonConvert.DeserializeObject<RootObject>(ViewBag.data);
            List<oArtist> artists = new List<oArtist>();
            foreach (var item in ViewBag.data.artists.data)
            {
                artists.Add(new oArtist()
                {
                    name = item.name,
                    picture_medium = item.picture_medium,
                    tracklist = item.tracklist
                });
            }
            ViewBag.artists = artists;
            return ViewBag.artists;
        }


        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserProfile","Users");
            }
            return View(user);
        }

        // edit user username
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserName(string UserName,string Email)
        {
            User user = db.Users.FirstOrDefault(u => u.Email.Equals(Email));
            user.ConfirmPassword = user.UserPassword;
            user.UserName = UserName;
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Information", "Users");
            }
            return View(user);
        }

        // edit user email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmail(string oldEmail, string Email)
        {
            User user = db.Users.FirstOrDefault(u => u.Email.Equals(oldEmail));
            user.ConfirmPassword = user.UserPassword;
            user.Email = Email;
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Information", "Users");
            }
            return View(user);
        }

        // edit user password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPass(string passEmail,string UserPassword)
        {
            User user = db.Users.FirstOrDefault(u => u.Email.Equals(passEmail));
            user.UserPassword =CreateJwrPackage(UserPassword);
            user.ConfirmPassword = user.UserPassword;
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Information", "Users");
            }
            return View(user);
        }

        //remove friend from friends table
        [HttpGet]
        public ActionResult RemoveFriend(int fid)
        {
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            db.Friends.Remove(db.Friends.FirstOrDefault(u => u.UserID == uid && u.FriendID == fid) as Friend);
            db.SaveChanges();
            return RedirectToAction("Friends", "Users");
        }

        // to creaye / update photo
        [HttpPost]
        public ActionResult CreateImg(User user)
        {
            string email = Session["user"].ToString();
            int uid = db.Users.FirstOrDefault(u => u.Email.Equals(email)).UserID;
            //Save in Object to save into DB
            string _fileName = "";//Path.GetFileNameWithoutExtension(product.FilePhoto.FileName);
            string _Extenstion = Path.GetExtension(user.FilePhoto.FileName);
            _fileName = uid + _Extenstion;
            // user.Photo = _fileName;
            using (SqlConnection con = new SqlConnection(@"data source=.\SQLEXPRESS;initial catalog=RemusixDB;user id=sa;password=MySqlAuth7777"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"Update Users set Photo='{_fileName}' where Email='{email}'", con);
                cmd.ExecuteNonQuery();
                con.Close(); cmd.Dispose();
            }
            //To Upload in Folder Upload 
            _fileName = Path.Combine(Server.MapPath("~/Upload/Images/") + _fileName);
            user.FilePhoto.SaveAs(_fileName);
         
            return RedirectToAction("UserProfile");

        }

        //to display photo
        public FileContentResult MyAction(string path)
        {
            if (path != null)
            {
                path = Path.Combine(Server.MapPath("~/Upload/Images/") + path);
                byte[] imgarray = System.IO.File.ReadAllBytes(path);
                return new FileContentResult(imgarray, "image/jpg");
            }return null;
        }

        // jwt for encryption
        string CreateJwrPackage(string Password)
        {
            var SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("amnkjnkqnkq fqnfknqfn qnkjwffnkjnkqwn fq f kjq@#m4j9u483213"));
            var SigningCridantioal = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,Password)
            };
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: SigningCridantioal);
            var encodedjwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedjwt;
        }

        //verify1 'T'
        public ActionResult Verify1()
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            return View();
        }

        //verify2 'T'
        public ActionResult Verify2()
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            return View();
        }

        //verify3 'T'
        public ActionResult Verify3()
        {
            if (Session["user"] == null) { return RedirectToAction("Login"); }
            return View();
        }
        
        //release resources
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //__________________________more________________________________//

        /* // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        */

        /* // GET: Users/Delete/5
      public ActionResult Delete(int? id)
      {
          if (id == null)
          {
              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
          }
          User user = db.Users.Find(id);
          if (user == null)
          {
              return HttpNotFound();
          }
          return View(user);
      }

      // POST: Users/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
          User user = db.Users.Find(id);
          db.Users.Remove(user);
          db.SaveChanges();
          return RedirectToAction("Index");
      }*/
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
