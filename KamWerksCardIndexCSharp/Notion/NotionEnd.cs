using Notion.Client;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using KamWerksCardIndexCSharp.Helpers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;

namespace KamWerksCardIndexCSharp.Notion
{
    internal class NotionEnd
    {
        public static List<string> CtiCardNames { get; private set; } = new();
        public static List<string> CtiSigilNames { get; private set; } = new();
        public static ConcurrentDictionary<string, string> CtiCards { get; private set; } = new();
        public static ConcurrentDictionary<string, string> CtiSigils { get; private set; } = new();
        public static List<string> DmcCardNames { get; private set; } = new();
        public static List<string> DmcSigilNames { get; private set; } = new();
        public static ConcurrentDictionary<string, string> DmcCards { get; private set; } = new();
        public static ConcurrentDictionary<string, string> DmcSigils { get; private set; } = new();
        
        public static List<string> IotfdCardNames { get; private set; } = new();
        public static List<string> IotfdSigilNames { get; private set; } = new();
        public static ConcurrentDictionary<string, string> IotfdCards { get; private set; } = new();
        public static ConcurrentDictionary<string, string> IotfdSigils { get; private set; } = new();

        private static List<string> previousSnapshot = new();

        public static async Task NotionMain()
        {
            var logger = LoggerFactory.CreateLogger("console");
            string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            if (string.IsNullOrWhiteSpace(NotionAPIKey))
            {
                logger.Error("Hey, You missed the Notion API Key Environment Var.");
                return;
            }

            logger.Info("Connecting to Notion API...");
            var client = NotionClientFactory.Create(new ClientOptions { AuthToken = NotionAPIKey });

            var CtiCardPagesList = new List<string>();
	        CtiCardPagesList = await FetchAllPageIds(client, "e19c88aa75b44bfe89321bcde8dc7d9f");
            CtiCards = await FetchPageNamesAndStore(client, CtiCardPagesList, "Card", CtiCardNames);

            var CtiSigilPagesList = new List<string>();
            CtiSigilPagesList = await FetchAllPageIds(client, "933d6166cb3f4ee89db51e4cf464f5bd");
            CtiSigils = await FetchPageNamesAndStore(client, CtiSigilPagesList, "Sigil", CtiSigilNames);

            var DmcCardPagesList = new List<string>();
            DmcCardPagesList = await FetchAllPageIds(client, "1229bd3134c34f69a369c5ef830bd7a0");
            DmcCards = await FetchPageNamesAndStore(client, DmcCardPagesList, "Card", DmcCardNames);

            var DmcSigilPagesList = new List<string>();
            DmcSigilPagesList = await FetchAllPageIds(client, "ae46b70d77e649d78a94d2fc62a886e0");
            DmcSigils = await FetchPageNamesAndStore(client, DmcSigilPagesList, "Sigil", DmcSigilNames);
            
            var IotfdCardPagesList = new List<string>();
            IotfdCardPagesList = await FetchAllPageIds(client, "8918fd0a983540308f80f131655db3d3");
            IotfdCards = await FetchPageNamesAndStore(client, IotfdCardPagesList, "Card", IotfdCardNames);

            var IotfdSigilPagesList = new List<string>();
            IotfdSigilPagesList = await FetchAllPageIds(client, "aa7158c620e14c1bade56faa7c09cd63");
            IotfdSigils = await FetchPageNamesAndStore(client, IotfdSigilPagesList, "Sigil", IotfdSigilNames);

            logger.Info("Notion data retrieval completed successfully.");
        }

        private static async Task<List<string>> FetchAllPageIds(NotionClient client, string databaseId)
        {
            List<string> pageIds = new();
            string? nextCursor = null;

            do
            {
                var queryParams = new DatabasesQueryParameters
                {
                    StartCursor = nextCursor,
                    PageSize = 100
                };

                var response = await RetryWithBackoff(() => client.Databases.QueryAsync(databaseId, queryParams));

				// Log the response from Notion API
                Console.WriteLine($"Received {response.Results.Count} results for database {databaseId}");

                pageIds.AddRange(response.Results.Select(page => page.Id));

                nextCursor = response.HasMore ? response.NextCursor : null;
            } while (!string.IsNullOrEmpty(nextCursor));

            return pageIds;
        }

        public static async Task<T> RetryWithBackoff<T>(Func<Task<T>> action, int maxRetries = 50)
        {
            int retryCount = 0;
            int delay = 1000;

            while (true)
            {
                try
                {
                    return await action();
                }
                catch (NotionApiRateLimitException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    int waitTime = ex.RetryAfter?.Milliseconds + 50 ?? delay;

                    Console.WriteLine($"Rate limited! Retrying in {waitTime}ms...");
                    await Task.Delay(waitTime);

                    retryCount++;
                    delay *= 2;
                }
            }
        }

        private static async Task<ConcurrentDictionary<string, string>> FetchPageNamesAndStore(NotionClient client, List<string> pageIds, string itemType, List<string> nameList)
        {
	        var logger = LoggerFactory.CreateLogger("console");
	        ConcurrentDictionary<string, string> pageNameDict = new();

	        var tasks = pageIds.Select(async id =>
	        {
		        var page = await RetryWithBackoff(() => client.Pages.RetrieveAsync(id));

		        if (page.Properties.TryGetValue("Internal Name", out PropertyValue nameValue) && nameValue is TitlePropertyValue titleProperty)
		        {
			        string nameText = titleProperty.Title.FirstOrDefault()?.PlainText ?? "Unnamed";
			        if (nameList != null) nameList.Add(nameText);
			        pageNameDict[nameText] = id;

			        // Log each retrieved page for debugging
			        logger.Info($"Retrieved {itemType} Name: {nameText} (ID: {id})");
		        }
		        else
		        {
			        // Log any failed attempts to retrieve data
			        logger.Warning($"Failed to retrieve {itemType} Name for ID: {id}");
		        }
	        }).ToList();

	        await Task.WhenAll(tasks);
	        return pageNameDict;
        }
    }
}