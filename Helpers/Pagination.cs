using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gepeteco.Helpers
{
    public static class PaginationHelper
    {
        public static readonly Dictionary<ulong, List<string>> PaginatedMessage = new();

        public static async Task SendPaginatedMessageAsync(CommandContext ctx, string longText)
        {
            var pages = SplitText(longText, 4000);

            var prevButton = new DiscordButtonComponent(ButtonStyle.Secondary, $"paginate_prev_{ctx.User.Id}", "⬅️ Anterior", disabled: true);
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, $"paginate_next_{ctx.User.Id}", "Próximo ➡️", disabled: pages.Count <= 1);

            var embed = new DiscordEmbedBuilder
            {
                Title = "Resposta",
                Description = pages[0],
                Color = DiscordColor.CornflowerBlue
            };
            
            embed.WithFooter($"Página 1 / {pages.Count}");  

            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embed)
                .AddComponents(prevButton, nextButton);

            var message = await ctx.RespondAsync(messageBuilder);

            PaginatedMessage[message.Id] = pages;

        }

        private static List<string> SplitText(string input, int chunkSize)
        {
            var pages = new List<string>();
            var lines = input.Split('\n');
            var currentPage = "";

            foreach (var line in lines)
            {
                if (currentPage.Length + line.Length + 1 > chunkSize)
                {
                    pages.Add(currentPage.Trim());
                    currentPage = "";
                }
                currentPage += line + "\n";
            }
            pages.Add(currentPage.Trim());
            return pages;
        }
    }

}