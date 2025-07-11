using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.Notion.FormatStructFetching;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System.Net;
using System.Numerics;

namespace KamWerksCardIndexCSharp.DiscordBot.Commands.Fancy_Format
{
	public class Fancy_IOTFD
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFD(int iterator30, string[] formattedcontent, string additionalproperties)
		{
			var logger = LoggerFactory.CreateLogger("console");
			DiscordMessageBuilder messageBuilder = new();
			var iotfdProperties = await IOTFD_Propeties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Card");

			foreach (var value in iotfdProperties)
			{
				logger.Info($"FANCY: {value}");
			}

			var BaseEmbedURL = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BaseCardEmbed.png";
			var PortraitURLBase = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/Formats/IOTFD/Fancy%20Formatting/Portraits/";
			var NotionPortraitURLBase = "https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Inscryption%20Overhaul%20-%20Final%20Duel%20Edition/Portraits/";
			var NotionSigilURLBase = "https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Inscryption%20Overhaul%20-%20Final%20Duel%20Edition/Sigils/";
			var BlankSigil = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BlankSigil.png";
			var PortraitBase = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BasePortrait.png";
			var RareStreak = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/RareTerrainStreak.png";
			var FontURL = "https://github.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/raw/refs/heads/main/heavyweight.ttf";
			var NumBase = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/Formats/IOTFD/Fancy%20Formatting/Costs/Numbers/";
			var CostX = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/Formats/IOTFD/Fancy%20Formatting/Costs/X.png";
			var CostBase = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/Formats/IOTFD/Fancy%20Formatting/Costs/";

			HttpClient httpClient = new();
			httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
			{
				NoCache = true,
				NoStore = true,
				MustRevalidate = true
			};
			var BaseEmbed = await httpClient.GetStreamAsync(BaseEmbedURL);

			Stream Portrait;
			Stream Sigil1;
			Stream Sigil2;
			Stream CostNum;
			Stream CostXSymbol;
			Stream CostType;

			try
			{
				Portrait = await httpClient.GetStreamAsync(PortraitURLBase + formattedcontent[1].TrimStart().Replace(" ", "%20").Replace("\n", "").Replace("’", "'") + ".png");
			}
			catch (HttpRequestException)
			{
				Portrait = await httpClient.GetStreamAsync(NotionPortraitURLBase + formattedcontent[1].TrimStart().Replace(" ", "%20").Replace("\n", "").Replace("’", "'") + ".png");
			}

			if (!string.IsNullOrWhiteSpace(iotfdProperties[10]))
			{
				try
				{
					string sigil1Name = iotfdProperties[10].TrimStart().Replace(" ", "%20").Replace("\n", "").Replace("’", "'");
					Sigil1 = await httpClient.GetStreamAsync(NotionSigilURLBase + sigil1Name + ".png");
				}
				catch (HttpRequestException)
				{
					logger.Info("There is no sigil 1 for this card");
					Sigil1 = await httpClient.GetStreamAsync(BlankSigil);
				}
			}
			else
			{
				logger.Info("Skipping request: No sigil 1 data provided.");
				Sigil1 = await httpClient.GetStreamAsync(BlankSigil);
			}

			if (!string.IsNullOrWhiteSpace(iotfdProperties[11]))
			{
				try
				{
					string sigil2Name = iotfdProperties[11].TrimStart().Replace(" ", "%20").Replace("\n", "").Replace("’", "'");
					Sigil2 = await httpClient.GetStreamAsync(NotionSigilURLBase + sigil2Name + ".png");
				}
				catch (HttpRequestException)
				{
					logger.Info("There is no sigil 2 for this card");
					Sigil2 = await httpClient.GetStreamAsync(BlankSigil);
				}
			}
			else
			{
				logger.Info("Skipping request: No sigil 2 data provided.");
				Sigil2 = await httpClient.GetStreamAsync(BlankSigil);
			}

			var costPieces = iotfdProperties[5].Split(", ").ToList();

			List<Image> costs = new List<Image>();

			foreach (var costPiece in costPieces)
			{
				var tempImage = await ProcessCostPiece(costPiece);
				costs.Add(tempImage);
			}

			async Task<Image> ProcessCostPiece(string costPiece)
			{
				var parts = costPiece
					.Split(" ")
					.Where(item => item != "Gem" && item != "Gems") // Exclude "Gem" and "Gems"
					.ToList(); // Convert back to a list after filtering
				Stream Cost;
				var CostStream = await httpClient.GetStreamAsync(BlankSigil);
				Image<Rgba32> CostImage = await Task.Run(() => Image.Load<Rgba32>(CostStream));

				if (parts[0] == "Free")
				{
					CostImage.Mutate(img => { img.Resize(25, 25); });
					return CostImage;
				}

				string costNum = parts[0]; // First part is the number (e.g., "1")
				string costType = "";
				if (parts[1] == "Shattered")
				{
					costType = parts[1] + "%20" + parts[2];
				}
				else
				{
					costType = parts[1]; // Second part is the main type (e.g., "Sapphire")
				}

				// Now fetch the streams
				CostNum = await httpClient.GetStreamAsync($"{NumBase}{costNum}.png");
				CostXSymbol = await httpClient.GetStreamAsync(CostX);
				CostType = await httpClient.GetStreamAsync($"{CostBase}{costType.Replace("Bones", "Bone").Replace("Skulls", "Skull")}.png");
				Image<Rgba32> Num = await Task.Run(() => Image.Load<Rgba32>(CostNum));
				Image<Rgba32> X = await Task.Run(() => Image.Load<Rgba32>(CostXSymbol));
				Image<Rgba32> Type = await Task.Run(() => Image.Load<Rgba32>(CostType));

				CostImage.Mutate(img =>
					{
						if (costType == "Energy" || costType == "Blood" || costType == "Sapphire" || costType == "Ruby" || costType == "Emerald" || costType == "Topaz" || costType == "Amethyst" || costType == "Garnet" || costType.Contains("Shattered") || costNum == "3" || costNum == "2" || costNum == "1")
						{
							int num = 0;
							int.TryParse(costNum, out num);
							img.Resize((Type.Width * num)-((num-1)*5), Type.Height);
							for (int i = 0; num > i; i++)
							{
								if (i == 0)
								{
									img.DrawImage(Type, new Point((Type.Width * i), (img.GetCurrentSize().Height - Type.Height) / 2), 1.0f);	
								}
								else
								{
									img.DrawImage(Type, new Point((Type.Width * i) - (5*i), (img.GetCurrentSize().Height - Type.Height) / 2), 1.0f);
								}
							}
						}
						else
						{
							int RoundToNearestFive(int value)
							{
								return (int)(Math.Round(value / 10.0) * 10);
							}
							
							img.Resize(Num.Width + X.Width + Type.Width -10, Math.Max(Num.Height, Math.Max(X.Height, Type.Height)));
							int xOffset = 0;
							img.DrawImage(Num, new Point(xOffset, RoundToNearestFive((img.GetCurrentSize().Height - Num.Height) / 2)), 1.0f);
							xOffset += Num.Width-5;
							img.DrawImage(X, new Point(xOffset, RoundToNearestFive((img.GetCurrentSize().Height - X.Height) / 2)), 1.0f);
							xOffset += X.Width-5;
							img.DrawImage(Type, new Point(xOffset, RoundToNearestFive((img.GetCurrentSize().Height - Type.Height) / 2)), 1.0f);
						}
					}
				);

				return CostImage;
			}

			var PortraitBaseStream = await httpClient.GetStreamAsync(PortraitBase);
			var rareStreakStream = await httpClient.GetStreamAsync(RareStreak);
			var FontFile = await httpClient.GetStreamAsync(FontURL);

			// Load base and font
			Image<Rgba32> image = await Task.Run(() => Image.Load<Rgba32>(BaseEmbed));
			MemoryStream fontStream = new();
			await FontFile.CopyToAsync(fontStream);
			fontStream.Position = 0;

			// Load Portrait
			Image<Rgba32> portraitImage = await Task.Run(() => Image.Load<Rgba32>(Portrait));
			Image<Rgba32> portraitBase = await Task.Run(() => Image.Load<Rgba32>(PortraitBaseStream));
			Image<Rgba32> rareStreak = await Task.Run(() => Image.Load<Rgba32>(rareStreakStream));
			Image<Rgba32> sigil1 = await Task.Run(() => Image.Load<Rgba32>(Sigil1));
			Image<Rgba32> sigil2 = await Task.Run(() => Image.Load<Rgba32>(Sigil2));

			FontCollection collection = new();
			FontFamily family = collection.Add(fontStream);
			Font TitleSize = family.CreateFont(50, FontStyle.Regular);
			Font NormalSize = family.CreateFont(30, FontStyle.Regular);

			RichTextOptions optionsTitle = new(TitleSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				Origin = new PointF(600, 50),
			};

			RichTextOptions optionsNormal = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				Origin = new PointF(600, 100),
			};

			RichTextOptions optionsSubText = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				Origin = new PointF(600, 465),
			};
			RichTextOptions optionsSubSubText = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				Origin = new PointF(600, 560),
			};

			RichTextOptions optionsPower = new(TitleSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				Origin = new PointF(227, 524),
			};

			RichTextOptions optionsHealth = new(TitleSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				TextAlignment = TextAlignment.Center,
				Origin = new PointF(974, 520),
			};

			Stream Cost;
			var CostStream = await httpClient.GetStreamAsync(BlankSigil);
			Image costImage = Image.Load(CostStream);

			image.Mutate(img =>
				{
					img.DrawText(optionsTitle, iotfdProperties[2], Color.White);
					img.DrawText(optionsNormal, $"{iotfdProperties[3]}, {iotfdProperties[4]}", Color.White);
					img.DrawText(optionsPower, iotfdProperties[6], Color.White);
					img.DrawText(optionsHealth, iotfdProperties[7], Color.White);
					img.DrawText(optionsSubText, iotfdProperties[8], Color.White);
					if (!string.IsNullOrWhiteSpace(iotfdProperties[14]))
					{
						img.DrawText(optionsSubSubText, $"{iotfdProperties[1]}; Artist(s): {iotfdProperties[14]}", Color.White);
					}
					else
					{
						img.DrawText(optionsSubSubText, $"{iotfdProperties[1]}; Artist(s): N/A", Color.White);
					}
					portraitImage.Mutate(img =>
						{
							if (portraitImage.Width == 860 && portraitImage.Height == 650)
							{
								img.Resize(86, 65, new NearestNeighborResampler());
								img.Crop(new Rectangle(0, 1, 86, 59));
								img.Resize(258, 177, new NearestNeighborResampler());
							}
							else if (portraitImage.Width == 86 && portraitImage.Height == 59)
							{
								// Define the source and target colors
								Rgba32 baseLight = Dicts.TierTempleShades["BasePortrait"].light;
								Rgba32 baseMid = Dicts.TierTempleShades["BasePortrait"].mid;
								Rgba32 baseDark = Dicts.TierTempleShades["BasePortrait"].dark;
								Rgba32 commonLight = Dicts.TierTempleShades[$"CTI-{iotfdProperties[3]}-Common"].light;
								Rgba32 commonMid = Dicts.TierTempleShades[$"CTI-{iotfdProperties[3]}-Common"].mid;
								Rgba32 commonDark = Dicts.TierTempleShades[$"CTI-{iotfdProperties[3]}-Common"].dark;
								Rgba32 rareLight = Dicts.TierTempleShades[$"CTI-{iotfdProperties[3]}-Rare"].light;
								Rgba32 rareMid = Dicts.TierTempleShades[$"CTI-{iotfdProperties[3]}-Rare"].mid;
								Rgba32 rareDark = Dicts.TierTempleShades[$"CTI-{iotfdProperties[3]}-Rare"].dark;
								portraitBase.Mutate(ctx =>
								{
									var imageFrame = portraitBase.Frames.RootFrame; // Get the root frame (ImageFrame<Rgba32>)

									if (iotfdProperties[3] == "Extras" && (iotfdProperties[4] == "Rare" || iotfdProperties[4] == "Talking"))
									{
										ctx.DrawImage(rareStreak, new Point(0, 0), 1.0f);
									}

									for (int y = 0; y < portraitBase.Height; y++)
									{
										Span<Rgba32> pixelRow = imageFrame.DangerousGetPixelRowMemory(y).Span; // Now explicitly in RGBA32 format

										for (int x = 0; x < pixelRow.Length; x++)
										{
											if (pixelRow[x] == baseLight)
											{
												if (iotfdProperties[4] == "Common" || iotfdProperties[4] == "Side-Deck" || iotfdProperties[4] == "Common (Joke Card)")
												{
													pixelRow[x] = commonLight;
												}
												else if (iotfdProperties[4] == "Rare" || iotfdProperties[4] == "Talking")
												{
													pixelRow[x] = rareLight;
												}
											}
											if (pixelRow[x] == baseMid)
											{
												if (iotfdProperties[4] == "Common" || iotfdProperties[4] == "Side-Deck" || iotfdProperties[4] == "Common (Joke Card)")
												{
													pixelRow[x] = commonMid;
												}
												else if (iotfdProperties[4] == "Rare" || iotfdProperties[4] == "Talking")
												{
													pixelRow[x] = rareMid;
												}
											}
											if (pixelRow[x] == baseDark)
											{
												if (iotfdProperties[4] == "Common" || iotfdProperties[4] == "Side-Deck" || iotfdProperties[4] == "Common (Joke Card)")
												{
													pixelRow[x] = commonDark;
												}
												else if (iotfdProperties[4] == "Rare" || iotfdProperties[4] == "Talking")
												{
													pixelRow[x] = rareDark;
												}
											}
										}
									}
								});
							}

							img.DrawImage(portraitBase, PixelColorBlendingMode.Normal, PixelAlphaCompositionMode.DestOver, 1.0f);
							img.Resize(258, 177, new NearestNeighborResampler());
						}
					);
					img.DrawImage(portraitImage, new Point(471, 212), 1.0f);

					sigil1.Mutate(img => { img.Invert(); }
					);
					sigil2.Mutate(img => { img.Invert(); }
					);
					int Sigil1Height = sigil1.Height / 2;
					int Sigil2Height = sigil2.Height / 2;
					int Sigil1Width = sigil1.Width / 2;
					int Sigil2Width = sigil2.Width / 2;
					img.DrawImage(sigil1, new Point(299 - Sigil1Width, 300 - Sigil1Height), 1.0f);
					img.DrawImage(sigil2, new Point(918 - Sigil2Width, 300 - Sigil2Height), 1.0f);
					int costHeight = 0;
					int costWidth = 0;
					int costIteration = 0;
					foreach (var image in costs)
					{
						if (costIteration == 0)
						{
							costHeight = image.Height;
							costWidth = image.Width;
						}
						else
						{
							costHeight = Math.Max(costHeight, image.Height);
							costWidth += image.Width + 5;
						}
						costIteration++;
					}
					costImage.Mutate(img =>
						{
							img.Resize(costWidth, costHeight, new NearestNeighborResampler());
							int XOffset = 0;
							int CostIteration = 0;
							foreach (var image in costs)
							{
								if (costIteration == 0)
								{
									img.DrawImage(image, new Point(XOffset, (costImage.Height - image.Height) / 2), 1.0f);
									XOffset = image.Width + 5;
								}
								else
								{
									img.DrawImage(image, new Point(XOffset, (costImage.Height - image.Height) / 2), 1.0f);
									XOffset += image.Width + 5;
								}
								CostIteration++;
							}
						}
					);
					
					int RoundToNearestFive(int value)
					{
						return (int)(Math.Round(value / 10.0) * 10);
					}
					
					int CostWidth = RoundToNearestFive(costImage.Width / 2);
					int CostHeight = RoundToNearestFive(costImage.Height / 2);

					img.DrawImage(costImage, new Point(598 - CostWidth, 512 - CostHeight), 1.0f);
				}
			);

			var stream = new MemoryStream();
			await image.SaveAsync(stream, PngFormat.Instance);
			stream.Position = 0;
			var output = "";
			if (additionalproperties != "NOEXCESS" && additionalproperties != "NOEXCESS-SIGILINCLUSIVE")
			{
				output = "To fetch the sigils associated do the following: \n";
				if (iotfdProperties[10] != null)
				{
					output += $"{{{{IOTFD;{iotfdProperties[10]};FANCY}}}}\n";
				}

				if (iotfdProperties[11] != null)
				{
					output += $"{{{{IOTFD;{iotfdProperties[11]};FANCY}}}}\n";
				}

				if (iotfdProperties[12] != null)
				{
					output += $"{{{{IOTFD;{iotfdProperties[12]};FANCY}}}}\n";
				}

				if (iotfdProperties[13] != null)
				{
					output += $"{{{{IOTFD;{iotfdProperties[13]};FANCY}}}}\n";
				}

				if (iotfdProperties[9] != null)
				{
					output += $"Token Detected for this card: {iotfdProperties[9]}\n";
				}
			}
			output += $"\nProvided content for this embed found from; <{iotfdProperties[15]}>";
			messageBuilder.AddFile($"fancy_{iterator30}.png", stream);
			iterator30++;
			
			if (additionalproperties == "SIGILINCLUSIVE" || additionalproperties == "NOEXCESS-SIGILINCLUSIVE")
			{
				string token = "";
				if (iotfdProperties[9] != null)
				{
					token = iotfdProperties[9];
				}

				output = output.Replace("this", "this cards");
				
				if (iotfdProperties[10] != null)
				{
					var sigil1Embeded = await Fancy_IOTFD_Sigil.IOTFDFromCard(formattedcontent[2], iotfdProperties[10], token);
					output += "\n" + sigil1Embeded.takeout.Replace("this", "the Sigil 1");
					messageBuilder.AddFile($"fancy_{iterator30}.png", sigil1Embeded.image);
					iterator30++;
				}

				if (iotfdProperties[11] != null)
				{
					var sigil2Embeded = await Fancy_IOTFD_Sigil.IOTFDFromCard(formattedcontent[2], iotfdProperties[11], token);
					output += "\n" + sigil2Embeded.takeout.Replace("this", "the Sigil 2");
					messageBuilder.AddFile($"fancy_{iterator30}.png", sigil2Embeded.image);
					iterator30++;
				}

				if (iotfdProperties[12] != null)
				{
					var sigil3Embeded = await Fancy_IOTFD_Sigil.IOTFDFromCard(formattedcontent[2], iotfdProperties[12], token);
					output += "\n" + sigil3Embeded.takeout.Replace("this", "the Sigil 3");
					messageBuilder.AddFile($"fancy_{iterator30}.png", sigil3Embeded.image);
					iterator30++;
				}

				if (iotfdProperties[13] != null)
				{
					var sigil4Embeded = await Fancy_IOTFD_Sigil.IOTFDFromCard(formattedcontent[2], iotfdProperties[13], token);
					output += "\n" + sigil4Embeded.takeout.Replace("this", "the Sigil 4");
					messageBuilder.AddFile($"fancy_{iterator30}.png", sigil4Embeded.image);
					iterator30++;
				}
			}
			
			var takeout = output;
			return (messageBuilder, takeout);
		}
	}
}