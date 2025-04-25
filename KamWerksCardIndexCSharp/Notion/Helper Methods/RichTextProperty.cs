using Notion.Client;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion.Helper_Methods
{
	public class RichTextProperty
	{
		public async static Task<string> GetPropertyAsString(PropertyValue value)
		{
			var valuenew = value as RichTextPropertyValue;

			return valuenew.RichText.FirstOrDefault()?.PlainText;
		}
	}
}