using Notion.Client;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion.Helper_Methods
{
	public class SelectProperty
	{
		public async static Task<string> GetPropertyAsString(PropertyValue value)
		{
			var valuenew = value as SelectPropertyValue;

			return valuenew.Select.Name;
		}
	}
}