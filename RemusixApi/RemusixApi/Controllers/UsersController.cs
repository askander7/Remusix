using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.IdentityModel.Tokens;
using RemusixApi.Models;
namespace RemusixApi.Controllers

{
    public class UsersController : ApiController
    {
        private RemusixDBEntities db = new RemusixDBEntities();




        //get all users
        [HttpGet]
        public IEnumerable<CustomUser> GetAllUsers()
        {
            var q = from u in db.Users
                    select new
                    {
                        u.UserID,
                        u.UserName,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.UserPassword,
                        u.Photo,
                        u.Gender,
                        u.FacebookUsername,
                        u.GoogleUsername,
                        u.SoundcloudUsername,
                        u.Birthdate,
                        u.Longitude,
                        u.Latitude,
                        u.Status,
                        u.Location
                    };
            List<CustomUser> CustomUsers = new List<CustomUser>();
            foreach (var item in q)
            {
                CustomUsers.Add(new CustomUser()
                {
                    UserID = item.UserID,
                    UserName = item.UserName,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    UserPassword = item.UserPassword,
                    Photo = item.Photo,
                    Gender = item.Gender,
                    FacebookUsername = item.FacebookUsername,
                    GoogleUsername = item.GoogleUsername,
                    SoundcloudUsername = item.SoundcloudUsername,
                    Birthdate = item.Birthdate,
                    Longitude=item.Longitude,
                    Latitude=item.Latitude,
                    Status=item.Status,
                    Location=item.Location
                });
            }

            return CustomUsers;
        }


        //get one user by id for admin
        [HttpGet]
        public CustomUser GetUser(int s_userid)
        {
            return GetAllUsers().FirstOrDefault(u => u.UserID.Equals(s_userid));
        }


        //get one user by email for users
        [HttpGet]
        public CustomUser GetUser(string s_email)
        {
            return GetAllUsers().FirstOrDefault(u => u.Email.Equals(s_email));
        }


        //login->return a user
        [HttpGet]
        [ResponseType(typeof(User))]
        public IHttpActionResult Login(string s_email, string s_password)
        {
            s_password = CreateJwrPackage(s_password);
            if (db.Users.Any(u => u.Email.Equals(s_email)))
            {
                if (db.Users.Any(u => u.Email.Equals(s_email) && u.UserPassword.Equals(s_password)))
                {
                    return Ok(GetAllUsers().FirstOrDefault(u => u.Email.Equals(s_email) && u.UserPassword.Equals(s_password)));
                }
                return BadRequest("wrong password");
            }
            return BadRequest("not registered!");
        }


        //check user user's existence by email
        [HttpGet]
        public bool IsUserEmail(string s_email)
        {
           return db.Users.Any(u => u.Email.Equals(s_email))?true:false;
        }


        //check user user's existence by username
        [HttpGet]
        public bool IsUserUsername(string s_username)
        {
           return db.Users.Any(u => u.UserName.Equals(s_username))?true:false;
        }


        // add new user
        [HttpPost]
        public bool Register(User user)
        {
            user.UserPassword = CreateJwrPackage(user.UserPassword);
            user.Status = "active";
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }


        //update user account
        [HttpPost]
        public IHttpActionResult ChangeUser(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Ok("updated!");
            }
            return BadRequest("invalid user!");
        }



        //update user's location by username
        [HttpGet]
        public IHttpActionResult ChangeLocation(string s_username,double s_longitude,double s_latitude)
        {
            if (s_username == null)
                return BadRequest("null username");

                User user = db.Users.FirstOrDefault(u => u.UserName.Equals(s_username));
            if (user == null)
                return BadRequest("no such user exist");

                user.Longitude = s_longitude; user.Latitude = s_latitude;
                   db.Entry(user).State = EntityState.Modified;
                   db.SaveChanges();
            return Ok("location updated!");
        }


        //return user's location by username
        [HttpGet]
        public double[] UserLocation(string s_username)
        {
            double[] location = new double[2];
            location[0]=(double) db.Users.FirstOrDefault(u => u.UserName.Equals(s_username)).Longitude;
            location[1] = (double)db.Users.FirstOrDefault(u => u.UserName.Equals(s_username)).Latitude;
            return location;
        }


        //return nearby users
        [HttpGet]
        public IEnumerable<CustomUser> NearbyUsers(string s_username, double s_longitude, double s_latitude)
        {
            if (s_username == null) return null;

            IEnumerable<CustomUser> Users = GetAllUsers();
             return Users.Where(u =>u.Longitude!=null&& Math.Abs((double)u.Longitude - (double)s_longitude) <= 5 &&
                 u.Latitude!=null&&Math.Abs((double)u.Latitude - (double)s_latitude) <= 5 && !u.UserName.Equals(s_username));
        }


        // return all followers of the user
        [HttpGet]
        public IEnumerable<CustomUser> Followers(string s_username)
        {
            if (s_username == null) return null;
            IEnumerable<CustomUser> AllUsers = GetAllUsers();
            int uid = AllUsers.FirstOrDefault(u => u.UserName.Equals(s_username)).UserID;
            IEnumerable<int> fids = db.Friends.Where(f => f.FriendID == uid).Select(f => f.UserID);
            List<CustomUser> followers = new List<CustomUser>();
            foreach (int fid in fids)
            {
                followers.Add(AllUsers.FirstOrDefault(u => u.UserID == fid));
            }
            return followers;
        }


        // return all followers of the user
        [HttpGet]
        public IEnumerable<CustomUser> Followings(string s_username)
        {
            if (s_username == null) return null;
            IEnumerable<CustomUser> AllUsers = GetAllUsers();
            int uid = AllUsers.FirstOrDefault(u => u.UserName.Equals(s_username)).UserID;
            IEnumerable<int> fids = db.Friends.Where(f => f.UserID == uid).Select(f => f.FriendID);
            List<CustomUser> followings = new List<CustomUser>();
            foreach (int fid in fids)
            {
                followings.Add(AllUsers.FirstOrDefault(u => u.UserID == fid));
            }
            return followings;
        }


        // follow some user
        [HttpPost]
        public IHttpActionResult Follow(Friend friend)
        {
            if (ModelState.IsValid)
            {
                if (db.Friends.Any(f => f.UserID == friend.UserID && f.FriendID == friend.FriendID)) { return BadRequest("aleardy friend"); }
                db.Friends.Add(friend);
                db.SaveChanges();
                new PostsController().AddNoti(friend.UserID, friend.FriendID, null, "follow", DateTime.Now);
                return Ok("followed!");
            }
            return BadRequest("invalid type");
        }

        //unfollow some user
        [HttpPost]
        public IHttpActionResult UnFollow(Friend friend)
        {
            if (ModelState.IsValid)
            {
                if (!db.Friends.Any(f => f.UserID == friend.UserID && f.FriendID == friend.FriendID)) { return BadRequest("not friend"); }
                db.Friends.Remove(db.Friends.FirstOrDefault(f=>f.UserID==friend.UserID&&f.FriendID==friend.FriendID));
                db.SaveChanges();
                return Ok("unfollowed!");
            }
            return BadRequest("invalid type");
        }

        //check friendship
        [HttpGet]
        public bool IsFriend(int s_userid,int s_friendid)
        {
           return db.Friends.Any(f => f.UserID == s_userid && f.FriendID == s_friendid) ? true : false;
        }

        // take encoded image from anfroid
        [HttpPost]
        public IHttpActionResult Upload(UserPhoto userPhoto)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Find(userPhoto.UserID);
                if (user == null) return BadRequest("no such user");
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Ok("uploaded!");
            }
            return BadRequest("invalid request");
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
            return  encodedjwt;
        }


   


        //_____________________more____________________/

        /*
                 // users's friends
        [HttpGet]
        public IEnumerable<CustomUser> UserFriends(int s_userid)
        {
            IEnumerable<int> fids = db.Friends.Where(u => u.UserID == s_userid).Select(p => p.FriendID);
            List<CustomUser> UserFriends = new List<CustomUser>();
            //get all users
            UsersController uc = new UsersController();
            List<CustomUser> Users = uc.GetAllUsers().ToList();
            foreach (int fid in fids)
            {
                UserFriends.Add(Users.FirstOrDefault(p => p.UserID == fid));
            }
            return UserFriends;
        }

                //delete user account
        [HttpDelete]
        public IHttpActionResult RemoveUser(int s_id)
        {
            User user = db.Users.Find(s_id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return Ok("removed!");
            }
            return BadRequest("no such user");
        }

                //get all artists
        [HttpGet]
        public IEnumerable<CustomArtist> AllArtists()
        {
            var q = from a in db.Artists
                    select new
                    {
                        a.ArtistID,
                        a.ArtistName,
                        a.ArtistPhoto
                    };
            List<CustomArtist> CustomArtists = new List<CustomArtist>();
            foreach (var item in q)
            {
                CustomArtists.Add(new CustomArtist()
                {
                    ArtistID = item.ArtistID,
                    ArtistName = item.ArtistName,
                    ArtistPhoto = item.ArtistPhoto
                });
            }

            return CustomArtists;
        }

        // users follows artists
        [HttpGet]
        public IEnumerable<CustomArtist> UserArtists(int s_userid)
        {
            IEnumerable<int> ArtistsID = db.UserFollowArtists.Where(u => u.UserID == s_userid).Select(p => p.ArtistID);
            List<CustomArtist> UserArtists = new List<CustomArtist>();
            //get all artists
            UsersController uc = new UsersController();
            List<CustomArtist> AllArtists = uc.AllArtists().ToList();
            foreach (int ArtistID in ArtistsID)
            {
                UserArtists.Add(AllArtists.FirstOrDefault(a => a.ArtistID == ArtistID));
            }
            return UserArtists;
        }
         */
    }
}
