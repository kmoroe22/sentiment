using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using SocialSentimentService.DataContract;
using SocialSentimentService.RepoServiceReference;

namespace SocialSentimentService.SentimentAdapters
{
    public class RepositoryAdapter
    {

        private readonly RepositoryServiceClient _client;

        public RepositoryAdapter(RepositoryServiceClient client)
        {
            _client = client;
        }

        public void UploadComments(ICollection<Comment> comments)
        {
            var dbComments = comments.Select(comment => new FbCommentObject()
            {
                CommentId = comment.id, 
                Likes = comment.like_count, 
                Message = comment.message, 
                PictureUrl = "", 
                PosterId = comment.fromId, 
                timeAdded = comment.created_time,
                Post = new PostObject()
                {
                    PostId = comment.PostId
                },
                SentimentScore = comment.sentimentScore
            }).ToList();
            _client.UploadComments(dbComments.ToArray());
        }
    }
}