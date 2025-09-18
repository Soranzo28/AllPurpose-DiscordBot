using DSharpPlus;
using DSharpPlus.CommandsNext;
using DotNetEnv;
using DSharpPlus.EventArgs;
using System.Reflection;

namespace Gepeteco
{
    public sealed class Program
    {
        private static DiscordClient Client { get; set; } = null!;
        private static CommandsNextExtension Commands { get; set; } = null!;
        static async Task Main()
        {
            Env.Load();
            string token = Env.GetString("DISCORD_TOKEN");
            string prefix = Env.GetString("BOT_PREFIX");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("ERROR! Token not found!");
                return;
            }

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

            Commands.CommandErrored += (sender, eventArgs) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERRO DE COMANDO] O comando '{eventArgs.Command?.Name}' falhou para o usuário '{eventArgs.Context.User.Username}'.");
                Console.WriteLine($"[ERRO DE COMANDO] Exceção: {eventArgs.Exception}");
                Console.ResetColor();
                return Task.CompletedTask;
            };

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