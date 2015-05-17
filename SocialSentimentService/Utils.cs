using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SocialSentimentService
{
    public class Utils
    {

        public static string GetFacebookToken()
        {
            return ConfigurationManager.AppSettings["accessToken"];
        }

        public static string GetAlchemyKey()
        {
            return ConfigurationManager.AppSettings["alchemyKey"];
        }
    }
}