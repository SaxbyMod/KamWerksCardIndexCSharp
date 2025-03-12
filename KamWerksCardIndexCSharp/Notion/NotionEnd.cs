using Notion.Client;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;



namespace KamWerksCardIndexCSharp
{
    internal class NotionEnd
    {
        public static List<string> CtiCardNames { get; private set; } = new();
        public static List<string> CtiSigilNames { get; private set; } = new();
        public static Dictionary<string, string> CtiCards { get; private set; } = new();
        public static Dictionary<string, string> CtiSigils { get; private set; } = new();
        public static List<string> DmcCardNames { get; private set; } = new();
        public static List<string> DmcSigilNames { get; private set; } = new();
        public static Dictionary<string, string> DmcCards { get; private set; } = new();
        public static Dictionary<string, string> DmcSigils { get; private set; } = new();

        public static async Task NotionMain(string[] args)
        {
            // Setup Logging
            var logger = LoggerFactory.CreateLogger("console");

            string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            if (string.IsNullOrWhiteSpace(NotionAPIKey))
            {
                logger.Error("Hey, You missed the Notion API Key Environment Var.");
                return;
            }
            logger.Info("Connecting to Notion API...");
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = NotionAPIKey,
            });

            // Fetch all pages from Cards database
            var CtiCardPagesList = await FetchAllPageIds(client, "e19c88aa75b44bfe89321bcde8dc7d9f");
            CtiCards = await FetchPageNamesAndStore(client, CtiCardPagesList, "Card", CtiCardNames);
            logger.Info($"Total Retrieved Cards: {CtiCardPagesList.Count}");

            // Fetch all pages from Sigils database
            var CtiSigilPagesList = await FetchAllPageIds(client, "933d6166cb3f4ee89db51e4cf464f5bd");
            CtiSigils = await FetchPageNamesAndStore(client, CtiSigilPagesList, "Sigil", CtiSigilNames);
            logger.Info($"Total Retrieved Sigils: {CtiSigilPagesList.Count}");
            
            var DmcCardPagesList = await FetchAllPageIds(client, "1229bd3134c34f69a369c5ef830bd7a0");
            DmcCards = await FetchPageNamesAndStore(client, DmcCardPagesList, "Card", DmcCardNames);
            logger.Info($"Total Retrieved Cards: {DmcCardPagesList.Count}");

            // Fetch all pages from Sigils database
            var DmcSigilPagesList = await FetchAllPageIds(client, "ae46b70d77e649d78a94d2fc62a886e0");
            DmcSigils = await FetchPageNamesAndStore(client, DmcSigilPagesList, "Sigil", DmcSigilNames);
            logger.Info($"Total Retrieved Sigils: {DmcSigilPagesList.Count}");

            logger.Info("Notion data retrieval completed successfully.");
        }

        // Fetch all page IDs from a given Notion database
        private static async Task<List<string>> FetchAllPageIds(NotionClient client, string databaseId)
        {
            List<string> pageIds = new List<string>();
            string? nextCursor = null;

            do
            {
                var queryParams = new DatabasesQueryParameters
                {
                    StartCursor = nextCursor,
                    PageSize = 100 // Notion max limit per request
                };

                var response = await client.Databases.QueryAsync(databaseId, queryParams);
                pageIds.AddRange(response.Results.Select(page => page.Id));
                nextCursor = response.HasMore ? response.NextCursor : null;

            } while (!string.IsNullOrEmpty(nextCursor));

            return pageIds;
        }

        // Consolidated method for fetching names and storing them in a list or dictionary
        private static async Task<Dictionary<string, string>> FetchPageNamesAndStore(NotionClient client, List<string> pageIds, string itemType, List<string> nameList)
        {
            var logger = LoggerFactory.CreateLogger("console");
            Dictionary<string, string> pageNameDict = new();

            // Parallelized tasks to speed up fetching
            var tasks = pageIds.Select(async id =>
            {
                var page = await client.Pages.RetrieveAsync(id);
                if (page.Properties.TryGetValue("Internal Name", out PropertyValue nameValue) && nameValue is TitlePropertyValue titleProperty)
                {
                    string nameText = titleProperty.Title.FirstOrDefault()?.PlainText ?? "Unnamed";
                    if (nameList != null) nameList.Add(nameText); // Add to list (if not null)
                    pageNameDict[nameText] = id; // Always add to dictionary
                    logger.Info($"Retrieved {itemType} Name: {nameText} (ID: {id})");
                }
                else
                {
                    logger.Warning($"Failed to retrieve {itemType} Name for ID: {id}");
                }
            }).ToList();

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            return pageNameDict;
        }
    }
}