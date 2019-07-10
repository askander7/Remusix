using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RemusixApi.Models;
namespace RemusixApi.Controllers
{
    public class PostsController : ApiController
    {
         private RemusixDBEntities db = new RemusixDBEntities();

        // return all users(usernames , photos)
        [HttpGet]
       public IEnumerable<UserNamePhoto> AllUsers()
        {
            var q = from u in db.Users
                    select new
                    {
                        u.UserID,
                        u.UserName,
                        u.Photo
                    };
            List<UserNamePhoto> users = new List<UserNamePhoto>();
            foreach (var item in q)
            {
                 users.Add(new UserNamePhoto()
                {
                     UserID = item.UserID,
                     UserName = item.UserName,
                    Photo = item.Photo
                });
            }

            return users;
        }


        //return all posts
        [HttpGet]
        public IEnumerable<CustomPost> GetAllPosts()
        {
            var q = (from p in db.Posts join s in db.Songs on p.SongID equals s.SongID
                    join ar in db.Artists on p.ArtistID equals ar.ArtistID
                    join u in db.Users on p.UserID equals u.UserID
                    select new
                    {
                        p.PostID,
                        s.SongName,
                        ar.ArtistName,
                        ar.ArtistPhoto,
                        u.UserName,
                        u.Photo,
                        p.PostTime
                    }).OrderByDescending(p=>p.PostID);
            List<CustomPost> CustomPosts = new List<CustomPost>();
            foreach (var item in q)
            {
                CustomPosts.Add(new CustomPost()
                {
                    PostID = item.PostID,
                    SongName=item.SongName,
                    ArtistName = item.ArtistName,
                    ArtistPhoto = item.ArtistPhoto,
                    UserName = item.UserName,
                    Photo = item.Photo,
                    PostTime = item.PostTime
                });
            }

            return CustomPosts;
        }

        // return post by id
        [HttpGet]
        public CustomPost GetPost(int s_postid)
        {
            return GetAllPosts().FirstOrDefault(p => p.PostID == s_postid);
        }

        // create post in database
        [HttpPost]
        public bool CreatePost(Post post)
         {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return true;
            }
            return false;
        }


        // get user's posts by username and all friends' posts
        [HttpGet]
        public IEnumerable<CustomPost> UserPosts(string s_username)
        {   if (s_username == null) return null;

            IEnumerable<CustomPost> AllPosts = GetAllPosts();
            List<CustomPost> posts = new List<CustomPost>();
            posts.AddRange(AllPosts.Where(u => u.UserName.Equals(s_username)));
            int uid = db.Users.FirstOrDefault(u => u.UserName.Equals(s_username)).UserID;
            IEnumerable<int> fids = db.Friends.Where(u => u.UserID == uid).Select(u => u.FriendID);
            foreach (int fid in fids)
            {
                string fname = db.Users.FirstOrDefault(u => u.UserID == fid).UserName;
                posts.AddRange(AllPosts.Where(u => u.UserName.Equals(fname)));
            }
            return posts;
        }


        // return users(username,photo) who like some post
        [HttpGet]
        public IEnumerable<UserNamePhoto> UsersLikePost(int s_postid)
        {
            IEnumerable<int> uids = db.UserLikePosts.Where(u => u.PostID == s_postid).Select(u => u.UserID);
            List<UserNamePhoto> users = new List<UserNamePhoto>();
            foreach (int uid in uids)
            {
                users.Add(AllUsers().FirstOrDefault(u => u.UserID == uid));
            }
            return users;
        }


        // make user like some post
        [HttpGet]
        public string LikeDislike(int s_postid,int s_userid)
        {
            if (db.UserLikePosts.Any(p => p.PostID == s_postid && p.UserID == s_userid))
            {
                db.UserLikePosts.Remove(db.UserLikePosts.FirstOrDefault(p => p.PostID == s_postid && p.UserID == s_userid) as UserLikePost);
                db.SaveChanges();
                return "disliked!";
            }
            else
            {
                DateTime time = DateTime.Now;
                int nUserID = db.Posts.FirstOrDefault(u => u.PostID == s_postid).UserID;
                AddNoti(s_userid, nUserID, s_postid, "like", time);
                UserLikePost userLikePost = new UserLikePost() { PostID = s_postid, UserID = s_userid, LikeTime = time, empty = "" };
                db.UserLikePosts.Add(userLikePost as UserLikePost);
                db.SaveChanges();
                return "liked!";
            }
            
        }


        // make user comment some post
        [HttpPost]
        public IHttpActionResult AddComment(UserCommentPost userCommentPost)
        {
                DateTime time = DateTime.Now;
                int nUserID = db.Posts.FirstOrDefault(u => u.PostID == userCommentPost.PostID).UserID;
                AddNoti(userCommentPost.UserID, nUserID, userCommentPost.PostID, "comment", time);
                userCommentPost.CommentTime = time;
                db.UserCommentPosts.Add(userCommentPost);
                db.SaveChanges();
                return Ok("commented!");
        }


        //remove comment by datetime
        [HttpGet]
        public IHttpActionResult RemoveComment(DateTime s_dateTime)
        {
            if (s_dateTime != null)
            {
                db.UserCommentPosts.Remove(db.UserCommentPosts.FirstOrDefault(p=>p.CommentTime.Equals(s_dateTime))as UserCommentPost);
                db.SaveChanges();
                return Ok("comment removed!");
            }
            return BadRequest("not datetime!");
        }


        //// return all comments and users(name,photo) who comment of some post(id)
        [HttpGet]
        public IEnumerable<CustomComment> GetComments(int s_postid)
        {
            var q = from ucp in db.UserCommentPosts
                    join u in db.Users on ucp.UserID equals u.UserID
                    where ucp.PostID==s_postid
                    select new
                    {
                        u.UserName,
                        u.Photo,
                        ucp.Comment,
                        ucp.CommentTime
                    };
            List<CustomComment> comments = new List<CustomComment>();
            foreach (var item in q)
            {
                comments.Add(new CustomComment()
                {
                    UserName=item.UserName,
                    Photo=item.Photo,
                    Comment=item.Comment,
                    CommentTime=item.CommentTime
                });
            }
            return comments;
        }


        // Create notification
        public void AddNoti(int UserID,int nUserID,int? PostID,string Action,DateTime ActionTime)
        {
            db.Notifications.Add(new Notification() {UserID=UserID,nUserID=nUserID,PostID=PostID,Action=Action,
                ActionTime = ActionTime,Status="unseen" });
            db.SaveChanges();
        }

        //return all notifications
        [HttpGet]
        public IEnumerable<CustomNotification> GetNotifications()
        {
            var q = (from n in db.Notifications
                     select new
                     {
                         n.UserID,
                         n.nUserID,
                         n.PostID,
                         n.Action,
                         n.ActionTime,
                         n.Status
                     }).OrderByDescending(n => n.ActionTime);
            List<CustomNotification> notifications = new List<CustomNotification>();
            foreach (var item in q)
            {
                notifications.Add(new CustomNotification()
                {
                    UserID = item.UserID,
                    nUserID = item.nUserID,
                    PostID = item.PostID,
                    Action = item.Action,
                    ActionTime = item.ActionTime,
                    Status = item.Status
                });
            }
            return notifications;
        }

        //return number of likes on some post
        [HttpGet]
        public int LikesNum(int s_postid)
        {
            return db.UserLikePosts.Where(p => p.PostID == s_postid).Count();
        }

        //return number of comments on some post

        [HttpGet]
        public int CommentsNum(int s_postid)
        {
            return db.UserCommentPosts.Where(p => p.PostID == s_postid).Count();
        }

        //______________________________more_______________________________//
        /*  // users liked posts
          [HttpGet]
          [ResponseType(typeof(CustomPost))]
          public IHttpActionResult UserLikePosts(int s_userid)
          {
              IEnumerable<int> PostsID = db.UserLikePosts.Where(u => u.UserID == s_userid).Select(p => p.PostID);
              List<CustomPost> UserLikePosts = new List<CustomPost>();
              //get all posts
              PostsController pc = new PostsController();
              List<CustomPost> AllPosts = pc.GetAllPosts().ToList();
              foreach (int PostID in PostsID)
              {
                  UserLikePosts.Add(AllPosts.FirstOrDefault(p => p.PostID == PostID));
              }
              return Ok(UserLikePosts);
          }*/
    }
}
