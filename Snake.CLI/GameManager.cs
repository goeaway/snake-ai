using Snake.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake.CLI
{
    public class GameManager
    {
        private const int REFRESH_RATE_MILLISECONDS = 80;
        private readonly IEnumerable<IController> _controllers;

        public GameManager(IEnumerable<IController> controllers)
        {
            _controllers = controllers ?? throw new ArgumentNullException(nameof(controllers));
        }

        private void PrintControllers()
        {
            Console.SetCursorPosition(0,0);
            foreach (var controller in _controllers)
            {
                Console.ForegroundColor = controller.Color;
                Console.WriteLine(controller.Id);
                Console.WriteLine(controller.CurrentGame.Score.ToString().PadLeft(4, '0'));
                Console.WriteLine(controller.CurrentGame.Board);
            }
        }

        public int Play(CancellationToken cancelToken)
        {
            // while not cancelled and no winner?
            while (!cancelToken.IsCancellationRequested)
            {
                // if a controller's game is over, reset it
                foreach (var controller in _controllers)
                {
                    var result = controller.Act();
                    if (!result)
                    {
                        controller.Reset();
                    }
                }

                PrintControllers();
                Thread.Sleep(REFRESH_RATE_MILLISECONDS);
            }

            return 0;
        }
    }
}
