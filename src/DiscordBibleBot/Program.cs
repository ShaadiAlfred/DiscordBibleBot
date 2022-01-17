using BibleVersions;
using dotenv.net;
using DSharpPlus;
using Interfaces;
using Utils;

DotEnv.Load();

CancellationTokenSource cts = new();

Console.CancelKeyPress += (sender, args) =>
{
    args.Cancel = true;
    cts.Cancel();
};

var client = new DiscordClient(new DiscordConfiguration()
{
    Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged
});

string PREFIX = Environment.GetEnvironmentVariable("PREFIX") ?? string.Empty;

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

        IBibleVersion<CSBible.Book> cSBibleKJV = new CSBibleKJV();

        string passage = cSBibleKJV.GetPassage(biblicalIndex);

        await client.SendMessageAsync(args.Channel, passage);
    }
    catch (Exception error)
    {
        Console.Error.WriteLine($"Error message:\t{error.Message}\nError type:\t{error.GetType()}");
        Console.Error.WriteLine($"Stack trace:\t{error.StackTrace}");
        Console.Error.WriteLine("---------------------------------");
        return;
    }
};

await client.ConnectAsync();

while (!cts.IsCancellationRequested)
    await Task.Delay(100);

cts.Dispose();

client.Dispose();