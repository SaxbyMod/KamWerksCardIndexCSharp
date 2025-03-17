﻿using Notion.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion
{
    internal class NotionPageFetcher
    {
        // Fetch page info and print blocks (based on the page's name and type)
        public static async Task<(List<string> textBlocks, string val)> FetchPageInfo(string name, string set, string type)
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
            
            string id = "";
            logger.Error($"Fetching information for {type}: {name}...");
            if (set == "CTI")
            {
	            id = NotionEnd.CtiCards.GetValueOrDefault(name);
            }
            if (set == "DMC")
            {
	            id = NotionEnd.DmcCards.GetValueOrDefault(name);
            }
            if (string.IsNullOrEmpty(id))
            {
                logger.Error($"No page ID found for {name}. Please ensure it's mapped correctly.");
                return (new List<string>(), "");
            }

            // Fetch the page content asynchronously
            Page page = await notionClient.Pages.RetrieveAsync(id);

            // Lists to hold text and image data
            var textBlocks = new List<string>();

            // Query blocks related to the page
            var blocks = await notionClient.Blocks.RetrieveChildrenAsync(id); // Query for blocks related to the page.

            // Iterate over the blocks if they exist
            foreach (var block in blocks.Results)
            {
                var newblocks = notionClient.Blocks.RetrieveChildrenAsync(block.Id).Result;
                foreach (var myblock in newblocks.Results)
                {
                    var newblocks2 = notionClient.Blocks.RetrieveChildrenAsync(myblock.Id).Result;
                    foreach (var myblock2 in newblocks2.Results)
                    {
                        if (myblock2.GetType().Name == "ParagraphBlock")
                        {
                            var text = myblock2 as ParagraphBlock;
                            textBlocks.Add(text.Id);
                        }
                    }
                }
            }

            // Return the text blocks and image URLs
            return (textBlocks, "");
        }
    }
}