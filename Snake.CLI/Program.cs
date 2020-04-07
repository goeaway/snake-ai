 using System;
using System.Collections.Generic;
 using System.Linq;
 using System.Net.NetworkInformation;
 using System.Threading;
 using CommandLine;
using Snake.Abstractions;
using Snake.AI;
using Snake.Factories;
using Snake.ItemPickupHandlers;
using Snake.Players;

namespace Snake.CLI
{
    class Program
    {
        static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                var gameFactory = new CountdownGameFactory(40, 20);
                var controllers = GetControllers(o.Players, o.AIPlayers, gameFactory);

                var gameManager = new GameManager(controllers);
                gameManager.Play(CancellationToken.None);
            });

        private static IEnumerable<IController> GetControllers(uint playerCount, uint aiCount, IGameFactory gameFactory)
            => new List<IController>()
                .Concat(Enumerable.Range(0, (int)playerCount).Select(i => new PlayerController(PlayerOptions.GetDefault(i), gameFactory)))
                .Concat(Enumerable.Range(0, (int)aiCount).Select(i => new AIController(AIOptions.GetDefault(i), gameFactory)));
    }
}
