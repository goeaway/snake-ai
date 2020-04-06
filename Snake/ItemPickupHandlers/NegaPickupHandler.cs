using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    public class NegaPickupHandler : IItemPickupHandler
    {
        public string Item => "-";

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            game.Score++;

            if (game.Snake.Head != null)
            {
                game.Snake = game.Snake.Head;
            }

            return true;
        }
    }
}
