using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Snake.CLI
{
    public class Options
    {
        [Option('d', "Difficulty", HelpText = "Faster snake is harder snake. Easy (0), Medium (1), Hard (2), Supersonic (3)")]
        public Difficulty Difficulty { get; set; }

        [Option('i', "Items", HelpText = "Additional items with will occasionally drop. Some more useful than others")]
        public bool Items { get; set; }

        [Option('p', "Players", HelpText = "Number of players", Default = (uint)1)]
        public uint Players { get; set; }

        [Option('a', "AI Players", HelpText = "Number of AI players")]
        public uint AIPlayers { get; set; }
    }
} 
