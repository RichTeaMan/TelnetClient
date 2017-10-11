using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelnetCtf
{
    class Program
    {
        private static string password = "bestapocalypse";

        private static Regex playerPositionRegex = new Regex(@"You are at position: (|-)\d+\.\d+ , (|-)\d+\.\d+ , (|-)\d+\.\d+");
        private static Regex zombiePositionRegex = new Regex(@"Zombie is at position: (|-)\d+\.\d+ , (|-)\d+\.\d+ , (|-)\d+\.\d+");
        private static Regex playerRotationRegex = new Regex(@"Your body is rotated: [\.\d]+ degrees.");

        static void Main(string[] args)
        {
            RunTelnetSession().Wait();
            Console.ReadLine();
        }

        private static async Task SendCommand(PrimS.Telnet.Client client, string command, params object[] args)
        {
            var formatted = string.Format(command, args);
            Console.WriteLine(formatted);
            await client.WriteLine(formatted + "\n");
            var result = await client.ReadAsync();
            Console.Write(result);
        }

        private static async Task RunTelnetSession()
        {
            string lastPart;
            using (var client = new PrimS.Telnet.Client("skillz.baectf.com", 443, new System.Threading.CancellationToken()))
            {
                var challenge = await client.ReadAsync();
                Console.WriteLine(challenge);
                await client.WriteLine(password + "\n");

                while (true)
                {
                    string line = await client.ReadAsync();
                    lastPart = line;
                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine(line);
                    }
                    if (line.Contains("Enter command:"))
                    {
                        break;
                    }
                }

                await KillZombies(client);

                // level 2
                await KillZombies(client);

                // level 3
                //await KillZombies(client);
                //var r = await client.ReadAsync();
                //Console.WriteLine(r);
            }

        }

        private static async Task KillZombies(PrimS.Telnet.Client client)
        {
            await client.WriteLine("LOOK\n");
            var lookResult = await client.ReadAsync();
            Console.Write(lookResult);
            var playerPositionLine = playerPositionRegex.Match(lookResult).Value;
            var rotationLine = playerRotationRegex.Match(lookResult).Value;
            var rotationStr = new Regex(@"[\d\.]+").Match(rotationLine).Value;
            var rotation = double.Parse(rotationStr);
            var player = new Player()
            {
                Position = new Position(playerPositionLine),
                Angle = rotation
            };

            Console.WriteLine(player);

            var zombieMatches = zombiePositionRegex.Matches(lookResult);
            var zombiePositions = new List<Position>();
            foreach (Match zm in zombieMatches)
            {
                var pos = new Position(zm.Value);
                zombiePositions.Add(pos);
            }

            foreach (var zp in zombiePositions)
            {
                var angle = player.FacePosition(zp);
                await SendCommand(client, "ROTATE {0}", angle);
                await SendCommand(client, "FIRE");

            }
            //await SendCommand(client, "LOOK");
        }
    }
}
