 using System;
using System.Threading;
using CommandLine;

namespace Snake.CLI
{
    class Program
    {
        // hamiltonian cycle
        static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                var board = new Board(40, 20);
                var currentDirection = Direction.Right;

                var game = new Game(board, currentDirection)
                {
                    AllowedOutOfBounds = o.OpenWalls,
                    Powerups = o.Powerups
                };

                var refreshRate = GetRefreshRateForDifficulty(o.Difficulty);

                while (true) 
                {
                    // todo change to bounds of game board
                    Console.SetCursorPosition(0, 0);
                    Console.Write(board);

                    Thread.Sleep(refreshRate);

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;

                        if (key == ConsoleKey.LeftArrow)
                        {
                            currentDirection = Direction.Left;
                        }

                        else if (key == ConsoleKey.UpArrow)
                        {
                            currentDirection = Direction.Up;
                        }

                        else if (key == ConsoleKey.RightArrow)
                        {
                            currentDirection = Direction.Right;
                        }

                        else if (key == ConsoleKey.DownArrow)
                        {
                            currentDirection = Direction.Down;
                        }

                        else if (key == ConsoleKey.P)
                        {
                            Console.SetCursorPosition(22, 7);
                            Console.Write("PAUSED");
                            while (true)
                            {
                                if (Console.KeyAvailable)
                                {
                                    var pauseKey = Console.ReadKey(true).Key;

                                    if (pauseKey == ConsoleKey.P)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(10);
                                }
                            }
                        }
                    }

                    // game over
                    if (!game.Move(currentDirection))
                    {
                        break;
                    }
                }
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
    }
}
