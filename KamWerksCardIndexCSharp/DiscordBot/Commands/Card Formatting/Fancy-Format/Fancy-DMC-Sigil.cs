using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.Notion.FormatStructFetching;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using KamWerksCardIndexCSharp.Notion;

namespace KamWerksCardIndexCSharp.DiscordBot.Commands.Fancy_Format
{
	public class Fancy_DMC_Sigil
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMC(int iterator30, string[] formattedcontent)
		{
			DiscordMessageBuilder messageBuilder = new();
			var logger = LoggerFactory.CreateLogger("console");
			
			var dmcProperties = await DMC_Propeties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Sigil");

			foreach (var value in dmcProperties)
			{
				logger.Info($"FANCY: {value}");
			}
			
			var BaseEmbedURL = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BaseCardEmbedSigils.png";
			var NotionSigilURLBase = "https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Desafts%20Mod%20(CTI)/Sigils/";
			var BlankSigil = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BlankSigil.png";
			var FontURL = "https://github.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/raw/refs/heads/main/heavyweight.ttf";
			
			HttpClient httpClient = new();
			httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
			{
				NoCache = true,
				NoStore = true,
				MustRevalidate = true
			};
			
			var BaseEmbed = await httpClient.GetStreamAsync(BaseEmbedURL);
			Stream Sigil;
			
			try
			{
				string sigilName = dmcProperties[1].TrimStart().Replace(" ", "%20").Replace("\n", "").Replace("’", "'");
				Sigil = await httpClient.GetStreamAsync(NotionSigilURLBase + sigilName + ".png");
			}
			catch (HttpRequestException)
			{
				logger.Info("This sigil has no Icon");
				Sigil = await httpClient.GetStreamAsync(BlankSigil);
			}
			
			var FontFile = await httpClient.GetStreamAsync(FontURL);
			
			// Load base and font
			Image<Rgba32> image = await Task.Run(() => Image.Load<Rgba32>(BaseEmbed));
			MemoryStream fontStream = new();
			await FontFile.CopyToAsync(fontStream);
			fontStream.Position = 0;
			
			Image<Rgba32> sigil = await Task.Run(() => Image.Load<Rgba32>(Sigil));
			
			FontCollection collection = new();
			FontFamily family = collection.Add(fontStream);
			Font TitleSize = family.CreateFont(50, FontStyle.Regular);
			Font NormalSize = family.CreateFont(30, FontStyle.Regular);
			
			RichTextOptions optionsTitle = new(TitleSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Origin = new PointF(736, 65),
			};

			RichTextOptions optionsNormal = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Origin = new PointF(736, 130),
				WrappingLength = 596,
			};
			
			RichTextOptions optionsSubSubText = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Origin = new PointF(600, 560),
			};

			image.Mutate(img =>
				{
					img.DrawText(optionsTitle, dmcProperties[1], Color.White);
					img.DrawText(optionsNormal, dmcProperties[2], Color.White);
					img.DrawText(optionsSubSubText, dmcProperties[3], Color.White);
					sigil.Mutate(img =>
						{
							img.Invert();
						}
					);
					int SigilHeight = sigil.Height / 2;
					int SigilWidth = sigil.Width / 2;
					
					img.DrawImage(sigil, new Point(298 - SigilWidth, 181 - SigilHeight), 1.0f);
				}
			);

			var stream = new MemoryStream();
			await image.SaveAsync(stream, PngFormat.Instance);
			stream.Position = 0;
			var id = NotionEnd.DmcSigils.GetValueOrDefault(formattedcontent[1]);
			string cleanPageId = id.Replace("-", "");
			var output = $"Provided content for this embed found from; <https://inscryption-pvp-wiki.notion.site/{cleanPageId}>";
			messageBuilder.AddFile($"fancy_{iterator30}.png", stream);
			var takeout = output;
			return (messageBuilder, takeout);
		}
		public async static Task<(MemoryStream image, string takeout)> DMCFromCard(string set, string name, string Token)
		{
			DiscordMessageBuilder messageBuilder = new();
			var logger = LoggerFactory.CreateLogger("console");
			
			var dmcProperties = await DMC_Propeties.FetchPageProperties(name, set, "Sigil");

			foreach (var value in dmcProperties)
			{
				logger.Info($"FANCY: {value}");
			}
			
			var BaseEmbedURL = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BaseCardEmbedSigils.png";
			var NotionSigilURLBase = "https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Desafts%20Mod%20(CTI)/Sigils/";
			var BlankSigil = "https://raw.githubusercontent.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/refs/heads/main/BlankSigil.png";
			var FontURL = "https://github.com/SaxbyMod/KamWerksPortraitsAndOtherAssets/raw/refs/heads/main/heavyweight.ttf";
			
			HttpClient httpClient = new();
			httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
			{
				NoCache = true,
				NoStore = true,
				MustRevalidate = true
			};
			
			var BaseEmbed = await httpClient.GetStreamAsync(BaseEmbedURL);
			Stream Sigil;
			
			try
			{
				string sigilName = dmcProperties[1].TrimStart().Replace(" ", "%20").Replace("\n", "").Replace("’", "'");
				Sigil = await httpClient.GetStreamAsync(NotionSigilURLBase + sigilName + ".png");
			}
			catch (HttpRequestException)
			{
				logger.Info("This sigil has no Icon");
				Sigil = await httpClient.GetStreamAsync(BlankSigil);
			}
			
			var FontFile = await httpClient.GetStreamAsync(FontURL);
			
			// Load base and font
			Image<Rgba32> image = await Task.Run(() => Image.Load<Rgba32>(BaseEmbed));
			MemoryStream fontStream = new();
			await FontFile.CopyToAsync(fontStream);
			fontStream.Position = 0;
			
			Image<Rgba32> sigil = await Task.Run(() => Image.Load<Rgba32>(Sigil));
			
			FontCollection collection = new();
			FontFamily family = collection.Add(fontStream);
			Font TitleSize = family.CreateFont(50, FontStyle.Regular);
			Font NormalSize = family.CreateFont(30, FontStyle.Regular);
			
			RichTextOptions optionsTitle = new(TitleSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Origin = new PointF(736, 65),
			};

			RichTextOptions optionsNormal = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Origin = new PointF(736, 130),
				WrappingLength = 596,
			};
			
			RichTextOptions optionsSubSubText = new(NormalSize)
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Origin = new PointF(600, 560),
			};

			image.Mutate(img =>
				{
					img.DrawText(optionsTitle, dmcProperties[1], Color.White);
					img.DrawText(optionsNormal, dmcProperties[2].Replace("[Token]", Token), Color.White);
					img.DrawText(optionsSubSubText, dmcProperties[3], Color.White);
					sigil.Mutate(img =>
						{
							img.Invert();
						}
					);
					int SigilHeight = sigil.Height / 2;
					int SigilWidth = sigil.Width / 2;
					
					img.DrawImage(sigil, new Point(298 - SigilWidth, 181 - SigilHeight), 1.0f);
				}
			);

			var stream = new MemoryStream();
			await image.SaveAsync(stream, PngFormat.Instance);
			stream.Position = 0;
			var id = NotionEnd.DmcSigils.GetValueOrDefault(name);
			string cleanPageId = id.Replace("-", "");
			var output = $"Provided content for this embed found from; <https://inscryption-pvp-wiki.notion.site/{cleanPageId}>";
			
			var takeout = output;
			return (stream, takeout);
		}
	}
}