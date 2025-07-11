using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.DiscordBot.CommandBases;
using KamWerksCardIndexCSharp.DiscordBot.Commands.Fancy_Format;
using KamWerksCardIndexCSharp.DiscordBot.Commands.Test_Format;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;
using SixLabors.ImageSharp.PixelFormats;

namespace KamWerksCardIndexCSharp.DiscordBot
{
	internal class DiscordEnd
	{
		private static bool hasNotionRun = false;

		public static async Task Main()
		{
			await HandleNotionAndDiscordAsync();
		}

		private static async Task HandleNotionAndDiscordAsync()
		{
			string KamWerksID = Environment.GetEnvironmentVariable("TUTOR_TOKEN");
			if (KamWerksID == null)
			{
				Console.WriteLine("Hey, You missed the Kam Werks ID Environment Var.");
			}

			var logger = LoggerFactory.CreateLogger("console");

			if (!hasNotionRun)
			{
				await NotionEnd.NotionMain();
				logger.Info("Running NotionEnd due to first run or Notion DB change...");
				if (!hasNotionRun)
				{
					await Dicts.defineShades(new Rgba32(238, 167, 109, 255), new Rgba32(229, 158, 105, 255),
						new Rgba32(220, 148, 101, 255), "BasePortrait");
					await Dicts.defineShades(new Rgba32(238, 167, 109, 255), new Rgba32(229, 158, 105, 255),
						new Rgba32(220, 148, 101, 255), "CTI-Beast-Common");
					await Dicts.defineShades(new Rgba32(246, 169, 92, 255), new Rgba32(242, 151, 99, 255),
						new Rgba32(238, 130, 114, 255), "CTI-Beast-Rare");
					await Dicts.defineShades(new Rgba32(194, 194, 173, 255), new Rgba32(173, 186, 160, 255),
						new Rgba32(151, 182, 158, 255), "CTI-Undead-Common");
					await Dicts.defineShades(new Rgba32(203, 195, 135, 255), new Rgba32(169, 194, 135, 255),
						new Rgba32(127, 190, 140, 255), "CTI-Undead-Rare");
					await Dicts.defineShades(new Rgba32(178, 219, 220, 255), new Rgba32(162, 209, 225, 255),
						new Rgba32(168, 192, 216, 255), "CTI-Tech-Common");
					await Dicts.defineShades(new Rgba32(150, 225, 216, 255), new Rgba32(149, 206, 233, 255),
						new Rgba32(157, 183, 246, 255), "CTI-Tech-Rare");
					await Dicts.defineShades(new Rgba32(220, 178, 210, 255), new Rgba32(225, 162, 197, 255),
						new Rgba32(220, 147, 179, 255), "CTI-Magicks-Common");
					await Dicts.defineShades(new Rgba32(232, 167, 238, 255), new Rgba32(242, 143, 208, 255),
						new Rgba32(255, 123, 165, 255), "CTI-Magicks-Rare");
					await Dicts.defineShades(new Rgba32(212, 201, 171, 255), new Rgba32(203, 189, 169, 255),
						new Rgba32(190, 182, 169, 255), "CTI-Extras-Common");
					await Dicts.defineShades(new Rgba32(242, 213, 131, 255), new Rgba32(238, 189, 116, 255),
						new Rgba32(216, 169, 134, 255), "CTI-Extras-Rare");
				}

				hasNotionRun = true;
			}

			DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(KamWerksID,
				DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);

			builder.ConfigureEventHandlers(commands =>
				commands.HandleMessageCreated(async (s, e) =>
				{
					if (e.Author.IsBot || e.Author.Id == s.CurrentUser.Id)
						return;
					await Cards_Command.CardAsyncCommands(s, e);
				})
			);

			builder.ConfigureEventHandlers(commands =>
				commands.HandleMessageCreated(async (s, e) =>
				{
					if (e.Author.IsBot || e.Author.Id == s.CurrentUser.Id)
						return;
					await Sigils_Command.SigilAsyncCommands(s, e);
				})
			);

			builder.UseCommands((IServiceProvider serviceProvider, CommandsExtension extension) =>
			{
				extension.AddCommands([typeof(DiscordEnd)]);
				TextCommandProcessor textCommandProcessor = new(new()
				{
					// The default behavior is that the bot reacts to direct
					// mentions and to the "!" prefix. If you want to change
					// it, you first set if the bot should react to mentions
					// and then you can provide as many prefixes as you want.
					PrefixResolver = new DefaultPrefixResolver(true, "|").ResolvePrefixAsync,
				});

				// Add text commands with a custom prefix (?ping)
				extension.AddProcessor(textCommandProcessor);
			}, new CommandsConfiguration()
			{
				// The default value is true, however it's shown here for clarity
				RegisterDefaultCommandProcessors = true,
			});

			await builder.ConnectAsync();
			await Task.Delay(-1);
		}

		[Command("Recache")]
		public async Task RecacheRolesAsync(CommandContext context)
		{
			IReadOnlyList<DiscordRole> guildRoles = await context.Guild.GetRolesAsync();
			DiscordMember member = await context.Guild.GetMemberAsync(context.User.Id);

			var Member_Roles = member.Roles.ToList();
			var Guild_Roles = guildRoles.ToList();
			var guild_id = context.Guild.Id;

			List<ulong> ApprovedRolesList = new List<ulong>();
			List<DiscordRole> ApprovedRoles = new List<DiscordRole>();
			if (guild_id == 928765504410247269)
			{
				ApprovedRolesList.Add(934185190048276552);
				ApprovedRolesList.Add(934493152369336330);
				ApprovedRolesList.Add(935634664574574612);
				ApprovedRolesList.Add(947695506379907114);
				ApprovedRolesList.Add(938259539906662442);
				ApprovedRolesList.Add(936651233958174780);
				ApprovedRolesList.Add(1353368197059055616);
				ApprovedRolesList.Add(938483990216708166);
			}

			if (guild_id == 1115010083168997376)
			{
				ApprovedRolesList.Add(1115015267798487221);
				ApprovedRolesList.Add(1160015157506879528);
				ApprovedRolesList.Add(1115015427924439051);
				ApprovedRolesList.Add(1341213126091341874);
				ApprovedRolesList.Add(1160316313500139691);
			}

			foreach (var id in ApprovedRolesList)
			{
				var role = Guild_Roles.FirstOrDefault(x => x.Id == id);
				ApprovedRoles.Add(role);
			}

			int TimesApproved = 0;
			for (int i = 0; Member_Roles.Count() > i; i++)
			{
				if (ApprovedRoles.Contains(Member_Roles[i]) && TimesApproved == 0)
				{
					Admin_Commands.AdminAsyncCommands(context);
					TimesApproved++;
				}
			}
		}
		
		public class SetsProvider : IChoiceProvider
		{
			private static readonly IEnumerable<DiscordApplicationCommandOptionChoice> sets =
			[
				new DiscordApplicationCommandOptionChoice("Custom TCG Inscryption", "CTI"),
				new DiscordApplicationCommandOptionChoice("Desaft's Mod (CTI)", "DMC"),
				new DiscordApplicationCommandOptionChoice("Inscryption Overhaul - The Final Duel", "IOTFD"),
			];

			public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) => ValueTask.FromResult(sets);
		}
		
		public class TypeProvider : IChoiceProvider
		{
			private static readonly IEnumerable<DiscordApplicationCommandOptionChoice> types =
			[
				new DiscordApplicationCommandOptionChoice("Fancy", "FANCY"),
				new DiscordApplicationCommandOptionChoice("Debug", "TEST"),
			];

			public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) => ValueTask.FromResult(types);
		}
		
		public class OverloadsProvider : IChoiceProvider
		{
			private static readonly IEnumerable<DiscordApplicationCommandOptionChoice> types =
			[
				new DiscordApplicationCommandOptionChoice("Sigil Inclusive", "SIGILINCLUSIVE"),
				new DiscordApplicationCommandOptionChoice("No Excess", "NOEXCESS"),
				new DiscordApplicationCommandOptionChoice("No Excess & Sigil Inclusive", "NOEXCESS-SIGILINCLUSIVE"),
				new DiscordApplicationCommandOptionChoice("No Overloads", ""),
			];

			public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) => ValueTask.FromResult(types);
		}

		[Command("FetchCard")]
		public async Task FetchCardAsync(CommandContext context, [SlashChoiceProvider<SetsProvider>] string set, string name, [SlashChoiceProvider<TypeProvider>] string type, [SlashChoiceProvider<OverloadsProvider>] string overloads)
		{
			context.RespondAsync("Fetching card: " + name + " from set: " + set + "with overloads: " + overloads + ". Please wait!");
			string[] request = new string[] { "", "", "", "" };
			request[0] = set;
			request[1] = Capitalization.CapitalizeWithSpaces(name);
			request[2] = type;
			request[3] = overloads;
			int iterator30 = 0;
			if (set == "CTI")
			{
				if (type == "FANCY" || string.IsNullOrEmpty(type))
				{
					if (NotionEnd.CtiCardNames.Contains(name))
					{
						var outputTest = await Fancy_CTI.CTI(iterator30, request, overloads.ToUpper());
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}

				if (type == "TEST")
				{
					if (NotionEnd.CtiCardNames.Contains(name))
					{
						var outputTest = await Test_CTI.CTI(request, iterator30);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}
			}
			if (set == "DMC")
			{
				if (type == "FANCY" || string.IsNullOrEmpty(type))
				{
					if (NotionEnd.DmcCardNames.Contains(name))
					{
						var outputTest = await Fancy_DMC.DMC(iterator30, request, overloads.ToUpper());
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}

				if (type == "TEST")
				{
					if (NotionEnd.DmcCardNames.Contains(name))
					{
						var outputTest = await Test_DMC.DMC(request, iterator30);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}
			}
			if (set == "IOTFD")
			{
				if (type == "FANCY" || string.IsNullOrEmpty(type))
				{
					if (NotionEnd.IotfdCardNames.Contains(name))
					{
						var outputTest = await Fancy_IOTFD.IOTFD(iterator30, request, overloads.ToUpper());
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}

				if (type == "TEST")
				{
					if (NotionEnd.IotfdCardNames.Contains(name))
					{
						var outputTest = await Test_IOTFD.IOTFD(request, iterator30);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}
			}
		}
		
		[Command("FetchSigil")]
		public async Task FetchSigilAsync(CommandContext context, [SlashChoiceProvider<SetsProvider>] string set, string name, [SlashChoiceProvider<TypeProvider>] string type)
		{
			context.RespondAsync("Fetching sigil: " + name + " from set: " + set + ". Please wait!");
			string[] request = new string[] { "", "" };
			request[0] = set;
			request[1] = Capitalization.CapitalizeWithSpaces(name);;
			int iterator30 = 0;
			if (set == "CTI")
			{
				if (type == "FANCY" || string.IsNullOrEmpty(type))
				{
					if (NotionEnd.CtiSigilNames.Contains(name))
					{
						var outputTest = await Fancy_CTI_Sigil.CTI(iterator30, request);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}

				if (type == "TEST")
				{
					if (NotionEnd.CtiSigilNames.Contains(name))
					{
						var outputTest = await Test_CTI_Sigil.CTI(request, iterator30);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}
			}
			if (set == "DMC")
			{
				if (type == "FANCY" || string.IsNullOrEmpty(type))
				{
					if (NotionEnd.DmcSigilNames.Contains(name))
					{
						var outputTest = await Fancy_DMC_Sigil.DMC(iterator30, request);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}

				if (type == "TEST")
				{
					if (NotionEnd.DmcSigilNames.Contains(name))
					{
						var outputTest = await Test_DMC_Sigil.DMC(request, iterator30);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}
			}
			if (set == "IOTFD")
			{
				if (type == "FANCY" || string.IsNullOrEmpty(type))
				{
					if (NotionEnd.IotfdSigilNames.Contains(name))
					{
						var outputTest = await Fancy_IOTFD_Sigil.IOTFD(iterator30, request);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}

				if (type == "TEST")
				{
					if (NotionEnd.IotfdSigilNames.Contains(name))
					{
						var outputTest = await Test_IOTFD_Sigil.IOTFD(request, iterator30);
						var messageOutput = outputTest.mess;
						messageOutput.Content = outputTest.takeout;
						await context.FollowupAsync(messageOutput);
					}
				}
			}
		}
	}
}