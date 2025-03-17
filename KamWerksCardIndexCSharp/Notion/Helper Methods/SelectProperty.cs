using Notion.Client;
using KamWerksCardIndexCSharp.Helpers;

namespace KamWerksCardIndexCSharp.Notion.Helper_Methods
{
	public class SelectProperty
	{
		public async static Task<string> GetPropertyAsString(PropertyValue value)
		{
			var logger = LoggerFactory.CreateLogger("console");
			string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
			if (string.IsNullOrWhiteSpace(NotionAPIKey))
			{
				logger.Error("Hey, You missed the Notion API Key Environment Var.");
				return "";
			}

			var notionClient = NotionClientFactory.Create(new ClientOptions
			{
				AuthToken = NotionAPIKey,
			});

			var valuenew = value as SelectPropertyValue;

			return valuenew.Select.Name;
		}
	}
}