 using System;
using System.Collections.Generic;
 using System.Threading;
 using CommandLine;
 using Snake.AI;
 using Snake.Players;

namespace Snake.CLI
{
    class Program
    {
        static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                var startDirection = Direction.Right;

                var controllers = new List<IController>();
                var gameFactory = new GameFactory(40, 20, startDirection, 1);

                for (var i = 0; i < o.Players; i++)
                {
                    controllers.Add(new PlayerController(GetDefaultPlayerOptions(i), gameFactory, startDirection));
                }

                for (var i = 0; i < o.AIPlayers; i++)
                {
                    controllers.Add(new AIController(GetDefaultAIOptions(i), gameFactory));
                }

                var gameManager = new GameManager(controllers);

                gameManager.Play(GetRefreshRateForDifficulty(o.Difficulty), CancellationToken.None);
            });

        private static int GetRefreshRateForDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return 100;
                case Difficulty.Medium:
                    return 70;
                case Difficulty.Hard:
                    return 50;
                case Difficulty.Supersonic:
                    return 20;
                default:
                    throw new NotSupportedException(difficulty.ToString());
            }
        }

        private static PlayerOptions GetDefaultPlayerOptions(int index)
        {
            switch (index)
            {
                case 0:
                    return new PlayerOptions
                    {
                        Id = "Player 1",
                        Color = ConsoleColor.Yellow,
                        Up = ConsoleKey.UpArrow,
                        Right = ConsoleKey.RightArrow,
                        Down = ConsoleKey.DownArrow,
                        Left = ConsoleKey.LeftArrow
                    };
                case 1:
                    return new PlayerOptions
                    {
                        Id = "Player 2",
                        Color = ConsoleColor.Cyan,
                        Up = ConsoleKey.W,
                        Right = ConsoleKey.D,
                        Down = ConsoleKey.S,
                        Left = ConsoleKey.A
                    };
                default:
                    throw new NotSupportedException("only 2 players supported");
            }
        }

        private static AIOptions GetDefaultAIOptions(int index)
        {
            switch (index)
            {
                case 0:
                    return new AIOptions
                    {
                        Id = "AI",
                        Color = ConsoleColor.Red,
                    };
                case 1:
                    return new AIOptions
                    {
                        Id = "AI",
                        Color = ConsoleColor.Green,
                    };
                default:
                    throw new NotSupportedException("only 2 players supported");
            }
        }
    }
}
