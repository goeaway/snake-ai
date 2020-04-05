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
    }
}
