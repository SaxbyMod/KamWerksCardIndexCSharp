using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using KamWerksCardIndexCSharp.DiscordBot.Outputs;
using KamWerksCardIndexCSharp.DiscordBot.Outputs.Test_Format;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;

namespace KamWerksCardIndexCSharp.DiscordBot.Commands
{
	public class Sigils_Command
	{
		public static async Task SigilAsyncCommands(DiscordClient clitent, MessageCreatedEventArgs eventArgs)
		{
			var logger = LoggerFactory.CreateLogger("console");

			string message = eventArgs.Message.Content;
			MatchCollection matches = Regex.Matches(message, @"\{\{(.*?)\}\}");
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
					string[] formattedcontent = content.Split(";");
					logger.Info(formattedcontent[0].ToString());
					logger.Info(formattedcontent[1].ToString());
					formattedcontent[0] = formattedcontent[0].ToUpper();
					formattedcontent[1] = Capitalization.CapitalizeWithSpaces(formattedcontent[1]);
					if (formattedcontent.Length == 3)
					{
						formattedcontent[2] = formattedcontent[2].ToUpper();
						logger.Info(formattedcontent[2].ToString());
						if (Dicts.SETS.Contains(formattedcontent[0]))
						{
							if (formattedcontent[0] == "CTI")
							{
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.CtiSigilNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									if (Dicts.Formatting.Contains(formattedcontent[2]))
									{
										if (formattedcontent[2] == Dicts.Formatting[0])
										{
											var outputTest = await Test.CTISigil(formattedcontent, iterator30);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										}
										else
										{
											var outputTest = await FANCY.CTISigil(iterator30, formattedcontent);
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
							else if (formattedcontent[0] == "DMC")
							{
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.DmcSigilNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									if (Dicts.Formatting.Contains(formattedcontent[2]))
									{
										if (formattedcontent[2] == Dicts.Formatting[0])
										{
											var outputTest = await Test.DMCSigil(formattedcontent, iterator30);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										}
										else
										{
											var outputTest = await FANCY.DMCSigil(iterator30, formattedcontent);
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
								if (NotionEnd.CtiSigilNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.CTISigil(iterator30, formattedcontent);
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
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.DmcSigilNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.DMCSigil(iterator30, formattedcontent);
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