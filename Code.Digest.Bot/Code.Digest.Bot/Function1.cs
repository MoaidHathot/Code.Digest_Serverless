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
        public static async Task FetchTweetsTimer([TimerTrigger("0 * * * * *")]TimerInfo timerInfo, ILogger log, [Blob("twitter-cache/lastExecution.json", FileAccess.ReadWrite)] CloudBlockBlob cache)
        {
            var consumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
            var consumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
            var accessToken= Environment.GetEnvironmentVariable("Accesstoken");
            var accessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret");

            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            var searchTerm = Environment.GetEnvironmentVariable("SearchTerm");

            long lastId = default;

            if (await cache.ExistsAsync())
            {
                lastId = long.Parse(await cache.DownloadTextAsync());
            }

            var searchArgs = new SearchTweetsParameters(searchTerm)
            {
                SinceId = lastId
            };

            var tweets = await SearchAsync.SearchTweets(searchArgs);


            if (tweets.Any())
            {
                lastId = tweets.Max(tweet => tweet.Id);
                await cache.UploadTextAsync(lastId.ToString());

            }

            foreach (var tweet in tweets)
            {
                Tweet.PublishRetweet(tweet.Id);
                log.LogInformation("Published {tweet} - {id} - {name}", tweet.Url, tweet.Id, tweet.CreatedBy.ScreenName);
            }

        }
    }
}
