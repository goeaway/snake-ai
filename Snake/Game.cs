using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    public class Game
    {
        private readonly Random _randomiser;

        private Direction _lastDirection;

        public bool AllowedOutOfBounds { get; set; }
        public bool Powerups { get; set; }

        public Board Board { get; }
        public SnakeBit Snake { get; private set; }

        public Game(Board board, Direction initialDirection, int? seed = null)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            _randomiser = new Random(seed ?? Environment.TickCount);
            _lastDirection = initialDirection;

            AddFood();

            var (x, y) = board.Bounds; 

            AddSnake(x / 2, y / 2);
        }

        private void AddFood()
        {
            var empty = Board.GetEmpty().ToList();
            var (x, y) = empty[_randomiser.Next(0, empty.Count)];

            Board.Update(x, y, Board.Food);
        }

        private void AddSnake(int x, int y)
        {
            Snake = new SnakeBit(x, y, Snake);
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

            Board.Update(x, y, Board.Snake);
            Board.Update(remX, remY, Board.Empty);
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
                    return _lastDirection == Direction.Right && Snake.Count() > 1 ? _lastDirection : direction;
                case Direction.Up:
                    return _lastDirection == Direction.Down && Snake.Count() > 1 ? _lastDirection : direction;
                case Direction.Right:
                    return _lastDirection == Direction.Left && Snake.Count() > 1 ? _lastDirection : direction;
                case Direction.Down:
                    return _lastDirection == Direction.Up && Snake.Count() > 1 ? _lastDirection : direction;
                default:
                    throw new NotSupportedException(direction.ToString());
            }
        }

        public bool Move(Direction direction)
        {
            direction = AlterDirectionIfRequired(direction);

            // find front of snake,
            var headPos = Snake.GetHeadPosition();

            // look to one space ahead (in the right direction)
            var (nextX, nextY) = GetNextPos(headPos, direction);

            var (xBound, yBound) = Board.Bounds;
            // check if we're now outside of the board. If we are, and we're not allowed to be, return false
            // otherwise update the nextX or Y to be on the other side of the board
            if (nextX < 0 || nextX >= xBound || nextY < 0 || nextY >= yBound)
            {
                if (!AllowedOutOfBounds)
                {
                    // snake died
                    return false;
                }

                // otherwise update the nextX or nextY to go to the other side of the board
                if (nextX < 0)
                {
                    nextX = xBound - 1;
                }
                else if (nextX >= xBound)
                {
                    nextX = 0;
                }
                else if (nextY < 0)
                {
                    nextY = yBound - 1;
                }
                else if (nextY >= yBound)
                {
                    nextY = 0;
                }
            }

            var nextValue = Board.GetValue(nextX, nextY);

            // if that space value is 1, return false (died)
            if (nextValue == Board.Snake)
            {
                return false;
            }

            // get the tail pos so we know where to add the new snake bit to
            var (tailX, tailY) = Snake.Position;

            // move the snake
            UpdateSnake(nextX, nextY);

            // if we've just eaten food, add a bit to the back
            // and replace the food
            if (nextValue == Board.Food)
            {
                AddSnake(tailX, tailY);
            }

            if (Board.GetFoodPosition() == (-1, -1))
            {
                AddFood();
            }

            _lastDirection = direction;
            return true;
        }
    }
}
