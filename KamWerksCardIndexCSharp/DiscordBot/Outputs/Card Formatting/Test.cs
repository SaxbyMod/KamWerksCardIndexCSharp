using KamWerksCardIndexCSharp.Notion;
using KamWerksCardIndexCSharp.Helpers;
using DSharpPlus.Entities;
using KamWerksCardIndexCSharp.DiscordBot.Outputs.Test_Format;

namespace KamWerksCardIndexCSharp.DiscordBot.Outputs
{
	public class Test
	{
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTI (string[] formattedcontent, int iterator30)
		{
			var outputTest = await  Test_CTI.CTI(formattedcontent, iterator30);
			return outputTest;
		}
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMC (string[] formattedcontent, int iterator30)
		{
			var outputTest = await  Test_DMC.DMC(formattedcontent, iterator30);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFD (string[] formattedcontent, int iterator30)
		{
			var outputTest = await  Test_IOTFD.IOTFD(formattedcontent, iterator30);
			return outputTest;
		}
		
		public async static Task<(DiscordMessageBuilder mess, string takeout)> CTISigil (string[] formattedcontent, int iterator30)
		{
			var outputTest = await  Test_CTI_Sigil.CTI(formattedcontent, iterator30);
			return outputTest;
		}
		public async static Task<(DiscordMessageBuilder mess, string takeout)> DMCSigil (string[] formattedcontent, int iterator30)
		{
			var outputTest = await  Test_DMC_Sigil.DMC(formattedcontent, iterator30);
			return outputTest;
		}
		public async static Task<(DiscordMessageBuilder mess, string takeout)> IOTFDSigil (string[] formattedcontent, int iterator30)
		{
			var outputTest = await  Test_IOTFD_Sigil.IOTFD(formattedcontent, iterator30);
			return outputTest;
		}
	}
}