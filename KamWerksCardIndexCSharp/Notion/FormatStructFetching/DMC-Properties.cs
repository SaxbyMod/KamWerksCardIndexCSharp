using Notion.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using KamWerksCardIndexCSharp.Helpers;
using KamWerksCardIndexCSharp.Notion.Helper_Methods;
using MultiSelectProperty = KamWerksCardIndexCSharp.Notion.Helper_Methods.MultiSelectProperty;
using RichTextProperty = KamWerksCardIndexCSharp.Notion.Helper_Methods.RichTextProperty;
using SelectProperty = KamWerksCardIndexCSharp.Notion.Helper_Methods.SelectProperty;
using TitleProperty = KamWerksCardIndexCSharp.Notion.Helper_Methods.TitleProperty;

namespace KamWerksCardIndexCSharp.Notion.FormatStructFetching
{
	public class DMC_Propeties
	{
		public async static Task<List<string>> FetchPageProperties (string name, string set, string type)
		{
			var logger = LoggerFactory.CreateLogger("console");
			string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
			if (string.IsNullOrWhiteSpace(NotionAPIKey))
			{
				logger.Error("Hey, You missed the Notion API Key Environment Var.");
				return new List<string>();
			}

			var notionClient = NotionClientFactory.Create(new ClientOptions
			{
				AuthToken = NotionAPIKey,
			});
            
			string id = "";
			logger.Error($"Fetching information for {type}: {name}...");

			id = NotionEnd.DmcCards.GetValueOrDefault(name);
			if (string.IsNullOrEmpty(id))
			{
				logger.Error($"No page ID found for {name}. Please ensure it's mapped correctly.");
				return new List<string>();
			}

			// Fetch the page content asynchronously
			Page page = await notionClient.Pages.RetrieveAsync(id);

			Dictionary<string, PropertyValue> Properties = new Dictionary<string, PropertyValue>();
			
			// Get Propeties for List
			page.Properties.TryGetValue("Internal Name", out var internalName);
			page.Properties.TryGetValue("From", out var from);
			page.Properties.TryGetValue("Name", out var notionName);
			page.Properties.TryGetValue("Temple", out var temple);
			page.Properties.TryGetValue("Rarity", out var ratity);
			page.Properties.TryGetValue("Tribes", out var tribes);
			page.Properties.TryGetValue("Cost", out var cost);
			page.Properties.TryGetValue("Power", out var power);
			page.Properties.TryGetValue("Health", out var health);
			page.Properties.TryGetValue("Flavor", out var flavor);
			page.Properties.TryGetValue("Token", out var token);
			page.Properties.TryGetValue("Sigil 1", out var sigil1);
			page.Properties.TryGetValue("Sigil 2", out var sigil2);
			page.Properties.TryGetValue("Sigil 3", out var sigil3);
			page.Properties.TryGetValue("Sigil 4", out var sigil4);
			page.Properties.TryGetValue("Artist", out var artist);
			page.Properties.TryGetValue("Wiki-Page", out var wikiPage);
			
			// Add properties to List
			Properties.Add("Internal Name", internalName);
			Properties.Add("From", from);
			Properties.Add("Notion Name", notionName);
			Properties.Add("Temple", temple);
			Properties.Add("Rarity", ratity);
			Properties.Add("Tribes", tribes);
			Properties.Add("Cost", cost);
			Properties.Add("Power", power);
			Properties.Add("Health", health);
			Properties.Add("Flavor", flavor);
			Properties.Add("Token", token);
			Properties.Add("Sigil 1", sigil1);
			Properties.Add("Sigil 2", sigil2);
			Properties.Add("Sigil 3", sigil3);
			Properties.Add("Sigil 4", sigil4);
			Properties.Add("Artist", artist);
			Properties.Add("Wiki-Page", wikiPage);
			
			List<string> properties = new List<string>();

			foreach (var property in Properties)
			{
				if (property.Value.Type == PropertyValueType.Title)
				{
					var titleProperty = await TitleProperty.GetPropertyAsString(property.Value);
					properties.Add(titleProperty);
				}
				if (property.Value.Type == PropertyValueType.RichText)
				{
					var richTextProperty = await RichTextProperty.GetPropertyAsString(property.Value);
					properties.Add(richTextProperty);
				}
				if (property.Value.Type == PropertyValueType.Select)
				{
					var selectProperty = await SelectProperty.GetPropertyAsString(property.Value);
					properties.Add(selectProperty);
				}
				if (property.Value.Type == PropertyValueType.Url)
				{
					var urlProperty = await URLProperty.GetPropertyAsString(property.Value);
					properties.Add(urlProperty);
				}
				if (property.Value.Type == PropertyValueType.MultiSelect)
				{
					var multiselectProperty = await MultiSelectProperty.GetPropertyAsString(property.Value);
					properties.Add(multiselectProperty);
				}
			}
			
			return properties;
		}
	}
}
