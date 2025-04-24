using KamWerksCardIndexCSharp.Notion;
using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.Notion.FormatStructFetching;

namespace KamWerksCardIndexCSharp.DiscordBot.Outputs.Test_Format
{
	public class Test_DMC_Sigil
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMC(string[] formattedcontent, int iterator30)
		{
			DiscordMessageBuilder messageBuilder = new();
			var logger = LoggerFactory.CreateLogger("console");
			var output = "```yml\n";
			
			var dmcProperties = await DMC_Propeties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Sigil");

			foreach (var value in dmcProperties)
			{
				logger.Info($"TEST: {value}");
			}

			output = output + $"Name: {dmcProperties[1]}\n";
			output = output + $"Description: {dmcProperties[2]}\n";
			output = output + $"Category: {dmcProperties[3]}\n```";
			
			var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Desafts%20Mod%20(CTI)/Sigils/{dmcProperties[0].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png";
			HttpClient httpClient = new HttpClient();
			var file = await httpClient.GetStreamAsync(imageurl);
			messageBuilder.AddFile($"sigil_{iterator30}.png", file);
			logger.Info(imageurl);
			return (messageBuilder, output);
		}
	}
}