using Notion.Client;

namespace KamWerksCardIndexCSharp
{
    internal class FetchImageAndParagraph
    {
        public static async Task<(List<string> textBlocks, List<FileObject> imageUrls)> FetchTextandImage(List<string> textids, List<string> imageids)
        {
            var logger = LoggerFactory.CreateLogger("console");
            string NotionAPIKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            if (string.IsNullOrWhiteSpace(NotionAPIKey))
            {
                logger.Error("Hey, You missed the Notion API Key Environment Var.");
                return (new List<string>(), new List<FileObject>());
            }

            var notionClient = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = NotionAPIKey,
            });

            List<string> TEXT = new List<string>();
            List<FileObject> Images = new List<FileObject>();

            foreach (var i in textids)
            {
                var item = notionClient.Blocks.RetrieveAsync(i).Result;
                var gtem = item as ParagraphBlock;
                var jtem = gtem.Paragraph.RichText;
                foreach (var g in jtem)
                {
                    var j = g.PlainText;
                    logger.Info(g.PlainText);
                    TEXT.Add(g.PlainText);
                }
            }
            Console.WriteLine("Text: " + TEXT.Count());
            foreach (var i in imageids)
            {
                var item = notionClient.Blocks.RetrieveAsync(i).Result;
                var gtem = item as ImageBlock;
                var jtem = gtem.Image;
            }
            Console.WriteLine("Images: " + Images.Count());
            return (TEXT, Images);
        }
    }
}
