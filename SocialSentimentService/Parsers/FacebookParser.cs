using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using Facebook;
using SocialSentimentService.DataContract;
using SocialSentimentService.Lookups;
using SocialSentimentService.RepoServiceReference;

namespace SocialSentimentService.Parsers
{
    public class FacebookParser
    {
        private readonly FacebookClient _facebookClient;
        public int MaxComments = 100;
        public FacebookParser()
        {
            _facebookClient = new FacebookClient(Utils.GetFacebookToken());
        }

        public FacebookParser(FacebookClient client)
        {
            _facebookClient = client;
        }
        
        public ICollection<Comment> ParseComments(String postId)
        {
            ICollection<Comment> comments = new List<Comment>();
            dynamic results = _facebookClient.Get("/" + postId + "/comments?limit="+MaxComments.ToString(), new object { });
            foreach (var comment in results.data)
            {
                comments.Add(new Comment()
                {
                    id = comment.id,
                    fromId = comment.from.id,
                    fromName = comment.from.name,
                    message = comment.message,
                    created_time = ParseFacebookDate(comment.created_time),
                    like_count = comment.like_count,
                    PostId = postId
                });
            }
            return comments;
        }

        public DateTime ParseFacebookDate(String date)
        {
            return DateTime.Parse(date);
        }

        public string GetCommentPostId(string commentid)
        {
            var unauthFbClient = new FacebookClient();
            dynamic id = unauthFbClient.Get("fql", new
            {
                q = new
                {
                    id = "SELECT parent_id,post_id  FROM comment WHERE id = '" + commentid + "'"
                }
            });
            return id.data[0].fql_result_set[0].post_id;
        }

        public AccountObject CreateFacebookAccount(string accountId)
        {
            dynamic account = _facebookClient.Get(accountId);
            return new AccountObject()
            {
                AccountId = accountId,
                About = account.about,
                Link = "https://www.facebook.com/" + accountId,
                Name = account.name
            };
        }

        public PostObject CreatePost(string postid)
        {
            dynamic post= _facebookClient.Get(postid);
            return new PostObject()
            {
                PostId = postid,
                timeAdded = ParseFacebookDate(post.created_time),
                PostTypeObject = new PostTypeObject()
                {
                    PostTypeId = CheckPostType(post.type)
                }
            };
        }

        public Post GetPost(string postid)
        {
            dynamic post = _facebookClient.Get(postid);
            var p = new Post()
            {
                created_time = ParseFacebookDate(post.created_time),
                message = post.message,
                type = post.type
            };
            if (post.likes != null)
                p.likes = post.likes.data.Count;
            if(post.link!=null)
                p.link = post.link;
            if (post.picture != null)
                p.picture = post.picture;
            if (post.status_type != null)
                p.status_type = post.status_type;
            if (post.comments != null)
                p.numComments = post.comments.data.Count;
            return p;
        }

        public ICollection<Post> GetFeed(string accountId, int limit)
        {
            ICollection<Post> posts = new List<Post>();
            dynamic feed = _facebookClient.Get(accountId + "/feed?limit="+limit);
            foreach (var item in feed.data)
            {
                var p = new Post()
                {
                    created_time = ParseFacebookDate(item.created_time),
                    message = item.message,
                    type = item.type
                };
                if (item.likes != null)
                    p.likes = item.likes.data.Count;
                if (item.link != null)
                    p.link = item.link;
                if (item.picture != null)
                    p.picture = item.picture;
                if (item.status_type != null)
                    p.status_type = item.status_type;
                if (item.comments != null)
                    p.numComments = item.comments.data.Count;
                posts.Add(p);
            }
            return posts;
        } 

        public User GetUser(string userid)
        {
            dynamic user = _facebookClient.Get(userid);
            return new User
            {
                id = userid,
                link = user.link,
                name = user.name
            };
        }

        public Comment GetComment(FbCommentObject obj)
        {
            var comment = new Comment(obj);
            comment.from = GetUser(obj.PosterId);
            return comment;
        }

        public ICollection<Comment> GetComments(ICollection<FbCommentObject> objects)
        {
            return objects.Select(GetComment).ToList();
        } 
        
        private static int CheckPostType(string type)
        {
            switch (type)
            {
                case "link":
                    return (int) PostTypeEnums.Link;
                case "status":
                    return (int) PostTypeEnums.StatusUpdate;
                case "photo":
                    return (int) PostTypeEnums.Picture;
                case "video":
                    return (int) PostTypeEnums.Video;
                case "offer":
                    return (int) PostTypeEnums.Offer;
                default:
                    return (int) PostTypeEnums.Post;
            }
        }

    }
}