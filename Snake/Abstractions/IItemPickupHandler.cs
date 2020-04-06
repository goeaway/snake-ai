using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Abstractions
{
    internal interface IItemPickupHandler
    {
        string Item { get; }
        bool HandleItem(Game game, (int X, int Y) pos);
    }
}
