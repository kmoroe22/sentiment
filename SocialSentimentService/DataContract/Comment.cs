using System;
using System.Runtime.Serialization;
using SocialSentimentService.RepoServiceReference;


namespace SocialSentimentService.DataContract
{
    [DataContract]
    public class Comment
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string fromName { get; set; }
        [DataMember]
        public string fromId { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public DateTime created_time { get; set; }
        [DataMember]
        public long like_count { get; set; }
        [DataMember]
        public double sentimentScore { get; set; }
        [DataMember]
        public string sentimentType { get; set; }
        [DataMember]
        public string PostId { get; set; }
        [DataMember]
        public User from { get; set; }

        public Comment(){}
        public Comment(FbCommentObject comment)
        {
            id = comment.CommentId;
            message = comment.Message;
            created_time = comment.timeAdded;
            like_count = comment.Likes;
            sentimentScore = comment.SentimentScore;
            PostId = comment.Post.PostId;
            fromId = comment.PosterId;
            sentimentType = SetSentimentType(comment.SentimentScore);
        }

        private string SetSentimentType(double score)
        {
            if (score < 0.0)
                return "negative";
            return score > 0.0 ? "positive" : "neutral";
        }
    }


}