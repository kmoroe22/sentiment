using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SocialSentimentService.DataContract;
using SocialSentimentService.Parsers;

namespace SocialSentimentService.Test
{

    [TestFixture]
    class AlchemySentimentParserTest
    {

        private AlchemySentimentParser parser;

        [SetUp]
        public void setUp()
        {
            parser = new AlchemySentimentParser(new AlchemyAPI.AlchemyAPI());
        }

        [Test]
        public void shouldGiveNegativeScoreForBadComment()
        {
            var comment = new Comment()
            {
                message = "I hate this place Its the worst."
            };
            parser.ProcessComment(comment);
            Assert.IsNotNull(comment.sentimentScore);
            Assert.IsNotNull(comment.sentimentType);
            Assert.True(comment.sentimentScore<0.0);
            Assert.True(comment.sentimentType.Equals("negative"));
        }

        [Test]
        public void ShouldGivePositiveScoreForGoodComment()
        {
            var comment = new Comment()
            {
                message = "I am so in love with this place its just so amazing"
            };
            parser.ProcessComment(comment);
            Assert.IsNotNull(comment.sentimentScore);
            Assert.IsNotNull(comment.sentimentType);
            Assert.True(comment.sentimentScore > 0.0);
            Assert.True(comment.sentimentType.Equals("positive"));
        }

        [Test]
        public void ShouldGiveNeutralScoreForNeutralComment()
        {
            var comment = new Comment()
            {
                message = "#7"
            };
            parser.ProcessComment(comment);
            Assert.IsNotNull(comment.sentimentScore);
            Assert.IsNotNull(comment.sentimentType);
            Assert.True(comment.sentimentScore > -1 && comment.sentimentScore < 1);
            Assert.True(comment.sentimentType.Equals("neutral"));
        }

        [Test]
        public void ShouldGiveNeutralScoreForSenselessComments()
        {
            var comment = new Comment()
            {
                message = "Ooo def. Meeeeeeee"
            };
            parser.ProcessComment(comment);
            Assert.IsNotNull(comment.sentimentScore);
            Assert.IsNotNull(comment.sentimentType);
            Assert.True(comment.sentimentScore > -1 && comment.sentimentScore < 1);
            Assert.True(comment.sentimentType.Equals("neutral"));
        }
    }

}
