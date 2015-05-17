using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SocialSentimentService.DataContract
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string id;
        [DataMember]
        public string link;
        [DataMember]
        public string name;
    }
}