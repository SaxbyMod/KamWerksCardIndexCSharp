using DSharpPlus;
using KamWerksCardIndexCSharp.DiscordBot.Commands;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;
using SixLabors.ImageSharp.PixelFormats;

namespace KamWerksCardIndexCSharp.DiscordBot
{
	internal class DiscordEnd
	{
		private static bool hasNotionRun = false;
		
		public static async Task Main(string[] args)
		{
			await HandleNotionAndDiscordAsync(args);
		}
		
		private static async Task HandleNotionAndDiscordAsync(string[] args)
		{
			string KamWerksID = Environment.GetEnvironmentVariable("TUTOR_TOKEN");
			if (KamWerksID == null)
			{
				Console.WriteLine("Hey, You missed the Kam Werks ID Environment Var.");
			}

			var logger = LoggerFactory.CreateLogger("console");
			
			bool notionChanged = await NotionEnd.HasNotionDatabaseChangedAsync();

			if (!hasNotionRun || notionChanged)
			{
				await NotionEnd.NotionMain(args);
				logger.Info("Running NotionEnd due to first run or Notion DB change...");
				if (!hasNotionRun)
				{
					await Dicts.defineShades(new Rgba32(238, 167, 109, 255), new Rgba32(229, 158, 105, 255), new Rgba32(220, 148, 101, 255), "BasePortrait");
					await Dicts.defineShades(new Rgba32(238, 167, 109, 255), new Rgba32(229, 158, 105, 255), new Rgba32(220, 148, 101, 255), "CTI-Beast-Common");
					await Dicts.defineShades(new Rgba32(246, 169, 92, 255), new Rgba32(242, 151, 99, 255), new Rgba32(238, 130, 114, 255), "CTI-Beast-Rare");
					await Dicts.defineShades(new Rgba32(194, 194, 173, 255), new Rgba32(173, 186, 160, 255), new Rgba32(151, 182, 158, 255), "CTI-Undead-Common");
					await Dicts.defineShades(new Rgba32(203, 195, 135, 255), new Rgba32(169, 194, 135, 255), new Rgba32(127, 190, 140, 255), "CTI-Undead-Rare");
					await Dicts.defineShades(new Rgba32(178, 219, 220, 255), new Rgba32(162, 209, 225, 255), new Rgba32(168, 192, 216, 255), "CTI-Tech-Common");
					await Dicts.defineShades(new Rgba32(150, 225, 216, 255), new Rgba32(149, 206, 233, 255), new Rgba32(157, 183, 246, 255), "CTI-Tech-Rare");
					await Dicts.defineShades(new Rgba32(220, 178, 210, 255), new Rgba32(225, 162, 197, 255), new Rgba32(220, 147, 179, 255), "CTI-Magicks-Common");
					await Dicts.defineShades(new Rgba32(232, 167, 238, 255), new Rgba32(242, 143, 208, 255), new Rgba32(255, 123, 165, 255), "CTI-Magicks-Rare");
					await Dicts.defineShades(new Rgba32(212, 201, 171, 255), new Rgba32(203, 189, 169, 255), new Rgba32(190, 182, 169, 255), "CTI-Extras-Common");
					await Dicts.defineShades(new Rgba32(242, 213, 131, 255), new Rgba32(238, 189, 116, 255), new Rgba32(216, 169, 134, 255), "CTI-Extras-Rare");
				}
				hasNotionRun = true;
			}

			DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(KamWerksID, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);
			
			builder.ConfigureEventHandlers
			(
				Commands => Commands.HandleMessageCreated(
					async (s, e) =>
					{
						await Cards_Command.CardAsyncCommands(s, e);
					}
				)
			);
			
			builder.ConfigureEventHandlers
			(
				Commands => Commands.HandleMessageCreated(
					async (s, e) =>
					{
						await Sigils_Command.SigilAsyncCommands(s, e);
					}
				)
			);
			
			builder.ConfigureEventHandlers
			(
				Commands => Commands.HandleMessageCreated(
					async (s, e) =>
					{
						await Admin_Commands.AdminAsyncCommands(s, e, args);
					}
				)
			);

			await builder.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}