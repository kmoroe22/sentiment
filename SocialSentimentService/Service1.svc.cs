using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using System.Xml.Linq;
using Facebook;
using NUnit.Framework;
using SocialSentimentService.DataContract;
using SocialSentimentService.Parsers;
using SocialSentimentService.RepoServiceReference;
using SocialSentimentService.SentimentAdapters;

namespace SocialSentimentService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SentimentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SentimentService.svc or SentimentService.svc.cs at the Solution Explorer and start debugging.
    public class SentimentService : ISentimentService
    {

        private readonly RepositoryServiceClient _repository = new RepositoryServiceClient();
        private readonly FacebookClient _facebookClient = new FacebookClient(Utils.GetFacebookToken());
        private readonly AlchemyAPI.AlchemyAPI _alchemyObj = new AlchemyAPI.AlchemyAPI();

        public string TestFacebookApi()
        {
            var client = new FacebookClient(Utils.GetFacebookToken());
            dynamic me = client.Get("177010045688404", new { fields = "name,id,about,link" });
            return me.ToString();
        }

        public AccountObject AddNewAccount(string accountId, int userId)
        {
            dynamic result =  _facebookClient.Get(accountId, new {fields = "name,id,about,link"});
            var account = new AccountObject()
            {
                AccountId = result.id,
                Name = result.name,
                About = result.about,
                Link = result.link                
            };
            return _repository.AddFacebookPageAccount(userId, account);
        }

        public AlchemySentiment GetTextSentiment(string text)
        {
            _alchemyObj.SetAPIKey(Utils.GetAlchemyKey());
            var xmlText = _alchemyObj.TextGetTextSentiment(text);
            return ParseSentimentXml(xmlText);
        }

        public ICollection<Comment> ProcessFacebookComments(string postId, string accountId)
        {
            var adapter = new FacebookSentimentAdapter();
            var repoAdapter = new RepositoryAdapter(_repository);
            var fbParser = CreateFacebookParser();

            if (!_repository.AccountExists(accountId))
                _repository.AddFacebookPageAccount(GetUser(), fbParser.CreateFacebookAccount(accountId));
            if(!_repository.PostExists(postId))
                _repository.AddPost(fbParser.CreatePost(postId),accountId);
            var comments= adapter.ProcessPostComments(postId);
            repoAdapter.UploadComments(comments);
            return comments;
        }

        public Post getPost(string postId)
        {
            var adapter = new FacebookSentimentAdapter();
            return adapter.GetPostSentiment(postId);
        }

        public ICollection<Post> GetFeed(string accountId)
        {
            return CreateFacebookParser().GetFeed(accountId,20);
        }

        public ICollection<Comment> GetPostComments(string postId)
        {
            return CreateFacebookParser().GetComments(_repository.GetCommentsByPosts(postId));
        }

        public ICollection<Comment> GetNeutrals(string postId)
        {
            return CreateFacebookParser().GetComments(_repository.GetNeutralComments(postId));
        }

        public Comment GetWorstComment(string postId)
        {
            return CreateFacebookParser().GetComment(_repository.GetWorstComment(postId));
        }

        public Comment GetBestComment(string postId)
        {
            return CreateFacebookParser().GetComment(_repository.GetBestComment(postId));
        }

        private FacebookParser CreateFacebookParser()
        {
            return new FacebookParser(_facebookClient);
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
                    Score = (double)xElement.Element("docSentiment").Element("score"),
                    Type = (string)xElement.Element("docSentiment").Element("type")
                }
            };
            return sentiment;
        }

        private int GetUser()
        {
            //TODO
            return 2;
        }
    }
}
