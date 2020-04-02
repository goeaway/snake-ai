using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class SnakeBit 
    {
        public (int X, int Y) Position { get; set; }

        // if this is null we're at the front
        public SnakeBit Head { get; set; }

        public SnakeBit(int x, int y, SnakeBit head)
        {
            Head = head; 
            Position = (x, y);
        }

        private int InnerCount(int current)
        {
            if (Head == null)
            {
                return ++current;
            }

            return Head.InnerCount(++current);
        }

        public int Count()
        {
            return InnerCount(0);
        }

        public SnakeBit GetHead()
        {
            if (Head == null)
            {
                return this;
            }

            return Head.GetHead();
        }

        public (int X, int Y) GetHeadPosition()
        {
            if (Head == null)
            {
                return Position;
            }

            return Head.GetHeadPosition();
        }

        private void InnerGetPositions(List<(int X, int Y)> list)
        {
            Head?.InnerGetPositions(list);
            list.Add(Position);
        }

        public (int X, int Y)[] GetPositions()
        {
            var list = new List<(int X, int Y)>();
            InnerGetPositions(list);
            return list.ToArray();
        }
    }
}
