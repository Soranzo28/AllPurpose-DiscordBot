using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Gepeteco.Commands
{
    public class GetMabPfp : BaseCommandModule
    {
        private const ulong MabID = 206861069724024842;
        [Command("bambam")]
        public async Task GetMabMabPfp(CommandContext ctx)
        {
            try
            {
                DiscordUser Mab = await ctx.Client.GetUserAsync(MabID);

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Rest in piece {Mab.Username}",
                    ImageUrl = Mab.AvatarUrl,
                    Color = DiscordColor.Goldenrod,
                };

                await ctx.RespondAsync(embed);
            }
            catch (Exception)
            {
                await ctx.RespondAsync("Mab encontra-se desaparecido no momento.");
            }
        }
    }
}