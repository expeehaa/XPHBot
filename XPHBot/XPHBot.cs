using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace XPHBot
{
    public class XPHBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public bool IsRunning = true;

        public async Task MainAsync() {
            _client = new DiscordSocketClient();
            _client.Log += Logger.Log;
            var cmdconfig = new CommandServiceConfig {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            };
            _commands = new CommandService(cmdconfig);
            await AddModules();
            _client.MessageReceived += CommandProcessing;
            await _client.LoginAsync(TokenType.Bot, "");
            await _client.StartAsync();
            Console.WriteLine($"Bot started on account \"{_client.CurrentUser.Username}\"");

            await Task.Delay(-1);
        }

        private async Task AddModules() {
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            Console.WriteLine($"Services loaded: {_services.GetServices<IServiceProvider>().Aggregate("", (s,isp) => s + isp.GetType().Name + ", ", s => s.Substring(0, s.Length - 2))}");
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            Console.WriteLine($"Available modules: {_commands.Modules.Aggregate("", (s, m) => s + m.Name + ", ", s => s.Substring(0, s.Length - 2))}");
            Console.WriteLine($"Available commands: {_commands.Commands.Aggregate("", (s, c) => s + c.Name + ", ", s => s.Substring(0, s.Length - 2))}");
        }

        private async Task CommandProcessing(SocketMessage sm) {
            if (!(sm is SocketUserMessage sum)) return;
            var argPos = 0;
            if (!sum.HasStringPrefix("!", ref argPos, StringComparison.OrdinalIgnoreCase)) return;
            var context = new SocketCommandContext(_client, sum);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if(!result.IsSuccess) Console.WriteLine("Failed to execute command: " + result.ErrorReason);
            await sum.DeleteAsync();
        }

        public void Stop() {
            _client.StopAsync().GetAwaiter().GetResult();
            _client.LogoutAsync().GetAwaiter().GetResult();
            IsRunning = false;
        }
    }
}