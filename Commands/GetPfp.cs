using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Gepeteco.Commands
{
    public class GetPfp : BaseCommandModule
    {
        [Command("Getpfp")]
        public async Task Getpfp(CommandContext ctx, DiscordMember? member = null)
        {

            var targetMember = member ?? ctx.Member;

            if (targetMember == null)
            {
                await ctx.RespondAsync("Unable to find a valid member!");
                return;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{targetMember.DisplayName}'s icon",
                ImageUrl = targetMember.AvatarUrl,
                Color = targetMember.Color,
            };

            await ctx.RespondAsync(embed);
        }
    }
}