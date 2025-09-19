using DotNetEnv;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GenerativeAI;
using GenerativeAI.Types;

namespace Gepeteco.Commands
{
    public class AskCommand : BaseCommandModule
    {
        [Command("ask")]
        public async Task AskIA(CommandContext ctx, [RemainingText] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                await ctx.RespondAsync("O que você deseja pedir à IA?");
                return;
            }

            await ctx.TriggerTypingAsync();
            var thinkingMessage = await ctx.RespondAsync("Guentai, to pensando...");

            try
            {
                string gemini_token = Env.GetString("GEMINI_TOKEN");

                var model = Program.Model;

                var response = await model.GenerateContentAsync(prompt);

                var response_in_text = response.Text();

                if (string.IsNullOrEmpty(response_in_text))
                {
                    await ctx.RespondAsync("A IA não foi capaz de gerar uma resposta em texto!");
                    return;
                }

                if (response_in_text.Length < 1024)
                {
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "Prompt",
                        Description = prompt,
                        Color = DiscordColor.Gold,
                    };

                    embed.AddField("Resposta da IA", response_in_text);
                    embed.WithFooter($"Autor da pergunta: {ctx.User.Username}", ctx.User.AvatarUrl);

                    await thinkingMessage.ModifyAsync(new DiscordMessageBuilder().WithEmbed(embed.Build()));
                }
                else
                {
                    var pages = SplitText(response_in_text, 1950);
                    await thinkingMessage.ModifyAsync(pages.First());

                    foreach (var page in pages.Skip(1))
                    {
                        await ctx.Channel.SendMessageAsync(page);
                    }
                }
            }
            catch (Exception ex)
            {
                await thinkingMessage.ModifyAsync($"A IA não foi capaz de gerar uma resposta em texto! (Detalhes: {ex.Message})");
            }
        }

        private List<string> SplitText(string input, int chunkSize)
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