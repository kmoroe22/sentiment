using System.Collections.Generic;
using System.Web.UI.WebControls;
using Facebook;
using SocialSentimentService.DataContract;
using SocialSentimentService.Parsers;

namespace SocialSentimentService.SentimentAdapters
{
    public class FacebookSentimentAdapter
    {
        private FacebookParser _facebookParser;
        private AlchemySentimentParser _sentimentParser;


        public FacebookSentimentAdapter()
        {
            _facebookParser  = new FacebookParser();
            _sentimentParser = new AlchemySentimentParser();
        }


        public FacebookSentimentAdapter(FacebookClient fbClient,AlchemyAPI.AlchemyAPI alchemyApi)
        {
            _facebookParser = new FacebookParser(fbClient);
            _sentimentParser = new AlchemySentimentParser(alchemyApi);
        }

        public ICollection<Comment> ProcessPostComments(string postId)
        {
            var comments = _facebookParser.ParseComments(postId);
            _sentimentParser.ProcessComments(comments);
            return comments;
        }

        public Post GetPostSentiment(string postId)
        {
            var post = _facebookParser.GetPost(postId);
            _sentimentParser.ProcessPostSentiments(post);
            return post;
        } 
    }
}