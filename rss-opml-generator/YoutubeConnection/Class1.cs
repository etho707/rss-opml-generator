using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YoutubeConnection
{
    /// <summary>
    /// YouTube Data API v3 sample: retrieve my uploads.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://developers.google.com/api-client-library/dotnet/get_started
    /// </summary>
    public class TestMyUploads
    {
        public static void TestMain(string[] args)
        {
            Console.WriteLine("YouTube Data API: My Uploads");
            Console.WriteLine("============================");

            try
            {
                new TestMyUploads().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task Run()
        {
            UserCredential credential;
            using (var stream = new FileStream("C:\\Users\\Mikhail\\Downloads\\client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            var nextPageToken = "";

            var listOfStrings = new List<string>();

            while (nextPageToken != null)
            {
                var subItemsListRequest = youtubeService.Subscriptions.List("snippet");
                subItemsListRequest.Mine = true;
                subItemsListRequest.MaxResults = 50;
                subItemsListRequest.PageToken = nextPageToken;

                // Retrieve the list of videos uploaded to the authenticated user's channel.
                var subItemsListListResponse = await subItemsListRequest.ExecuteAsync();

                foreach (var subItemsList in subItemsListListResponse.Items)
                {
                    // Print information about each video.
                    Console.WriteLine("{0} ({1})", subItemsList.Snippet.Title, subItemsList.Snippet.ChannelId);
                }

                nextPageToken = subItemsListListResponse.NextPageToken;
            }
        }
    }
}

