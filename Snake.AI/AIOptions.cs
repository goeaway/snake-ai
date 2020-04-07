using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.AI
{
    public class AIOptions
    {
        public string Id { get; set; }
        public ConsoleColor Color { get; set; }

        public static AIOptions GetDefault(int index)
        {
            switch (index)
            {
                case 0:
                    return new AIOptions
                    {
                        Id = "AI",
                        Color = ConsoleColor.Red,
                    };
                case 1:
                    return new AIOptions
                    {
                        Id = "AI",
                        Color = ConsoleColor.Green,
                    };
                default:
                    throw new NotSupportedException("only 2 players supported");
            }
        }
    }
}
