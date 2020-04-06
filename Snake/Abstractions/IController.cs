using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Abstractions
{
    public struct OnGameAlteredEventArgs
    {

    }

    public interface IController
    {
        event EventHandler<OnGameAlteredEventArgs> OnGameAltered;

        ConsoleColor Color { get; }
        Game CurrentGame { get; }
        string Id { get; }
        void Reset();
        bool Act();
    }
}
