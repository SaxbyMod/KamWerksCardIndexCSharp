using Notion.Client;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion.Helper_Methods
{
	public class URLProperty
	{
		public async static Task<string> GetPropertyAsString (PropertyValue value)
		{
			var valuenew = value as UrlPropertyValue;

			return valuenew.Url;
		}
	}
}