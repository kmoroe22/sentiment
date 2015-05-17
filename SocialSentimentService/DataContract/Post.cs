using System;
using System.Globalization;
using System.Runtime.Serialization;


namespace SocialSentimentService.DataContract
{
    [DataContract]
    public class Post
    {
        [DataMember] 
        public string message;
        [DataMember]
        public string type;
        [DataMember]
        public long likes;
        [DataMember]
        public long numComments;
        [DataMember] 
        public string picture;
        [DataMember]
        public string link;
        [DataMember]
        public string status_type;
        [DataMember] 
        public double messageSentiment;
        [DataMember]
        public string sentimentType;
        [DataMember]
        public DateTime created_time;
    }
}