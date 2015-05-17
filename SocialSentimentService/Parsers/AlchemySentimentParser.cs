using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SocialSentimentService.DataContract;

namespace SocialSentimentService.Parsers
{
    public class AlchemySentimentParser
    {
        private AlchemyAPI.AlchemyAPI _alchemyObj;

        public AlchemySentimentParser(AlchemyAPI.AlchemyAPI alchemy)
        {
            _alchemyObj = alchemy;
            _alchemyObj.SetAPIKey(Utils.GetAlchemyKey());
        }

        public AlchemySentimentParser()
        {
            _alchemyObj = new AlchemyAPI.AlchemyAPI();
            _alchemyObj.SetAPIKey(Utils.GetAlchemyKey());
        }

        public void ProcessComment(Comment comment)
        {
            var text = comment.message;
            if(text.Length< 5)
                text = text +"     ";
            try
            {
                var sentiment = ParseSentimentXml(_alchemyObj.TextGetTextSentiment(text));
                comment.sentimentScore = sentiment.DocSentiment.Score;
                comment.sentimentType = sentiment.DocSentiment.Type;
            }
            catch (Exception)
            {
                comment.sentimentScore = 0.0;
                comment.sentimentType = "neutral";
            }
        }

        public void ProcessComments(ICollection<Comment> comments)
        {
            foreach (var comment in comments)
            {
                ProcessComment(comment);
            }
        }

        private static AlchemySentiment ParseSentimentXml(string xml)
        {
            var document = XDocument.Parse(xml);
            var xElement = document.Element("results");
            if (xElement == null) return new AlchemySentiment();
            var sentiment = new AlchemySentiment()
            {
                Language = (string)xElement.Element("language"),
                TotalTransactions = (int)xElement.Element("totalTransactions"),
                Usage = (string)xElement.Element("usage"),
                Status = (string)xElement.Element("status"),
                DocSentiment = new AlchemyDocResult()
                {
                    Score = DefaultNeutralScore(xElement.Element("docSentiment").Element("score")),
                    Type = (string)xElement.Element("docSentiment").Element("type")
                }
            };
            return sentiment;
        }

        private static double DefaultNeutralScore(dynamic arg)
        {
            if (arg == null)
                return 0;
            else
            {
                return (double)arg;
            }
        }

        public void ProcessPostSentiments(Post post)
        {
            var text = post.message;
            if (text.Length < 5)
                text = text + "     ";
            try
            {
                var sentiment = ParseSentimentXml(_alchemyObj.TextGetTextSentiment(text));
                post.messageSentiment= sentiment.DocSentiment.Score;
                post.sentimentType = sentiment.DocSentiment.Type;
            }
            catch (Exception)
            {
                post.messageSentiment= 0.0;
                post.sentimentType = "neutral";
            }
        }
    }
}