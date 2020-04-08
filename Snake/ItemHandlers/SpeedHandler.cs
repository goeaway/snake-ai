using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    /// <summary>
    /// Triggers an event to speed up other players on the board
    /// </summary>
    public class SpeedHandler : IItemPickupHandler, IItemReactionHandler
    {
        public char Item => BoardPiece.Speed;

        public bool PickupItem(Game game, (int X, int Y) pos, Random random, out char item)
        {
            item = Item;
            return true;
        }

        public void ReactToItem(Game game, (int X, int Y) pos, IList<FiniteItem> finiteItems, Random random)
        {
            throw new NotImplementedException();
        }
    }
}
