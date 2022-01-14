using System.Text;
using CSBible;
using dotenv.net;
using DSharpPlus;
using Utils;

DotEnv.Load(new DotEnvOptions(ignoreExceptions: false));

var client = new DiscordClient(new DiscordConfiguration()
{
    Token = DotEnv.Read()["DISCORD_TOKEN"],
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged
});

string PREFIX = DotEnv.Read()["PREFIX"];

client.MessageCreated += async (client, args) =>
{
    if (args.Author.IsCurrent)
    {
        return;
    }

    InputParser inputParser = new(args.Message.Content, PREFIX);

    try
    {
        var biblicalIndex = inputParser.Parse();

        if (biblicalIndex.VerseRange is null)
        {
            var response = Bible.GetVerse(
                biblicalIndex.CSBibleBookName,
                biblicalIndex.Chapter,
                biblicalIndex.Verse
            );

            await client.SendMessageAsync(args.Channel, $"{response} [{biblicalIndex.ToString()}]");
            return;
        }
        else
        {
            StringBuilder sb = new();

            for (int verseNumber = biblicalIndex.Verse; verseNumber <= biblicalIndex.VerseRange; verseNumber++)
            {
                sb.Append(
                    string.Join(string.Empty,
                    verseNumber
                        .ToString()
                        .ToCharArray()
                        .Select(x => SuperscriptNumbers.Get(x - '0'))
                    )
                );

                sb.Append(Bible.GetVerse(
                    biblicalIndex.CSBibleBookName,
                    biblicalIndex.Chapter,
                    verseNumber
                ));

                sb.Append(' ');
            }

            sb.Append($"[{biblicalIndex.ToString()}]");
            await client.SendMessageAsync(args.Channel, sb.ToString());
            return;
        }

    }
    catch (Exception error)
    {
        Console.Error.WriteLine(error.Message);
        Console.Error.WriteLine("\t\t" + error.StackTrace);
        Console.Error.WriteLine("\n");
    }

};

await client.ConnectAsync();
await Task.Delay(-1);
