using System;
using System.Threading.Tasks;

namespace XPHBot
{
    public class Program
    {
        public static void Main(string[] args) {
            var bot = new XPHBot();
            var t = Task.Run(async () => await bot.MainAsync());
            Console.WriteLine("Console input active!");
            while (bot.IsRunning) {
                var input = Console.ReadLine();
                if (input.Equals("stop")) {
                    Console.WriteLine("Bot will be stopped...");
                    bot.Stop();
                }
            }
        }
    }
}
