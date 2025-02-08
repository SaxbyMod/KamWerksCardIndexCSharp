using DSharpPlus;
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
                fetchcard => fetchcard.HandleMessageCreated(async (s, e) =>
                {
                    string message = e.Message.Content;
                    MatchCollection matches = Regex.Matches(message, @"\[\[(.*?)\]\]");
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
                            if (true)
                            {
                                output += $"\n{content}";
                            }
                        }
                        await e.Message.RespondAsync(output);
                    }
                })
            );
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

            await builder.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
