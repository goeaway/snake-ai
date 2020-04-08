using Snake.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach (var controller in _controllers)
            {
                controller.CurrentGame.OnGameAltered += OnGameAlteredHandler;
            }
        }

        private void OnGameAlteredHandler(object sender, OnGameAlteredEventArgs e)
        {
            // find all the controllers that didn't trigger this e.Controller
            // make them act on the item being added
            foreach(var controller in _controllers.Where(c => c != e.Controller))
            {
                controller.CurrentGame.ReactToItem(e.Item, e.Position);
            }
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
                        controller.CurrentGame.OnGameAltered -= OnGameAlteredHandler;
                        controller.Reset();
                        controller.CurrentGame.OnGameAltered += OnGameAlteredHandler;
                    }
                }

                PrintControllers();
                Thread.Sleep(REFRESH_RATE_MILLISECONDS);
            }

            return 0;
        }
    }
}
