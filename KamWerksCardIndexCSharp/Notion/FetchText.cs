using System;
using System.Collections.Generic;
using Notion.Client;
using System.Linq;
using System.Threading.Tasks;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion
{
    internal class FetchText
    {
        public static async Task<(List<string> textBlocks, string val)> FetchTexts (List<string> textids)
        {
            var logger = LoggerFactory.CreateLogger("console");
            string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            if (string.IsNullOrWhiteSpace(NotionAPIKey))
            {
                logger.Error("Hey, You missed the Notion API Key Environment Var.");
                return (new List<string>(), "");
            }

            var notionClient = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = NotionAPIKey,
            });

            List<string> TEXT = new List<string>();

            foreach (var i in textids)
            {
                var item = notionClient.Blocks.RetrieveAsync(i).Result;
                var gtem = item as ParagraphBlock;
                var jtem = gtem.Paragraph.RichText;
                foreach (var g in jtem)
                {
                    var j = g.PlainText;
                    TEXT.Add(g.PlainText);
                }
            }
            return (TEXT, "");
        }
    }
}
