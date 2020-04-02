using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Snake.CLI
{
    public class Options
    {
        [Option('o', "Open", HelpText = "Snake will be able to go through walls an appear on the other side of the board")]
        public bool OpenWalls { get; set; }

        [Option('d', "Difficulty", HelpText = "Faster snake is harder snake. Easy (0), Medium (1), Hard (2), Supersonic (3)")]
        public Difficulty Difficulty { get; set; }

        [Option('p', "Power Ups", HelpText = "Additional items with will occasionally drop. Some more useful than others")]
        public bool Powerups { get; set; }
    }
} 
