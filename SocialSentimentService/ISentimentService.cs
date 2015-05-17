using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SocialSentimentService.DataContract;
using SocialSentimentService.RepoServiceReference;

namespace SocialSentimentService
{
    [ServiceContract]
    public interface ISentimentService
    {

//        [OperationContract]
//        string TestFacebookApi();

        [OperationContract]
        [WebGet(UriTemplate = "accounts/{accountId}?userId={userId}", ResponseFormat = WebMessageFormat.Json/*, Method = "POST"*/)]
        AccountObject AddNewAccount(string accountId, int userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "getTextSentiment/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        AlchemySentiment GetTextSentiment(string text);

        [OperationContract]
        [WebInvoke(UriTemplate = "processComments/{postId}/{accountId}", ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        ICollection<Comment> ProcessFacebookComments(string postId, string accountId);

        [OperationContract]
        [WebGet(UriTemplate = "getPost/{postId}", ResponseFormat = WebMessageFormat.Json)]
        Post getPost(string postId);

        [WebGet(UriTemplate = "feed/{accountId}", ResponseFormat = WebMessageFormat.Json)]
        ICollection<Post> GetFeed(string accountId);

        [OperationContract]
        [WebGet(UriTemplate = "getComments/{postId}", ResponseFormat = WebMessageFormat.Json)]
        ICollection<Comment> GetPostComments(string postId);

        [OperationContract]
        [WebGet(UriTemplate = "getNeutral/{postId}", ResponseFormat = WebMessageFormat.Json)]
        ICollection<Comment> GetNeutrals(string postId);

        [OperationContract]
        [WebGet(UriTemplate = "worst/{postId}", ResponseFormat = WebMessageFormat.Json)]
        Comment GetWorstComment(string postId);

        [OperationContract]
        [WebGet(UriTemplate = "best/{postId}", ResponseFormat = WebMessageFormat.Json)]
        Comment GetBestComment(string postId);

         
    }

}
