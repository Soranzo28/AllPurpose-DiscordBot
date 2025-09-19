using DSharpPlus;
using DSharpPlus.CommandsNext;
using DotNetEnv;
using DSharpPlus.EventArgs;
using System.Reflection;
using GenerativeAI;
using Microsoft.VisualBasic;

namespace Gepeteco
{
    public sealed class Program
    {
        private static DiscordClient Client { get; set; } = null!;
        private static CommandsNextExtension Commands { get; set; } = null!;
        private static GoogleAi GenIA { get; set; } = null!;
        public static GenerativeModel Model { get; private set; } = null!;
        static async Task Main()
        {
            Env.Load();
            string token = Env.GetString("DISCORD_TOKEN");
            string prefix = Env.GetString("BOT_PREFIX");
            string ia_token = Env.GetString("GEMINI_TOKEN");
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(ia_token))
            {
                Console.WriteLine("ERROR! Token not found!");
                return;
            }

            GenIA = new GoogleAi(ia_token);
            Model = GenIA.CreateGenerativeModel("models/gemini-1.5-flash");

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { prefix },
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
            };

            Client = new DiscordClient(discordConfig);
            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands(Assembly.GetExecutingAssembly());
            Client.Ready += Client_Ready;

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("Successefully connected!");
            return Task.CompletedTask;
        }
    }
}