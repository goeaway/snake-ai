using Snake.Abstractions;
using System;

namespace Snake.Players
{
    public class PlayerController : IController
    {
        private Direction _direction;

        private readonly ConsoleKey _up;
        private readonly ConsoleKey _right;
        private readonly ConsoleKey _down;
        private readonly ConsoleKey _left;
        private readonly IGameFactory _gameFactory;

        public event EventHandler<OnGameAlteredEventArgs> OnGameAltered;

        public string Id { get; }

        public Game CurrentGame { get; private set; }

        public ConsoleColor Color { get; }

        public PlayerController(
            PlayerOptions options,
            IGameFactory gameFactory,
            Direction startDirection)
        {
            Id = options.Id;
            Color = options.Color;
            _up = options.Up;
            _right = options.Right;
            _down = options.Down;
            _left = options.Left;
            _direction = startDirection;
            _gameFactory = gameFactory;

            Reset();
        }

        public bool Act()
        {
            // if key available, make action, otherwise use the same direction as last time
            if (Console.KeyAvailable)
            {
                var pressed = Console.ReadKey(true).Key;

                if (pressed == _up)
                {
                    _direction = Direction.Up;
                }
                else if (pressed == _right)
                {
                    _direction = Direction.Right;
                }
                else if (pressed == _down)
                {
                    _direction = Direction.Down;
                }
                else if (pressed == _left)
                {
                    _direction = Direction.Left;
                }
            }

            return CurrentGame.Move(_direction);
        }

        public void Reset()
        {
            CurrentGame = _gameFactory.CreateGame();
        }
    }
}
