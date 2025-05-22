using KamWerksCardIndexCSharp.Notion;
using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.Notion.FormatStructFetching;

namespace KamWerksCardIndexCSharp.DiscordBot.Outputs.Test_Format
{
    public class Test_IOTFD_Sigil
    {
        public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFD(string[] formattedcontent, int iterator30)
        {
            DiscordMessageBuilder messageBuilder = new();
            var logger = LoggerFactory.CreateLogger("console");
            var output = "```yml\n";
			
            var iotfdProperties = await IOTFD_Propeties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Sigil");

            foreach (var value in iotfdProperties)
            {
                logger.Info($"TEST: {value}");
            }

            output = output + $"Name: {iotfdProperties[1]}\n";
            output = output + $"Description: {iotfdProperties[2]}\n";
            output = output + $"Category: {iotfdProperties[3]}\n```";
			
            var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Desafts%20Mod%20(CTI)/Sigils/{iotfdProperties[0].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png";
            HttpClient httpClient = new HttpClient();
            var file = await httpClient.GetStreamAsync(imageurl);
            messageBuilder.AddFile($"sigil_{iterator30}.png", file);
            logger.Info(imageurl);
            return (messageBuilder, output);
        }
    }
}