using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    public class BlockerHandler : IItemPickupHandler, IItemReactionHandler
    {
        public char Item => BoardPiece.BlockCreator;

        public bool PickupItem(Game game, (int X, int Y) pos, Random random, out char item)
        {
            item = Item;
            return true;
        }

        public void ReactToItem(Game game, (int X, int Y) pos, IList<FiniteItem> finiteItems, Random random)
        {
            var randomPos = game.Board.GetRandomEmptyPosition(random);

            finiteItems.Add(new FiniteItem { Item = BoardPiece.Blocker, Position = randomPos, TTL = 10 });
            // add an X to this board at random, 
            throw new NotImplementedException();
        }
    }
}
