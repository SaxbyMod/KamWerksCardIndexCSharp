using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace KamWerksCardIndexCSharp
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
			}

			DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(KamWerksID, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);

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
			builder.ConfigureEventHandlers
			(
				Commands => Commands.HandleMessageCreated(async (s, e) => { await CommandInvokerAsync(s, e); })
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
											output += "\ntrue\n```";
											var carddictinaryvalue = NotionEnd.CtiCards.GetValueOrDefault(formattedcontent[1]);
											var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "CTI", "card");
											var textnimages = await FetchText.FetchTexts(pagefetchresults.textBlocks);
											int l = 0;
											foreach (var i in textnimages.textBlocks)
											{
												var j = i.ToString().Replace("\n", "");
												logger.Info(j);
												l += 1;
												var k = l % 2;
												if (k == 0)
												{
													output += j + "\n";
												}
												else
												{
													output += j;
												}
											}
											output += "```";
											logger.Info(textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", ""));
											logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", ""));
											var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png";
											HttpClient httpClient = new HttpClient();
											var file = await httpClient.GetStreamAsync(imageurl);
											messageBuilder.AddFile($"portrait_{iterator30}.png", file);
											logger.Info(imageurl);
										}
									}
								}
								else
								{
									output += "\nfalse";
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
											output += "\ntrue\n```";
											var carddictinaryvalue = NotionEnd.DmcCards.GetValueOrDefault(formattedcontent[1]);
											var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "DMC", "card");
											var textnimages = await FetchText.FetchTexts(pagefetchresults.textBlocks);
											int l = 0;
											foreach (var i in textnimages.textBlocks)
											{
												var j = i.ToString().Replace("\n", "");
												logger.Info(j);
												l += 1;
												var k = l % 2;
												if (k == 0)
												{
													output += j + "\n";
												}
												else
												{
													output += j;
												}
											}
											output += "```";
											logger.Info(textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", ""));
											logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", ""));
											var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png".Replace("'", "").Replace("’", "");
											HttpClient httpClient = new HttpClient();
											var file = await httpClient.GetStreamAsync(imageurl);
											messageBuilder.AddFile($"portrait_{iterator30}.png", file);
											logger.Info(imageurl);
										}
									}
								}
								else
								{
									output += "\nfalse";
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
									output += "\ntrue\n```";
									var carddictinaryvalue = NotionEnd.CtiCards.GetValueOrDefault(formattedcontent[1]);
									var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "CTI", "card");
									var textnimages = await FetchText.FetchTexts(pagefetchresults.textBlocks);
									int l = 0;
									foreach (var i in textnimages.textBlocks)
									{
										var j = i.ToString().Replace("\n", "");
										logger.Info(j);
										l += 1;
										var k = l % 2;
										if (k == 0)
										{
											output += j + "\n";
										}
										else
										{
											output += j;
										}
									}
									output += "```";
									logger.Info(textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", ""));
									logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", ""));
									var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png";
									HttpClient httpClient = new HttpClient();
									var file = await httpClient.GetStreamAsync(imageurl);
									messageBuilder.AddFile($"portrait_{iterator30}.png", file);
									logger.Info(imageurl);
								}
								else
								{
									output += "\nfalse";
								}
							}
							else if (formattedcontent[0] == "DMC")
							{
								if (NotionEnd.DmcCardNames.Contains(formattedcontent[1], StringComparer.OrdinalIgnoreCase))
								{
									output += "\ntrue\n```";
									var carddictinaryvalue = NotionEnd.DmcCards.GetValueOrDefault(formattedcontent[1]);
									var pagefetchresults = await NotionPageFetcher.FetchPageInfo(formattedcontent[1], "DMC", "card");
									var textnimages = await FetchText.FetchTexts(pagefetchresults.textBlocks);
									int l = 0;
									foreach (var i in textnimages.textBlocks)
									{
										var j = i.ToString().Replace("\n", "");
										logger.Info(j);
										l += 1;
										var k = l % 2;
										if (k == 0)
										{
											output += j + "\n";
										}
										else
										{
											output += j;
										}
									}
									output += "```";
									logger.Info(textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", ""));
									logger.Info(textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", ""));
									var imageurl = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/{textnimages.textBlocks[1].TrimStart().Replace(" ", "%20").Replace("\n", "")}/Portraits/{textnimages.textBlocks[3].TrimStart().Replace(" ", "%20").Replace("\n", "")}.png".Replace("'", "").Replace("’", "");
									logger.Info(imageurl);
									HttpClient httpClient = new HttpClient();
									var file = await httpClient.GetStreamAsync(imageurl);
									messageBuilder.AddFile($"portrait_{iterator30}.png", file);
									logger.Info(imageurl);
								}
								else
								{
									output += "\nfalse";
								}
							}
						}
					}
					iterator30++;
				}
				messageBuilder.Content = output;
			}
			await eventArgs.Message.RespondAsync(messageBuilder);
		}
	}
}