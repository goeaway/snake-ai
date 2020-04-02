using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Snake.AI
{
    public static class RoutingEngine
    {
        private class PathPiece
        {
            public (int X, int Y) Position { get; set; }
            public int FromStart { get; set; }
            public int ToEnd { get; set; }

            public int Combined => FromStart + ToEnd;

            public PathPiece Previous { get; set; }

            public List<(int X, int Y)> GetList()
            {
                if (Previous == null)
                {
                    return new List<(int X, int Y)>
                    {
                        Position
                    };
                }

                var previous = Previous.GetList();
                previous.Add(Position);

                return previous;
            }
        }

        private static PathPiece GetSmallest(List<PathPiece> list)
        {
            return list?
                .GroupBy(i => i.Combined)
                .OrderBy(g => g.Key)
                .FirstOrDefault()?
                .OrderBy(i => i.ToEnd)
                .FirstOrDefault();
        }

        private static List<(int X, int Y)> GetNeighbours()
        {
            return new List<(int X, int Y)>
            {
                // left
                (-1, 0),
                // up 
                (0,-1),
                // right
                (1, 0),
                // down
                (0, 1)
            };
        }

        private static (int X, int Y) GetAdjacent((int X, int Y) to, Direction direction)
        {
            return
                direction == Direction.Left ? (to.X - 1, to.Y) :
                direction == Direction.Up ? (to.X, to.Y - 1) :
                direction == Direction.Right ? (to.X + 1, to.Y) :
                (to.X, to.Y + 1);
        }

        private static bool PositionInGrid<T>(T[,] grid, (int X, int Y) position)
        {
            return position.X >= grid.GetLowerBound(1) && position.X <= grid.GetUpperBound(1) &&
                   position.Y >= grid.GetLowerBound(0) && position.Y <= grid.GetUpperBound(0);
        }

        private static List<Direction> ConvertToDirections(List<(int X, int Y)> path)
        {
            var result = new List<Direction>();

            if (path == null)
            {
                return result;
            }

            for (var i = 0; i < path.Count - 1; i++)
            {
                var current = path[i];
                var next = path[i + 1];

                result.Add(DirectionExtensions.GetDirectionFromXY(current, next));
            }

            return result;
        }

        private static bool PositionValid<T>(T[,] grid, (int X, int Y) position, (int X, int Y)[] disallowed)
        {
            return PositionInGrid(grid, position) && !disallowed.Contains(position);
        }

        public static List<(int X, int Y)> ShortestRoutePositions<T>(T[,] grid,
            (int X, int Y) from,
            (int X, int Y) to,
            params (int X, int Y)[] disallowed)
        {
            var path = new List<(int X, int Y)>();

            var start = new PathPiece { Position = from };

            var open = new List<PathPiece>
            {
                { start }
            };
            var closed = new List<PathPiece>();

            var xMax = grid.GetUpperBound(1);
            var yMax = grid.GetUpperBound(0);

            while (open.Count != 0)
            {
                var current = GetSmallest(open);

                if (current.Position == to)
                {
                    return current.GetList();
                }

                open.Remove(current);

                closed.Add(current);

                foreach (var (X, Y) in GetNeighbours())
                {
                    var (newX, newY) = (current.Position.X + X, current.Position.Y + Y);

                    if (!PositionValid(grid, (newX, newY), disallowed) ||
                        closed.Exists(c => c.Position == (newX, newY)))
                    {
                        continue;
                    }

                    if (open.Exists(o => o.Position == (newX, newY)))
                    {
                        var node = open.Single(o => o.Position == (newX, newY));
                        var f = Math.Abs(newX - from.X) + Math.Abs(newY - from.Y);

                        if (f < node.FromStart)
                        {
                            node.FromStart = f;
                            node.Previous = current;
                        }
                    }
                    else
                    {
                        open.Add(new PathPiece
                        {
                            Position = (newX, newY),
                            FromStart = Math.Abs(newX - from.X) + Math.Abs(newY - from.Y),
                            ToEnd = Math.Abs(newX - to.X) + Math.Abs(newY - to.Y),
                            Previous = current
                        });
                    }
                }
            }

            return null;
        }

        public static List<Direction> ShortestRouteDirections<T>(
            T[,] grid, 
            (int X, int Y) from, 
            (int X, int Y) to, 
            params (int X, int Y)[] disallowed) => ConvertToDirections(ShortestRoutePositions(grid, from, to, disallowed));

        public static List<Direction> LongestRouteDirections<T>(
            T[,] grid, 
            (int X, int Y) from, 
            (int X, int Y) to,  
            params (int X, int Y)[] disallowed)
        {
            var positions = ShortestRoutePositions(grid, from, to, disallowed);
            var directions = ConvertToDirections(positions);

            var currentPos = positions.First();
            var index = 0;
            var alreadyAddedPositions = new List<(int X, int Y)>(positions);

            while(true)
            {
                // figure out the current direction...
                var currentDirection = directions[index];
                // find the adjacent position in the direction
                var nextPos = GetAdjacent(currentPos, currentDirection);
                var extended = false;

                // test the two directions perpendicular to the current one
                foreach (var testDirection in currentDirection.GetPerpendicular())
                {
                    var currentTest = GetAdjacent(currentPos, testDirection);
                    var nextTest = GetAdjacent(nextPos, testDirection);

                    if (PositionValid(grid, currentTest, disallowed) && 
                        PositionValid(grid, nextTest, disallowed) &&
                        !alreadyAddedPositions.Any(ap => (ap.X == currentTest.X && ap.Y == currentTest.Y) || (ap.X == nextTest.X && ap.Y == nextTest.Y)))
                    {
                        alreadyAddedPositions.Add(currentTest);
                        alreadyAddedPositions.Add(nextTest);
                        directions.Insert(index, testDirection);
                        directions.Insert(index + 2, testDirection.GetOpposite());
                        extended = true;

                        break;
                    }
                }

                if (!extended)
                {
                    currentPos = nextPos;
                    index++;
                    if (index >= directions.Count)
                    {
                        break;
                    }
                }
            }

            return directions;
        }

        public static List<Direction> HamiltonianCycle<T>(
            T[,] grid)
        {
            var path = LongestRouteDirections(grid, (2, 0), (0, 0), (1, 0));

            // get from 0,0 to 1,0
            path.Add(Direction.Right);
            // get from 1,0 to 2,0
            path.Add(Direction.Right);
            return path.ToList();
        }
    }
}
