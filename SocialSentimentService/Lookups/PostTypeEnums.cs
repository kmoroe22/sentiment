using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialSentimentService.Lookups
{
    public enum PostTypeEnums
    {
        Tweet = 1,
        Comment = 2,
        StatusUpdate = 3,
        Link = 4,
        Video = 5,
        Picture = 6,
        ProfilePicChange= 7,
        WallPost = 8,
        Post = 9,
        Offer = 12
    }
}