using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using KamWerksCardIndexCSharp.DiscordBot.Outputs;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;
using SixLabors.ImageSharp.ColorSpaces;

namespace KamWerksCardIndexCSharp.DiscordBot
{
	internal class DiscordEnd
	{
		private static bool hasNotionRun = false;
		public static async Task Main(string[] args)
		{
			string KamWerksID = Environment.GetEnvironmentVariable("TUTOR_TOKEN");
			if (KamWerksID == null)
			{
				Console.WriteLine("Hey, You missed the Kam Werks ID Environment Var.");
				return;
			}

			var logger = LoggerFactory.CreateLogger("console");

			// Run NotionEnd only once on startup
			if (!hasNotionRun)
			{
				hasNotionRun = true;
				logger.Info("Running NotionEnd for the first time...");
				await NotionEnd.NotionMain(args);

				await Dicts.defineShades(new Rgb(238, 167, 109), new Rgb(229, 158, 105), new Rgb(220, 148, 101), "BasePortrait");
				await Dicts.defineShades(new Rgb(238, 167, 109), new Rgb(229, 158, 105), new Rgb(220, 148, 101), "CTI-Beast-Common");
				await Dicts.defineShades(new Rgb(246, 169, 92), new Rgb(242, 151, 99), new Rgb(238, 130, 114), "CTI-Beast-Rare");
				await Dicts.defineShades(new Rgb(194, 194, 173), new Rgb(173, 186, 160), new Rgb(151, 182, 158), "CTI-Undead-Common");
				await Dicts.defineShades(new Rgb(203, 195, 135), new Rgb(169, 194, 135), new Rgb(127, 190, 140), "CTI-Undead-Rare");
				await Dicts.defineShades(new Rgb(178, 219, 220), new Rgb(162, 209, 225), new Rgb(168, 192, 216), "CTI-Tech-Common");
				await Dicts.defineShades(new Rgb(150, 225, 216), new Rgb(149, 206, 233), new Rgb(157, 183, 246), "CTI-Tech-Rare");
				await Dicts.defineShades(new Rgb(220, 178, 210), new Rgb(225, 162, 197), new Rgb(220, 147, 179), "CTI-Magicks-Common");
				await Dicts.defineShades(new Rgb(232, 167, 238), new Rgb(242, 143, 208), new Rgb(255, 123, 165), "CTI-Magicks-Rare");
			}

			DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(KamWerksID, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);
			/*
			builder.ConfigureEventHandlers
			(
				fetchsigil => fetchsigil.HandleMessageCreated(async (s, e) =>
				{
					string message = e.Message.Content;
					MatchCollection matches = Regex.Matches(message, @"\(\((.*?)\)\)");
					List<string> extractedContents = new List<string>();

					foreach (Match match in matches)
					{
						extractedContents.Add(match.Groups[1].Value);
					}

					if (extractedContents.Count > 0)
					{
						string output = "The Outputs are as follows:";
						foreach (string content in extractedContents)
						{
							output += $"\n{content}";
						}
						await e.Message.RespondAsync(output);
					}
				})
			);
			*/
			builder.ConfigureEventHandlers
			(
				Commands => Commands.HandleMessageCreated(
					async (s, e) =>
					{
						await CommandInvokerAsync(s, e);
					}
				)
			);
			await builder.ConnectAsync();
			await Task.Delay(-1);
		}

		private static async Task CommandInvokerAsync(DiscordClient client, MessageCreatedEventArgs eventArgs)
		{
			var logger = LoggerFactory.CreateLogger("console");

			DiscordMessageBuilder messageBuilder = new();

			logger.Info("Building Command Base");

			string message = eventArgs.Message.Content;
			MatchCollection matches = Regex.Matches(message, @"\[\[(.*?)\]\]");
			List<string> extractedContents = new List<string>();

			logger.Info("Regex Checking;");
			foreach (Match match in matches)
			{
				extractedContents.Add(match.Groups[1].Value.Trim()); // Trim extracted content
			}

			if (extractedContents.Count > 0)
			{
				logger.Info("Extracted Contents;");
				string output = "The Outputs are as follows:";
				var iterator30 = 0;
				foreach (string content in extractedContents)
				{
					string[] formattedcontent = content.Split(";");
					logger.Info(formattedcontent[0].ToString());
					logger.Info(formattedcontent[1].ToString());
					if (formattedcontent.Length == 3)
					{
						logger.Info(formattedcontent[2].ToString());
						if (Dicts.SETS.Contains(formattedcontent[0]))
						{
							if (formattedcontent[0] == "CTI")
							{
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.CtiCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									if (Dicts.Formatting.Contains(formattedcontent[2]))
									{
										if (formattedcontent[2] == Dicts.Formatting[0])
										{
											var outputTest = await Test.CTI(formattedcontent, messageBuilder, iterator30, output);
											output = outputTest.takeout;
											messageBuilder = outputTest.mess;
										} else if (formattedcontent[2] == Dicts.Formatting[1])
										{
											var outputFancy = await FANCY.CTI(messageBuilder, iterator30, output, formattedcontent);
											output = outputFancy.takeout;
											messageBuilder = outputFancy.mess;
										}
									}
								}
								else
								{
									output += $"\n {formattedcontent[1]} was not found, try again!";
								}
							}
							else if (formattedcontent[0] == "DMC")
							{
								if (NotionEnd.DmcCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									if (Dicts.Formatting.Contains(formattedcontent[2]))
									{
										if (formattedcontent[2] == Dicts.Formatting[0])
										{
											var outputTest = await Test.DMC(formattedcontent, messageBuilder, iterator30, output);
											output = outputTest.takeout;
											messageBuilder = outputTest.mess;
										} else if (formattedcontent[2] == Dicts.Formatting[1])
										{
											
										}
									}
								}
								else
								{
									output += $"\n {formattedcontent[1]} was not found, try again!";
								}
							}
						}
					}
					else
					{
						if (Dicts.SETS.Contains(formattedcontent[0]))
						{
							if (formattedcontent[0] == "CTI")
							{
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.CtiCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await Test.CTI(formattedcontent, messageBuilder, iterator30, output);
									output = outputTest.takeout;
									messageBuilder = outputTest.mess;
								}
								else
								{
									output += $"\n {formattedcontent[1]} was not found, try again!";
								}
							}
							else if (formattedcontent[0] == "DMC")
							{
								if (NotionEnd.DmcCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await Test.DMC(formattedcontent, messageBuilder, iterator30, output);
									output = outputTest.takeout;
									messageBuilder = outputTest.mess;
								}
								else
								{
									output += $"\n {formattedcontent[1]} was not found, try again!";
								}
							}
						}
					}
					iterator30++;
				}
				messageBuilder.Content = output;
			}
			await eventArgs.Message.RespondAsync(messageBuilder);
			return;
		}
	}
}