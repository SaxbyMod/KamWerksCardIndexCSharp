using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;

namespace KamWerksCardIndexCSharp.DiscordBot.Commands
{
	public class Admin_Commands
	{
		public static async Task AdminAsyncCommands(CommandContext context)
		{
			var logger = LoggerFactory.CreateLogger("console");
			var message = context.Command.ToString();
			MatchCollection matches = Regex.Matches(message, @"(.*?)\>\>");
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
					Console.WriteLine("Content found: " + content);

					if (content == "Recache")
					{
						await NotionEnd.NotionMain();
						var messageOutput = content + "Recache has completed";
						await context.RespondAsync(messageOutput);
					}
				}
				return;
			}
		}
	}
}