using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Gepeteco.Commands
{
    public class Ping : BaseCommandModule
    {
        [Command("ping")]
        public async Task PingMessage(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong!");
        }
    }
}