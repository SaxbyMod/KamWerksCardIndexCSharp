using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace KamWerksCardIndexCSharp.Helpers
{
	public class Dicts
	{
		public static List<string> SETS = new List<string>()
		{
			"CTI",
			"DMC"
		};

		public static List<string> Formatting = new List<string>()
		{
			"TEST",
			"FANCY"
		};
		
		public static List<string> ResponseTypes = new List<string>()
		{
			"NOEXCESS",
			"SIGILINCLUSIVE",
			"NOEXCESS-SIGILINCLUSIVE",
			"SIGILINCLUSIVE-NOEXCESS"
		};

		public static Dictionary<string, Shades> TierTempleShades = new Dictionary<string, Shades>();
		public static async Task defineShades (Rgb Light, Rgb Mid, Rgb Dark, string PalleteName)
		{
			Shades Shades = new Shades()
			{
				light = Light,
				mid = Mid,
				dark = Dark,
			};
			TierTempleShades.Add(PalleteName, Shades);
		}
	}
	
	public class Shades{
		public Rgba32 light;
		public Rgba32 mid;
		public Rgba32 dark;
	}
}