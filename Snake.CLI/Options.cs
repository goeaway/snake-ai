using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Snake.CLI
{
    public class Options
    {
        [Option('p', "Players", HelpText = "Number of players")]
        public uint Players { get; set; }

        [Option('a', "AI Players", HelpText = "Number of AI players")]
        public uint AIPlayers { get; set; }
    }
} 
