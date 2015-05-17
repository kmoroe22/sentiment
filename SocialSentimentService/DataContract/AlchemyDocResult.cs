using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace SocialSentimentService.DataContract
{

    [DataContract]
    public class AlchemyDocResult
    {
        [DataMember(Name = "score")]
        public double Score { get; set; }
       
        [DataMember(Name = "type")]
        public string Type { get; set; }

    }
}