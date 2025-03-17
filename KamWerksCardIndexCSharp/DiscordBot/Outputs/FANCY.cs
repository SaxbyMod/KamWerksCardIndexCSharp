using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.Notion.FormatStructFetching;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using KamWerksCardIndexCSharp.Helpers;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace KamWerksCardIndexCSharp.DiscordBot.Outputs
{
	public class FANCY
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTI(DiscordMessageBuilder messageBuilder, int iterator30, string input, string[] formattedcontent)
		{
			var logger = LoggerFactory.CreateLogger("console");
			var BaseEmbedURL = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BaseCardEmbed.png";
			var PortraitURLBase = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/Formats/CTI/Portraits/";
			var NotionPortraitURLBase = "https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Custom%20TCG%20Inscryption/Portraits/";
			var FontURL = "https://github.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/raw/refs/heads/main/heavyweight.ttf";

			HttpClient httpClient = new();
			var BaseEmbed = await httpClient.GetStreamAsync(BaseEmbedURL);
			Stream Portrait;
			try
			{
				Portrait = await httpClient.GetStreamAsync(PortraitURLBase + formattedcontent[1].TrimStart().Replace(" ", "%20").Replace("\n", "") + ".png");
			}
			catch (HttpRequestException)
			{
				Portrait = await httpClient.GetStreamAsync(NotionPortraitURLBase + formattedcontent[1].TrimStart().Replace(" ", "%20").Replace("\n", "") + ".png");
			}

			var FontFile = await httpClient.GetStreamAsync(FontURL);

			// Load base and font
			Image<Rgba32> image = await Task.Run(() => Image.Load<Rgba32>(BaseEmbed));
			MemoryStream fontStream = new();
			await FontFile.CopyToAsync(fontStream);
			fontStream.Position = 0;

			// Load Portrait
			Image<Rgba32> portraitImage = await Task.Run(() => Image.Load<Rgba32>(Portrait));

			var ctiProperties = await CTI_Properties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Card");

			foreach (var value in ctiProperties)
			{
				logger.Info($"FANCY: {value}");
			}

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

			image.Mutate(img =>
				{
					img.DrawText(optionsTitle, ctiProperties[2], Color.White);
					img.DrawText(optionsNormal, $"{ctiProperties[3]}, {ctiProperties[4]}", Color.White);
					img.DrawText(optionsPower, ctiProperties[6], Color.White);
					img.DrawText(optionsHealth, ctiProperties[7], Color.White);
					img.DrawText(optionsSubText, ctiProperties[8], Color.White);
					img.DrawText(optionsSubSubText, $"{ctiProperties[1]};Artist(s): {ctiProperties[14]}", Color.White);
					portraitImage.Mutate(img =>
						{
							img.Resize(85, 64);

							// Crop to 86x59 (shift +1 right, -1 top, -4 bottom)
							img.Crop(new Rectangle(1, 1, 86, 59));

							// Get color at (0,57) and fill transparency
							Rgba32 fillColor;
							fillColor = portraitImage[0, 57];
							img.BackgroundColor(fillColor);
							// Scale up by 300%
							img.Resize(portraitImage.Width * 3, portraitImage.Height * 3);
						}
					);
					img.DrawImage(portraitImage, new Point(471, 212), 1.0f);
				}
			);

			var stream = new MemoryStream();
			await image.SaveAsync(stream, PngFormat.Instance);
			stream.Position = 0;

			messageBuilder.AddFile($"fancy_{iterator30}.png", stream);
			return (messageBuilder, input);
		}
	}
}