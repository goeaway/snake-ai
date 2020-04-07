using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Players
{
    public class PlayerOptions
    {
        public string Id { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleKey Up { get; set; }
        public ConsoleKey Right { get; set; }
        public ConsoleKey Down { get; set; }
        public ConsoleKey Left { get; set; }

        public static PlayerOptions GetDefault(int index)
        {
            switch (index)
            {
                case 0:
                    return new PlayerOptions
                    {
                        Id = "Player 1",
                        Color = ConsoleColor.Yellow,
                        Up = ConsoleKey.UpArrow,
                        Right = ConsoleKey.RightArrow,
                        Down = ConsoleKey.DownArrow,
                        Left = ConsoleKey.LeftArrow
                    };
                case 1:
                    return new PlayerOptions
                    {
                        Id = "Player 2",
                        Color = ConsoleColor.Cyan,
                        Up = ConsoleKey.W,
                        Right = ConsoleKey.D,
                        Down = ConsoleKey.S,
                        Left = ConsoleKey.A
                    };
                default:
                    throw new NotSupportedException("only 2 players supported");
            }
        }
    }
}
