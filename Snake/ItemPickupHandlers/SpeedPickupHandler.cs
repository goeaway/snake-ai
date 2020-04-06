using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    internal class SpeedPickupHandler : IItemPickupHandler
    {
        public string Item => ")";

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            // alter the game board or snake
            // return whether you changed anything
            return false;
        }
    }
}
