using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    /// <summary>
    /// Removes tail of snake but increments score
    /// </summary>
    public class NegaHandler : IItemPickupHandler
    {
        public char Item => BoardPiece.Nega;

        public bool PickupItem(Game game, (int X, int Y) pos, out char item)
        {
            game.Score++;

            if (game.Snake.Head != null)
            {
                var (remX, remY) = game.Snake.Position;
                game.Snake = game.Snake.Head;
                game.Board.Update(remX,remY,BoardPiece.Empty);
            }

            item = Item;
            // this item has no effect on other players
            return false;
        }
    }
}
