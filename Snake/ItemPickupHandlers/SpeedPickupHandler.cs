using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    /// <summary>
    /// Triggers an event to speed up other players on the board
    /// </summary>
    public class SpeedPickupHandler : IItemPickupHandler
    {
        public string Item => Consts.Items.Speed;

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            // alter the game board or snake
            // return whether you changed anything
            return true;
        }
    }
}
