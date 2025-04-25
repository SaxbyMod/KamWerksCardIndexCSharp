using Notion.Client;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion.Helper_Methods
{
	public class MultiSelectProperty
	{
		public async static Task<string> GetPropertyAsString(PropertyValue value)
		{
			var valuenew = value as MultiSelectPropertyValue;
			string multiselect = "";
			
			foreach (var item in valuenew.MultiSelect)
			{
					multiselect = multiselect + $"{item.Name} ";
			}
			return multiselect;
		}
	}
}