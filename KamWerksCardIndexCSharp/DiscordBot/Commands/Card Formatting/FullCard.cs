using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.DiscordBot.Commands.FullCard_Format;
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

namespace KamWerksCardIndexCSharp.DiscordBot.Commands
{
	public class FullCard
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTI(int iterator30, string[] formattedcontent, string additionalproperties)
		{
			var outputTest = await FullCard_CTI.CTI(iterator30, formattedcontent, additionalproperties);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMC(int iterator30, string[] formattedcontent, string additionalproperties)
		{
			var outputTest = await FullCard_DMC.DMC(iterator30, formattedcontent, additionalproperties);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFD(int iterator30, string[] formattedcontent, string additionalproperties)
		{
			var outputTest = await FullCard_IOTFD.IOTFD(iterator30, formattedcontent, additionalproperties);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTISigil(int iterator30, string[] formattedcontent)
		{
			var outputTest = await FullCard_CTI_Sigil.CTI(iterator30, formattedcontent);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMCSigil(int iterator30, string[] formattedcontent)
		{
			var outputTest = await FullCard_DMC_Sigil.DMC(iterator30, formattedcontent);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFDSigil(int iterator30, string[] formattedcontent)
		{
			var outputTest = await FullCard_IOTFD_Sigil.IOTFD(iterator30, formattedcontent);
			return outputTest;
		}
	}
}