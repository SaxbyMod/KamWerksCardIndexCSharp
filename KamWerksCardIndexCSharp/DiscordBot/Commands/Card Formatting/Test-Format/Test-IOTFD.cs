using KamWerksCardIndexCSharp.Notion;
using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;

namespace KamWerksCardIndexCSharp.DiscordBot.Commands.Test_Format
{
    public class Test_IOTFD
    {
        public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFD(string[] formattedcontent, int iterator30)
        {
            var logger = LoggerFactory.CreateLogger("console");
            DiscordMessageBuilder messageBuilder = new();
            var output = "```";
            var carddictinaryvalue = NotionEnd.IotfdCards.GetValueOrDefault(formattedcontent[1]);
            var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "IOTFD", "card");
            var textnimages = await FetchText.FetchTexts(pagefetchresults.textBlocks);
            int l = 0;
            foreach (var i in textnimages.textBlocks)
            {
                var j = i.ToString().Replace("\n", "");
                logger.Info(j);
                l += 1;
                var k = l % 2;
                if (k == 0)
                {
                    output += j + "\n";
                }
                else
                {
                    output += j;
                }
            }
            output += "```";
            logger.Info(textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", ""));
            logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "").Replace("\n", ""));
            var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png".Replace("'", "").Replace("’", "");
            logger.Info(imageurl);
            HttpClient httpClient = new HttpClient();
            var file = await httpClient.GetStreamAsync(imageurl);
            messageBuilder.AddFile($"portrait_{iterator30}.png", file);
            logger.Info(imageurl);
            return (messageBuilder, output);
        }
    }
}