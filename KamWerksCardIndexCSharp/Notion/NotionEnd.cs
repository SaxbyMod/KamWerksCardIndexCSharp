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

        private static List<string> previousSnapshot = new();

        public static async Task NotionMain(string[] args)
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

            var CtiCardPagesList = await FetchAllPageIds(client, "e19c88aa75b44bfe89321bcde8dc7d9f");
            CtiCards = await FetchPageNamesAndStore(client, CtiCardPagesList, "Card", CtiCardNames);

            var CtiSigilPagesList = await FetchAllPageIds(client, "933d6166cb3f4ee89db51e4cf464f5bd");
            CtiSigils = await FetchPageNamesAndStore(client, CtiSigilPagesList, "Sigil", CtiSigilNames);

            var DmcCardPagesList = await FetchAllPageIds(client, "1229bd3134c34f69a369c5ef830bd7a0");
            DmcCards = await FetchPageNamesAndStore(client, DmcCardPagesList, "Card", DmcCardNames);

            var DmcSigilPagesList = await FetchAllPageIds(client, "ae46b70d77e649d78a94d2fc62a886e0");
            DmcSigils = await FetchPageNamesAndStore(client, DmcSigilPagesList, "Sigil", DmcSigilNames);

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

        private static async Task<T> RetryWithBackoff<T>(Func<Task<T>> action, int maxRetries = 50)
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
                    int waitTime = ex.RetryAfter?.Milliseconds ?? delay;

                    if (retryCount >= maxRetries)
                        throw new Exception($"Max retries reached. Last error: {ex.Message}");

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

        public static async Task<bool> HasNotionDatabaseChangedAsync()
        {
	        var currentSnapshot = new List<string>(CtiCardNames.Concat(CtiSigilNames).Concat(DmcCardNames).Concat(DmcSigilNames));

	        // Log the current snapshot for debugging
	        Console.WriteLine("Current Snapshot: " + string.Join(", ", currentSnapshot));
	        Console.WriteLine("Previous Snapshot: " + string.Join(", ", previousSnapshot));

	        if (!previousSnapshot.SequenceEqual(currentSnapshot))
	        {
		        previousSnapshot = new List<string>(currentSnapshot);
        
		        // Log if change is detected
		        Console.WriteLine("Changes detected in Notion database!");
		        return true;
	        }

	        // Log if no changes are detected
	        Console.WriteLine("No changes detected.");
	        return false;
        }
    }
}