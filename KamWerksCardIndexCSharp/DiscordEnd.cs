using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Notion.Client;
using System.Text.RegularExpressions;


namespace KamWerksCardIndexCSharp
{
    internal class DiscordEnd
    {
        private static bool hasNotionRun = false;
        public static async Task Main(string[] args)
        {
            string KamWerksID = Environment.GetEnvironmentVariable("TUTOR_TOKEN");
            if (KamWerksID == null)
            {
                Console.WriteLine("Hey, You missed the Kam Werks ID Environment Var.");
                return;
            }

            var logger = LoggerFactory.CreateLogger("console");

            // Run NotionEnd only once on startup
            if (!hasNotionRun)
            {
                hasNotionRun = true;
                logger.Info("Running NotionEnd for the first time...");
                await NotionEnd.NotionMain(args);
            }

            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(KamWerksID, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);

            builder.ConfigureEventHandlers
            (
                fetchsigil => fetchsigil.HandleMessageCreated(async (s, e) =>
                {
                    string message = e.Message.Content;
                    MatchCollection matches = Regex.Matches(message, @"\(\((.*?)\)\)");
                    List<string> extractedContents = new List<string>();

                    foreach (Match match in matches)
                    {
                        extractedContents.Add(match.Groups[1].Value);
                    }

                    if (extractedContents.Count > 0)
                    {
                        string output = "The Outputs are as follows:";
                        foreach (string content in extractedContents)
                        {
                            output += $"\n{content}";
                        }
                        await e.Message.RespondAsync(output);
                    }
                })
            );
            builder.ConfigureEventHandlers
            (
                Commands => Commands.HandleMessageCreated(async (s, e) =>
                    {
                        await e.Message.RespondAsync(await CommandInvokerAsync(s, e));
                    })
            );
            await builder.ConnectAsync();
            await Task.Delay(-1);
        }

        // TODO: Checkout our command library, CommandsNext. It makes this a lot easier!
        private static async Task<string> CommandInvokerAsync(DiscordClient client, MessageCreatedEventArgs eventArgs)
        {
            var logger = LoggerFactory.CreateLogger("console");

            DiscordMessageBuilder messageBuilder = new();

            logger.Info("Building Command Base");

            string message = eventArgs.Message.Content;
            MatchCollection matches = Regex.Matches(message, @"\[\[(.*?)\]\]");
            List<string> extractedContents = new List<string>();

            logger.Info("Regex Checking;");
            foreach (Match match in matches)
            {
                extractedContents.Add(match.Groups[1].Value.Trim()); // Trim extracted content
                logger.Info(match.Groups[1].Value.Trim());
            }

            if (extractedContents.Count > 0)
            {
                logger.Info("Extracted Contents;");
                string output = "The Outputs are as follows:";
                foreach (string content in extractedContents)
                {
                    logger.Info(content);
                    // Compare with trimmed content and case-insensitive
                    if (NotionEnd.CtiCardNames.Contains(content, StringComparer.OrdinalIgnoreCase))
                    {
                        output += "\ntrue\n```";
                        var carddictinaryvalue = NotionEnd.CtiCards.GetValueOrDefault(content);
                        var pagefetchresults = await NotionPageFetcher.FetchPageInfo(content, "card");
                        var textnimages = await FetchImageAndParagraph.FetchTextandImage(pagefetchresults.textBlocks, pagefetchresults.imageUrls);
                        int l = 0;
                        foreach (var i in textnimages.textBlocks)
                        {
                            var j = i.ToString().Replace("\n", "");
                            logger.Info(j);
                            l += 1;
                            var k = l % 2;
                            if (k == 0)
                            {
                                output += j + "\n";
                            } else
                            {
                                output += j;
                            }
                        }
                        output += "```";
                        //foreach (var p in textnimages.imageUrls) {
                        //    messageBuilder.AddFile(p, File.OpenRead(p));
                        //}
                    }
                    else
                    {
                        output += "\nfalse";
                    }
                }
                return message = output;
            }
            else
            {
                return null;
            }
            // Send the message
            await eventArgs.Message.RespondAsync(messageBuilder);
        }
    }
}
