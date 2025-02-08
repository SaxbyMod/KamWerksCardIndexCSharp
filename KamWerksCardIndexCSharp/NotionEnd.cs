using Notion.Client;
using KamWerksCardIndexCSharp;

namespace KamWerksCardIndexCSharp
{
    internal class NotionEnd
    {
        public static async Task NotionMain(string[] args)
        {
            var logger = LoggerFactory.CreateLogger("console");
            string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            if (NotionAPIKey == null)
            {
                Console.WriteLine("Hey, You missed the Notion API Key Environment Var.");
                return;
            }

            logger.Info("Connecting to Notion API...");

            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = NotionAPIKey,
            });

            var queryParams = new DatabasesQueryParameters();
            var CtiCardPages = await client.Databases.QueryAsync("e19c88aa75b44bfe89321bcde8dc7d9f", queryParams);
            logger.Info(CtiCardPages.ToString());
            var CtiSigilPages = await client.Databases.QueryAsync("933d6166cb3f4ee89db51e4cf464f5bd", queryParams);
            logger.Info(CtiSigilPages.ToString());
            logger.Info("Notion data retrieved successfully.");
        }
    }
}
