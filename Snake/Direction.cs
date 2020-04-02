using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    public static class DirectionExtensions
    {
        public static Direction GetDirectionFromXY((int X, int Y) posA, (int X, int Y) posB)
        {
            return posA.X - posB.X == 1 ? Direction.Left :
                posA.X - posB.X == -1 ? Direction.Right :
                posA.Y - posB.Y == 1 ? Direction.Up :
                Direction.Down;
        }

        public static Direction GetOpposite(this Direction direction)
        {
            return
                direction == Direction.Left ? Direction.Right :
                direction == Direction.Right ? Direction.Left :
                direction == Direction.Up ? Direction.Down :
                Direction.Up;
        }

        public static Direction[] GetPerpendicular(this Direction direction)
        {
            return direction == Direction.Left || direction == Direction.Right
                    ? new Direction[] {Direction.Up, Direction.Down}
                    : new Direction[] {Direction.Left, Direction.Right};
        }
    }
}
