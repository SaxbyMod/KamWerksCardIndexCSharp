using KamWerksCardIndexCSharp.Notion;
using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.Notion.FormatStructFetching;

namespace KamWerksCardIndexCSharp.DiscordBot.Outputs.Test_Format
{
	public class Test_CTI_Sigil
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTI(string[] formattedcontent, int iterator30)
		{
			DiscordMessageBuilder messageBuilder = new();
			var logger = LoggerFactory.CreateLogger("console");
			var output = "```yml\n";
			
			var ctiProperties = await CTI_Properties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Sigil");

			foreach (var value in ctiProperties)
			{
				logger.Info($"TEST: {value}");
			}

			output = output + $"Name: {ctiProperties[1]}\n";
			output = output + $"Description: {ctiProperties[2]}\n";
			output = output + $"Category: {ctiProperties[3]}\n```";
			
			var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Custom%20TCG%20Inscryption/Sigils/{ctiProperties[0].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png";
			HttpClient httpClient = new HttpClient();
			var file = await httpClient.GetStreamAsync(imageurl);
			messageBuilder.AddFile($"sigil_{iterator30}.png", file);
			logger.Info(imageurl);
			return (messageBuilder, output);
		}
	}
}