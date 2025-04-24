using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using KamWerksCardIndexCSharp.DiscordBot.Outputs;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;

namespace KamWerksCardIndexCSharp.DiscordBot.Commands
{
	public class Admin_Commands
	{
		public static async Task AdminAsyncCommands(DiscordClient client, MessageCreatedEventArgs eventArgs, string[] args)
		{
			var logger = LoggerFactory.CreateLogger("console");

			string message = eventArgs.Message.Content;
			var user = eventArgs.Author.Presence;

			MatchCollection matches = Regex.Matches(message, @"\<\<(.*?)\>\]>");
			List<string> extractedContents = new List<string>();

			foreach (Match match in matches)
			{
				extractedContents.Add(match.Groups[1].Value.Trim()); // Trim extracted content
			}

			if (extractedContents.Count > 0)
			{
				logger.Info("Extracted Contents;");
				var iterator30 = 0;
				foreach (string content in extractedContents)
				{
					if (user.User.Username == "thincreator3483" || user.User.Username == "master_yurpo")
					{
						if (content == "Recache")
						{
							await NotionEnd.NotionMain(args);
						}
					}
				}
			}
		}
	}
}