using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using NUnit.Framework;
using SocialSentimentService.Lookups;
using SocialSentimentService.Parsers;

namespace SocialSentimentService.Test
{
    [TestFixture]
    class FacebookParserTest
    {
        private FacebookParser _parser;
        private const string _takealotPostId = "177010045688404_891794427543292";
        private string _takealotId = "177010045688404";
        private const string _commentId = "891794427543292_893112814078120";
        private const string _discoveryVitalityPostId = "372963979437054_867932619940185";
        private const string RandomProfile = "10153304795229770";

        [SetUp]
        public void setUp()
        {
            _parser = new FacebookParser(new FacebookClient(TestUtils.GetFacebookToken()));
        }

        [Test]
        public void ShouldParseFacebookDateStringToDateObject()
        {
            const string stringDate = "2015-05-11T08:34:00+0000";
            var date = _parser.ParseFacebookDate(stringDate);
            Assert.AreEqual(date.Year, 2015);
            Assert.AreEqual(date.Month, 5);
            Assert.AreEqual(date.Day, 11);
        }


        [Test]
        public void ShouldParseFacebookPayloadToListOfCommentObjects()
        {
            var comments = _parser.ParseComments(_takealotPostId);
            Assert.IsNotNull(comments);
            Assert.IsNotEmpty(comments);
        }

        [Test]
        public void ShouldReturnPostIdWhenCommentIdIsPassed()
        {
            var post = _parser.GetCommentPostId(_commentId);
            Assert.True(post.Equals(_takealotPostId));
        }

        [Test]
        public void ShouldCreateTakealotAccount()
        {
            var account = _parser.CreateFacebookAccount(_takealotId);
            Assert.IsTrue(account.AccountId.Equals(_takealotId));
            Assert.IsTrue(account.Link.EndsWith(_takealotId));
            Assert.IsTrue(account.Name.ToUpper().StartsWith("TAKEALOT"));
        }

        [Test]
        public void ShouldCreateTakeAlotPostObject()
        {
            var post= _parser.CreatePost(_takealotPostId);
            Assert.IsTrue(post.PostId.Equals(_takealotPostId));
            Assert.IsTrue(post.PostTypeObject.PostTypeId.Equals((int)PostTypeEnums.Picture));
            Assert.IsTrue(post.timeAdded.Equals(ParseFacebookDate("2015-05-07T13:59:59+0000")));
        }

        [Test]
        public void ShouldCreatePostWhenPostIdIsPassed()
        {
            var post = _parser.GetPost(_discoveryVitalityPostId);
            Assert.NotNull(post);
        }

        [Test]
        public void ShouldCreateUserWhenUserIdIsParsed()
        {
            var user = _parser.GetUser(RandomProfile);
            Assert.NotNull(user);
            Assert.NotNull(user.id);
            Assert.NotNull(user.name);
            Assert.NotNull(user.link);
        }

        [Test]
        public void ShouldCreate10PostsWhenAccountFeedand10LimitUsPassed()
        {
            var feed = _parser.GetFeed(_takealotId, 10);
            Assert.IsTrue(feed.Count==10);
        }

        public DateTime ParseFacebookDate(String date)
        {
            return DateTime.Parse(date);
        }

    }
}
