using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Abstractions
{
    public interface IController
    {
        

        ConsoleColor Color { get; }
        Game CurrentGame { get; }
        string Id { get; }
        void Reset();
        bool Act();
    }
}
