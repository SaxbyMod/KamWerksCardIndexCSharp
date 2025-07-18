using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;

namespace KamWerksCardIndexCSharp.DiscordBot.CommandBases
{
	public class Admin_Commands
	{
		public static async Task AdminAsyncCommands(CommandContext context)
		{
			var logger = LoggerFactory.CreateLogger("console");
			var message = context.Command.ToString();

			if (message.Replace("|", "").Contains("Recache"))
			{
				await context.RespondAsync("Running Recache, please wait until the next message is sent.");
				await NotionEnd.NotionMain();
				var messageOutput = "Admin Command: " + "Recache" + " has completed";
				await context.FollowupAsync(messageOutput);
			}
			return;
		}
	}
}