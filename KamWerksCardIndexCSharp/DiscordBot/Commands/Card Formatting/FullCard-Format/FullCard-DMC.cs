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

namespace KamWerksCardIndexCSharp.DiscordBot.Commands.FullCard_Format
{
	public class FullCard_DMC
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMC(int iterator30, string[] formattedcontent, string additionalproperties)
		{
			var logger = LoggerFactory.CreateLogger("console");
			DiscordMessageBuilder messageBuilder = new();
			var dmcProperties = await DMC_Propeties.FetchPageProperties(formattedcontent[1], formattedcontent[0], "Card");

			foreach (var value in dmcProperties)
			{
				logger.Info($"FullCard: {value}");
			}

			var CardURL = $"https://raw.githubusercontent.com/SaxbyMod/NotionAssets/refs/heads/main/Formats/Desafts%20Mod%20(CTI)/{Capitalization.CapitalizeWithSpaces(dmcProperties[3]).Replace(" ", "%20")}/{Capitalization.CapitalizeWithSpaces(dmcProperties[2]).Replace(" ", "")}.png";

			HttpClient httpClient = new();
			httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
			{
				NoCache = true,
				NoStore = true,
				MustRevalidate = true
			};
			logger.Info(CardURL);
			var Card = await httpClient.GetStreamAsync(CardURL);
			var output = $"Full Card Output for {formattedcontent[1]}; ";
			messageBuilder.AddFile($"fullcard_{iterator30}.png", Card);
			iterator30++;
			
			var takeout = output;
			logger.Info(CardURL);
			return (messageBuilder, takeout);
		}
	}
}