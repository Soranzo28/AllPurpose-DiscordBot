using DotNetEnv;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GenerativeAI;

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

                var genAI = new GoogleAi(gemini_token);
                var model = genAI.CreateGenerativeModel("models/gemini-1.5-flash");

                var response = await model.GenerateContentAsync(prompt);

                var response_in_text = response.Text();

                if (string.IsNullOrEmpty(response_in_text))
                {
                    await ctx.RespondAsync("A IA não foi capaz de gerar uma resposta em texto!");
                    return;
                }

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
            catch (Exception ex)
            {
                await thinkingMessage.ModifyAsync($"A IA não foi capaz de gerar uma resposta em texto! (Detalhes: {ex.Message})");
            }
        }
    }
}