using System;
using System.Threading.Tasks;
using Discord;

namespace XPHBot
{
    public static class Logger
    {
        public static Task Log(LogMessage msg) {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}