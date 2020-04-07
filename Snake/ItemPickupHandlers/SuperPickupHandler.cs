using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    public class SuperPickupHandler : IItemPickupHandler
    {
        public char Item => BoardPiece.Super;

        public bool HandleItem(Game game, (int X, int Y) pos, out char item)
        {
            game.Score += 10;

            // remove all but the head position (last in the array)
            foreach (var (remX, remY) in game.Snake.GetPositions())
            {
                game.Board.Update(remX, remY, BoardPiece.Empty);
            }

            // add score and reduce this snake to its head only
            game.Snake = game.Snake.GetHead();

            item = Item;
            return false;
        }
    }
}
