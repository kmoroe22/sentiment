using System.Runtime.Serialization;

namespace SocialSentimentService.DataContract
{

    [DataContract]
    public class AlchemySentiment
    {
        [DataMember(Name = "status")] 
        public string Status { get; set; }

        [DataMember(Name = "usage")]
        public string Usage { get; set; }

        [DataMember(Name = "totalTransactions")]
        public int TotalTransactions { get; set; }

        [DataMember(Name = "docSentiment")]
        public AlchemyDocResult DocSentiment { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }


    }
}