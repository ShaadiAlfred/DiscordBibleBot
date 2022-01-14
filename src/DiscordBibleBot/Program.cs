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

client.MessageCreated += async (client, args) =>
{
    if (args.Author.IsCurrent)
    {
        return;
    }

    InputParser inputParser = new(args.Message.Content, DotEnv.Read()["PREFIX"]);

    try
    {
        var p = inputParser.Parse();

        await client.SendMessageAsync(args.Channel, String.Join(' ', inputParser.Parse()));
    }
    catch (Exception error)
    {
        throw error;
        // Console.Error.WriteLine(error.Message);
    }

};

await client.ConnectAsync();
await Task.Delay(-1);
