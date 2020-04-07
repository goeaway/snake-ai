using Snake.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Snake.ItemPickupHandlers;

namespace Snake
{
    public struct OnGameAlteredEventArgs
    {
        public string Item { get; set; }
    }

    public class Game
    {
        public event EventHandler<OnGameAlteredEventArgs> OnGameAltered;

        private readonly Random _randomiser;

        private Direction _currentDirection;
        private IEnumerable<IItemPickupHandler> _pickupHandlers;

        internal IReadOnlyCollection<IItemPickupHandler> PickupHandlers => _pickupHandlers.ToImmutableArray();
        public int Score { get; internal set; }
        public SnakeBit Snake { get; internal set; }
        public Board Board { get; }

        public Game(
            Board board, 
            Direction initialDirection, 
            Random randomiser, 
            IEnumerable<IItemPickupHandler> pickupHandlers, 
            int? seed = null)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            _randomiser = new Random(seed ?? Environment.TickCount);
            _currentDirection = initialDirection;
            _pickupHandlers = pickupHandlers ?? throw new ArgumentNullException(nameof(pickupHandlers)); 

            AddFood();

            var (x, y) = board.Bounds; 

            Snake = new SnakeBit(x / 2, y / 2, Snake);
        }

        private void AddFood()
        {
            var empty = Board.GetEmpty().ToList();

            if (empty.Count > 0)
            {
                var (x, y) = empty[_randomiser.Next(0, empty.Count)];

                Board.Update(x, y, Consts.Items.Food);
            }
        }

        private void UpdateSnake(int x, int y)
        {
            var (remX, remY) = Snake.Position;

            if (Snake.Head == null)
            {
                // if this is the head just update it's pos
                Snake.Position = (x, y);
            }
            else
            {
                // drop the tail, we don't need it
                Snake = Snake.Head;
                Snake.GetHead().Head = new SnakeBit(x, y, null);
            }

            Board.Update(x, y, Consts.Items.Snake);
            Board.Update(remX, remY, Consts.Items.Empty);
        }

        private (int, int) GetNextPos((int, int) currentPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return (currentPosition.Item1 - 1, currentPosition.Item2);
                case Direction.Up:
                    return (currentPosition.Item1, currentPosition.Item2 - 1);
                case Direction.Right:
                    return (currentPosition.Item1 + 1, currentPosition.Item2);
                case Direction.Down:
                    return (currentPosition.Item1, currentPosition.Item2 + 1);
                default:
                    throw new NotSupportedException(direction.ToString());
            }
        }

        private Direction AlterDirectionIfRequired(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return _currentDirection == Direction.Right && Snake.Count() > 1 ? _currentDirection : direction;
                case Direction.Up:
                    return _currentDirection == Direction.Down && Snake.Count() > 1 ? _currentDirection : direction;
                case Direction.Right:
                    return _currentDirection == Direction.Left && Snake.Count() > 1 ? _currentDirection : direction;
                case Direction.Down:
                    return _currentDirection == Direction.Up && Snake.Count() > 1 ? _currentDirection : direction;
                default:
                    throw new NotSupportedException(direction.ToString());
            }
        }

        public bool Move(Direction direction)
        {
            _currentDirection = AlterDirectionIfRequired(direction);

            // find front of snake,
            var headPos = Snake.GetHeadPosition();

            // look to one space ahead (in the right direction)
            var (nextX, nextY) = GetNextPos(headPos, _currentDirection);

            var (xBound, yBound) = Board.Bounds;

            // check if we're now outside of the board. If we are, return false
            if (nextX < 0 || nextX >= xBound || nextY < 0 || nextY >= yBound)
            {
                return false;
            }

            var nextValue = Board.GetValue(nextX, nextY);

            // if that space value is 1, return false (died)
            if (nextValue == Consts.Items.Snake)
            {
                return false;
            }

            // move the snake
            UpdateSnake(nextX, nextY);

            // if we've just eaten food, add a bit to the back
            // and replace the food
            if (nextValue != Consts.Items.Empty)
            {
                var handler = _pickupHandlers.FirstOrDefault(h => h.Item == nextValue);

                if (handler != null && handler.HandleItem(this, (nextX, nextY)))
                {
                    // raise event
                    OnGameAltered(this, new OnGameAlteredEventArgs { Item = nextValue });
                }
            }

            if (Board.GetFoodPosition() == (-1, -1))
            {
                AddFood();
            }

            return true;
        }
    }
}
