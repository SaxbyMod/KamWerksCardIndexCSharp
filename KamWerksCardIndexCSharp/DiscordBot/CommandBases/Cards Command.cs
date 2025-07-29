using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using KamWerksCardIndexCSharp.DiscordBot.Outputs;
using System.Text.RegularExpressions;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion;

namespace KamWerksCardIndexCSharp.DiscordBot.CommandBases
{
	public class Cards_Command
	{
		public static async Task CardAsyncCommands(DiscordClient client, MessageCreatedEventArgs eventArgs)
		{
			var logger = LoggerFactory.CreateLogger("console");

			string message = eventArgs.Message.Content;
			MatchCollection matches = Regex.Matches(message, @"\[\[(.*?)\]\]");
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
					formattedcontent[1] = Capitalization.CapitalizeWithSpaces(formattedcontent[1]).Replace("'", "’");
					string additionalproperties = "";
					if (formattedcontent.Length >= 3 && !Dicts.ResponseTypes.Contains(formattedcontent[2].ToUpper()))
					{
						formattedcontent[2] = formattedcontent[2].ToUpper();
						if (formattedcontent.Length == 4 && Dicts.ResponseTypes.Contains(formattedcontent[3].ToUpper()))
						{
							formattedcontent[3] = formattedcontent[3].ToUpper();
							if (formattedcontent[3] == "NOEXCESS")
							{
								additionalproperties = "NOEXCESS";
							}
							else if (formattedcontent[3] == "SIGILINCLUSIVE")
							{
								additionalproperties = "SIGILINCLUSIVE";
							}
							else if (formattedcontent[3] == "NOEXCESS-SIGILINCLUSIVE" || formattedcontent[3] == "SIGILINCLUSIVE-NOEXCESS")
							{
								additionalproperties = "NOEXCESS-SIGILINCLUSIVE";
							}
						}
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
											var outputFancy = await FANCY.CTI(iterator30, formattedcontent, additionalproperties);
											var messageOutput = outputFancy.mess;
											messageOutput.Content = outputFancy.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										} else if (formattedcontent[2] == Dicts.Formatting[2])
										{
											var outputFancy = await FullCard.CTI(iterator30, formattedcontent, additionalproperties);
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
											var outputTest = await FANCY.DMC(iterator30, formattedcontent, additionalproperties);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										} else if (formattedcontent[2] == Dicts.Formatting[2])
										{
											var outputTest = await FullCard.DMC(iterator30, formattedcontent, additionalproperties);
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
							else if (formattedcontent[0] == "IOTFD")
							{
								if (NotionEnd.IotfdCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									if (Dicts.Formatting.Contains(formattedcontent[2]))
									{
										if (formattedcontent[2] == Dicts.Formatting[0])
										{
											var outputTest = await Test.IOTFD(formattedcontent, iterator30);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										} else if (formattedcontent[2] == Dicts.Formatting[1])
										{
											var outputTest = await FANCY.IOTFD(iterator30, formattedcontent, additionalproperties);
											var messageOutput = outputTest.mess;
											messageOutput.Content = outputTest.takeout;
											await eventArgs.Message.RespondAsync(messageOutput);
										} else if (formattedcontent[2] == Dicts.Formatting[2])
										{
											var outputTest = await FullCard.IOTFD(iterator30, formattedcontent, additionalproperties);
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
					} else if (formattedcontent.Length == 3 && !Dicts.Formatting.Contains(formattedcontent[2].ToUpper()))
					{
						if (formattedcontent.Length == 3 && Dicts.ResponseTypes.Contains(formattedcontent[2].ToUpper()))
						{
							formattedcontent[2] = formattedcontent[2].ToUpper();
							if (formattedcontent[2] == "NOEXCESS")
							{
								additionalproperties = "NOEXCESS";
							}
							else if (formattedcontent[2] == "SIGILINCLUSIVE")
							{
								additionalproperties = "SIGILINCLUSIVE";
							}
							else if (formattedcontent[2] == "NOEXCESS-SIGILINCLUSIVE" || formattedcontent[2] == "SIGILINCLUSIVE-NOEXCESS")
							{
								additionalproperties = "NOEXCESS-SIGILINCLUSIVE";
							}
						}
						
						if (Dicts.SETS.Contains(formattedcontent[0]))
						{
							if (formattedcontent[0] == "CTI")
							{
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.CtiCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.CTI(iterator30, formattedcontent, additionalproperties);
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
									var outputTest = await FANCY.DMC(iterator30, formattedcontent, additionalproperties);
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
							else if (formattedcontent[0] == "IOTFD")
							{
								if (NotionEnd.IotfdCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.IOTFD(iterator30, formattedcontent, additionalproperties);
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
					else
					{
						if (Dicts.SETS.Contains(formattedcontent[0]))
						{
							if (formattedcontent[0] == "CTI")
							{
								// Compare with trimmed content and case-insensitive
								if (NotionEnd.CtiCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.CTI(iterator30, formattedcontent, additionalproperties);
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
									var outputTest = await FANCY.DMC(iterator30, formattedcontent, additionalproperties);
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
							else if (formattedcontent[0] == "IOTFD")
							{
								if (NotionEnd.IotfdCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									var outputTest = await FANCY.IOTFD(iterator30, formattedcontent, additionalproperties);
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