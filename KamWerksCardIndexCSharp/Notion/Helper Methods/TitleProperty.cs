using Notion.Client;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion.Helper_Methods
{
	public class TitleProperty
	{
		public async static Task<string> GetPropertyAsString (PropertyValue value)
		{
			var valuenew = value as TitlePropertyValue;

			return valuenew.Title.FirstOrDefault()?.PlainText;
		}
	}
}