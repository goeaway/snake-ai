using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    /// <summary>
    /// Removes tail of snake but increments score
    /// </summary>
    public class NegaPickupHandler : IItemPickupHandler
    {
        public string Item => Consts.Items.Nega;

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            game.Score++;

            if (game.Snake.Head != null)
            {
                var (remX, remY) = game.Snake.Position;
                game.Snake = game.Snake.Head;
                game.Board.Update(remX,remY,Consts.Items.Empty);
            }
            // this item has no effect on other players
            return false;
        }
    }
}
