using Notion.Client;
using Microsoft.Extensions.Logging;

namespace KamWerksCardIndexCSharp
{
    internal class NotionEnd
    {
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
            var CtiCardNames = await FetchPageNames(client, CtiCardPagesList, "Card");
            logger.Info($"Total Retrieved Cards: {CtiCardPagesList.Count}");

            // Fetch all pages from Sigils database
            var CtiSigilPagesList = await FetchAllPageIds(client, "933d6166cb3f4ee89db51e4cf464f5bd");
            var CtiSigilNames = await FetchPageNames(client, CtiSigilPagesList, "Sigil");
            logger.Info($"Total Retrieved Sigils: {CtiSigilPagesList.Count}");

            logger.Info("Notion data retrieval completed successfully.");
        }

        // Fetch all page IDs from a given Notion database
        private static async Task<List<string>> FetchAllPageIds(NotionClient client, string databaseId)
        {
            var logger = LoggerFactory.CreateLogger("console");
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

                logger.Info($"Fetched {response.Results.Count} pages from database {databaseId}, continuing...");

            } while (!string.IsNullOrEmpty(nextCursor));

            return pageIds;
        }

        // Fetch names of pages based on their IDs
        private static async Task<List<string>> FetchPageNames(NotionClient client, List<string> pageIds, string itemType)
        {
            var logger = LoggerFactory.CreateLogger("console");
            List<string> pageNames = new List<string>();

            foreach (var id in pageIds)
            {
                var page = await client.Pages.RetrieveAsync(id);
                if (page.Properties.TryGetValue("Internal Name", out PropertyValue nameValue) && nameValue is TitlePropertyValue titleProperty)
                {
                    string nameText = titleProperty.Title.FirstOrDefault()?.PlainText ?? "Unnamed";
                    pageNames.Add(nameText);
                    logger.Info($"Retrieved {itemType} Name: {nameText}");
                }
                else
                {
                    logger.Warning($"Failed to retrieve {itemType} Name for ID: {id}");
                }
            }

            return pageNames;
        }
    }
}
