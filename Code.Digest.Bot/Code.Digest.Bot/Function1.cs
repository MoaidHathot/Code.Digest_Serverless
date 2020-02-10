using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.WindowsAzure.Storage.Blob;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace Code.Digest.Bot
{
    public static class Function1
    {
        [FunctionName(nameof(FetchTweetsTimer))]
        public static void FetchTweetsTimer([TimerTrigger("0 * * * * *")]TimerInfo timerInfo, ILogger log)
        {
            var consumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
            var consumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
            var accessToken= Environment.GetEnvironmentVariable("Accesstoken");
            var accessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret");
        
        }
    }
}
