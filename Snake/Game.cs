using Snake.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Snake
{
    public struct OnGameAlteredEventArgs
    {
        public char Item { get; set; }
        public (int X, int Y) Position { get; set; }
        public IController Controller { get; set; }
    }

    public struct FiniteItem
    {
        public char Item { get; set; }
        public (int X, int Y) Position { get; set; }
        public int TTL { get; set; }
    }

    public class Game
    {
        public event EventHandler<OnGameAlteredEventArgs> OnGameAltered;

        private readonly Random _randomiser;
        private readonly IController _controller;
        private readonly IList<FiniteItem> _finiteItems;

        private Direction _currentDirection;
        private IEnumerable<IItemPickupHandler> _pickupHandlers;

        public IReadOnlyCollection<char> ItemsUsed => _pickupHandlers.Select(h => h.Item).ToImmutableArray();

        // collection to push pos,value,and ticks
        // every time we move, the ticks for each item is reduced by 1
        // any that have reached zero are checked, if the value is still there
        // we reset to empty and then remove from the collection

        public int Score { get; internal set; }
        public SnakeBit Snake { get; internal set; }
        public Board Board { get; }

        public Game(
            IController controller,
            Board board, 
            Random randomiser, 
            IEnumerable<IItemPickupHandler> pickupHandlers)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _randomiser = randomiser;
            _pickupHandlers = pickupHandlers ?? throw new ArgumentNullException(nameof(pickupHandlers));
            OnGameAltered += (_, e) => { };

            AddFood();

            var (x, y) = board.Bounds; 

            Snake = new SnakeBit(x / 2, y / 2, Snake);

            _finiteItems = new List<FiniteItem>();
        }

        private void AddFood()
        {
            var empty = Board.GetEmpty().ToList();

            if (empty.Count > 0)
            {
                var (x, y) = empty[_randomiser.Next(0, empty.Count)];

                Board.Update(x, y, BoardPiece.Food);
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

            Board.Update(x, y, BoardPiece.Snake);
            Board.Update(remX, remY, BoardPiece.Empty);
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

            // if the value in the next space is not empty nor is it as item that's handled by anything we die
            // (snake or blocker + any others added in future)
            if (nextValue != BoardPiece.Empty && !_pickupHandlers.Select(h => h.Item).Contains(nextValue))
            {
                return false;
            }

            // move the snake
            UpdateSnake(nextX, nextY);

            // if we've just eaten food, add a bit to the back
            // and replace the food
            if (nextValue != BoardPiece.Empty)
            {
                var handler = _pickupHandlers.FirstOrDefault(h => h.Item == nextValue);

                if (handler != null && handler.PickupItem(this, (nextX, nextY), _randomiser, out var item))
                {
                    // raise event
                    OnGameAltered(this, new OnGameAlteredEventArgs { Item = item, Position = (nextX, nextY), Controller = _controller });
                }
            }

            if (Board.GetFoodPosition() == (-1, -1))
            {
                AddFood();
            }

            return true;
        }

        public void ReactToItem(char item, (int X, int Y) pos)
        {
            foreach (var handler in _pickupHandlers)
            {
                if (handler is IItemReactionHandler && handler.Item == item)
                {
                    (handler as IItemReactionHandler).ReactToItem(this, pos, _finiteItems, _randomiser);
                    break;
                }
            }
        }
    }
}
