using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using KamWerksCardIndexCSharp.DiscordBot.Outputs;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

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
											var outputTest = await Test.CTI(formattedcontent, iterator30);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										} else if (formattedcontent[2] == Dicts.Formatting[1])
										{
											var outputFancy = await FANCY.CTI(iterator30, formattedcontent);
											var messageOutput = outputFancy.mess;
											messageOutput.Content = outputFancy.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										}
									}
								}
								else
								{
									DiscordMessageBuilder messageBuilder = new();
									messageBuilder.Content = $"\n {formattedcontent[1]} was not found, try again!";
									await eventArgs.Message.RespondAsync(messageBuilder);
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
											var outputTest = await Test.DMC(formattedcontent, iterator30);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										} else if (formattedcontent[2] == Dicts.Formatting[1])
										{
											var outputTest = await FANCY.DMC(iterator30, formattedcontent);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										}
									}
								}
								else
								{
									DiscordMessageBuilder messageBuilder = new();
									messageBuilder.Content = $"\n {formattedcontent[1]} was not found, try again!";
									await eventArgs.Message.RespondAsync(messageBuilder);
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
									var outputTest = await FANCY.CTI(iterator30, formattedcontent);
									var messageOutput = outputTest.mess;
									messageOutput.Content = outputTest.takeout;
									await eventArgs.Message.RespondAsync(messageOutput);
								}
								else
								{
									DiscordMessageBuilder messageBuilder = new();
									messageBuilder.Content = $"\n {formattedcontent[1]} was not found, try again!";
									await eventArgs.Message.RespondAsync(messageBuilder);
								}
							}
							else if (formattedcontent[0] == "DMC")
							{
								if (NotionEnd.DmcCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.DMC(iterator30, formattedcontent);
									var messageOutput = outputTest.mess;
									messageOutput.Content = outputTest.takeout;
									await eventArgs.Message.RespondAsync(messageOutput);
								}
								else
								{
									DiscordMessageBuilder messageBuilder = new();
									messageBuilder.Content = $"\n {formattedcontent[1]} was not found, try again!";
									await eventArgs.Message.RespondAsync(messageBuilder);
								}
							}
						}
					}
					iterator30++;
				}
			}
			return;
		}
	}
}