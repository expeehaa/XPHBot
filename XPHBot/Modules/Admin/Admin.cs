using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace XPHBot.Modules.Admin
{
    [Group]
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("ibims")]
        [RequireContext(ContextType.Guild)]
        public async Task Ibims() {
            await ReplyAsync($"{Context.User}");
        }

        [Command("embed")]
        [RequireContext(ContextType.Guild)]
        public async Task Embed([Remainder]string text = "Ich mag Züge.") {
            await Context.Channel.SendMessageAsync("", embed: new EmbedBuilder().WithTitle("Test").WithDescription($"```{text}```").WithColor(Color.Blue).Build());
        }
    }
}