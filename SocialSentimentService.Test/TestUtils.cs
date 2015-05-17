using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialSentimentService.Test
{
    class TestUtils
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
