using KamWerksCardIndexCSharp.Notion;
using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;

namespace KamWerksCardIndexCSharp.DiscordBot.Outputs
{
	public class Test
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTI (string[] formattedcontent, DiscordMessageBuilder messageBuilder, int iterator30, string input)
		{
			var logger = LoggerFactory.CreateLogger("console");
			var output = "```";
			var carddictinaryvalue = NotionEnd.CtiCards.GetValueOrDefault(formattedcontent[1]);
			var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "CTI", "card");
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
			logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", ""));
			var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png";
			HttpClient httpClient = new HttpClient();
			var file = await httpClient.GetStreamAsync(imageurl);
			messageBuilder.AddFile($"portrait_{iterator30}.png", file);
			logger.Info(imageurl);
			return (messageBuilder, input + output);
		}
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMC (string[] formattedcontent, DiscordMessageBuilder messageBuilder, int iterator30, string input)
		{
			var logger = LoggerFactory.CreateLogger("console");
			var output = "```";
			var carddictinaryvalue = NotionEnd.DmcCards.GetValueOrDefault(formattedcontent[1]);
			var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "DMC", "card");
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
			logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", ""));
			var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png".Replace("'", "").Replace("’", "");
			logger.Info(imageurl);
			HttpClient httpClient = new HttpClient();
			var file = await httpClient.GetStreamAsync(imageurl);
			messageBuilder.AddFile($"portrait_{iterator30}.png", file);
			logger.Info(imageurl);
			return (messageBuilder, input + output);
		}
	}
}