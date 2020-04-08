using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommandLine;
using Snake.Abstractions;
using Snake.AI;
using Snake.Factories;
using Snake.Players;

namespace Snake.CLI
{
    class Program
    {
        static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                var gameFactory = new CountdownGameFactory(40, 20);
                var router = new Router();
                var controllers = GetControllers(o.Players, o.AIPlayers, gameFactory, router);

                var gameManager = new GameManager(controllers);
                gameManager.Play(CancellationToken.None);
            });

        private static IEnumerable<IController> GetControllers(uint playerCount, uint aiCount, IGameFactory gameFactory,
            IRouter router)
        {
            var result = new List<IController>();

            for (var i = 0; i < playerCount; i++)
            {
                result.Add(new PlayerController(PlayerOptions.GetDefault(i), gameFactory));
            }

            for (var i = 0; i < aiCount; i++)
            {
                result.Add(new AIController(AIOptions.GetDefault(i), gameFactory, router));
            }

            return result;
        } 
    }
}
