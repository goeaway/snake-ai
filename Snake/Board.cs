using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    public class Board
    {
        private readonly char[,] _board;

        public (int X, int Y) Bounds { get; }

        public int Count => Bounds.X * Bounds.Y;

        public Board(int xSize, int ySize)
        {
            Bounds = (xSize, ySize);
            _board = new char[ySize,xSize];

            for (var yDim = 0; yDim < ySize; yDim++)
            {
                for (var xDim = 0; xDim < xSize; xDim++)
                {
                    _board[yDim, xDim] = BoardPiece.Empty;
                }
            }
        }

        public void Update(int x, int y, char value)
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Item2; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.Item1; xDim++)
                {
                    if (xDim == x && yDim == y)
                    {
                        _board[yDim, xDim] = value;
                    }
                }
            }
        }

        public void Remove(char value)
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Y; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.X; xDim++)
                {
                    if (_board[yDim, xDim] == value)
                    {
                        _board[yDim, xDim] = BoardPiece.Empty;
                    }
                }
            }
        }

        public char GetValue(int x, int y)
        {
            var (xBound, yBound) = Bounds;
            if (x < 0 || x >= xBound)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= yBound)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            for (var yDim = 0; yDim < Bounds.Y; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.X; xDim++)
                {
                    if (yDim == y && xDim == x)
                    {
                        return _board[yDim, xDim];
                    }
                }
            }

            return BoardPiece.Empty;
        }

        public (int X, int Y) GetFoodPosition()
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Y; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.X; xDim++)
                {
                    if (_board[yDim, xDim] == BoardPiece.Food)
                    {
                        return (xDim, yDim);
                    }
                }
            }

            return (-1, -1);
        }

        public IEnumerable<(int, int)> GetEmpty()
        {
            return GetEmpty(false);
        }

        public IEnumerable<(int, int)> GetEmpty(bool foodCountsAsEmpty)
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Y; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.X; xDim++)
                {
                    if (_board[yDim, xDim] == BoardPiece.Empty || (foodCountsAsEmpty && _board[yDim, xDim] == BoardPiece.Food))
                    {
                        yield return (xDim, yDim);
                    }
                }
            }
        }

        public (int X, int Y) GetRandomEmptyPosition(Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            var empty = GetEmpty().ToList();

            if (empty.Count > 0)
            {
                return empty[random.Next(0, empty.Count)];
            }

            return (-1, -1);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var (xBound, yBound) = Bounds;

            builder.AppendLine(new string('_', xBound + 2));

            for (var y = 0; y < Bounds.Y; y++)
            {
                builder.Append("|");
                for (var x = 0; x < Bounds.X; x++)
                {
                    builder.Append(_board[y, x]);
                }
                builder.AppendLine("|");
            }

            builder.Append(new string('¯', xBound + 2));

            return builder.ToString();
        }

        public List<string> GetPrintLines()
        {
            var result = new List<string>();

            var (xBound, yBound) = Bounds;

            result.Add(new string('_', xBound + 2));

            for (var y = 0; y < Bounds.Y; y++)
            {
                var line = "|";
                for (var x = 0; x < Bounds.X; x++)
                {
                    line += _board[y, x];
                }

                line += "|";

                result.Add(line);
            }

            result.Add(new string('¯', xBound + 2));

            return result;
        }

        public char[,] ToMultiArray()
        {
            return _board;
        }
    }
}
