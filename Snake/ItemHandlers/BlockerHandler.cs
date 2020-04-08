using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    public class BlockerHandler : IItemPickupHandler, IItemReactionHandler
    {
        public char Item => BoardPiece.BlockCreator;

        public bool PickupItem(Game game, (int X, int Y) pos, out char item)
        {
            item = Item;
            return true;
        }

        public void ReactToItem(Game game, (int X, int Y) pos)
        {
            // add an X to this board at random, 
            throw new NotImplementedException();
        }
    }
}
