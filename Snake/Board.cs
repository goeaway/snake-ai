using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class Board
    {
        public const string Empty = " ";
        public const string Snake = "0";
        public const string Food = "+";

        private readonly string[,] _board;

        public (int, int) Bounds { get; } 

        public Board(int xSize, int ySize)
        {
            Bounds = (xSize, ySize);
            _board = new string[ySize,xSize];

            for (var yDim = 0; yDim < ySize; yDim++)
            {
                for (var xDim = 0; xDim < xSize; xDim++)
                {
                    _board[yDim, xDim] = Empty;
                }
            }
        }

        public void Update(int x, int y, string value)
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

        public void Remove(string value)
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Item2; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.Item1; xDim++)
                {
                    if (_board[yDim, xDim] == value)
                    {
                        _board[yDim, xDim] = Board.Empty;
                    }
                }
            }
        }

        public string GetValue(int x, int y)
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

            for (var yDim = 0; yDim < Bounds.Item2; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.Item1; xDim++)
                {
                    if (yDim == y && xDim == x)
                    {
                        return _board[yDim, xDim];
                    }
                }
            }

            return Board.Empty;
        }

        public (int X, int Y) GetFoodPosition()
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Item2; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.Item1; xDim++)
                {
                    if (_board[yDim, xDim] == Food)
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

        public IEnumerable<(int, int)> GetEmpty(bool itemCountsAsEmpty)
        {
            var (xBound, yBound) = Bounds;

            for (var yDim = 0; yDim < Bounds.Item2; yDim++)
            {
                for (var xDim = 0; xDim < Bounds.Item1; xDim++)
                {
                    if (_board[yDim, xDim] == Empty || (itemCountsAsEmpty && _board[yDim, xDim] == Food))
                    {
                        yield return (xDim, yDim);
                    }
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var (xBound, yBound) = Bounds;

            builder.AppendLine(new string('_', xBound + 2));

            for (var y = 0; y < Bounds.Item2; y++)
            {
                builder.Append("|");
                for (var x = 0; x < Bounds.Item1; x++)
                {
                    builder.Append(_board[y, x]);
                }
                builder.AppendLine("|");
            }

            builder.Append(new string('¯', xBound + 2));

            return builder.ToString();
        }

        public string[,] ToMultiArray()
        {
            return _board;
        }
    }
}
