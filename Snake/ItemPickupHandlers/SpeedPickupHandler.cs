using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    /// <summary>
    /// Triggers an event to speed up other players on the board
    /// </summary>
    public class SpeedPickupHandler : IItemPickupHandler, IItemPickupReactionHandler
    {
        public char Item => BoardPiece.Speed;

        public bool HandleItem(Game game, (int X, int Y) pos, out char item)
        {
            item = Item;
            return true;
        }

        public void ReactToItem(Game game, (int X, int Y) pos)
        {
            throw new NotImplementedException();
        }
    }
}
