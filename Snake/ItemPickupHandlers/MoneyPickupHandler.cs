using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    public class MoneyPickupHandler : IItemPickupHandler
    {
        public char Item => BoardPiece.Money;

        public bool HandleItem(Game game, (int X, int Y) pos, out char item)
        {
            game.Score += 4;
            item = Item;
            return false;
        }
    }
}
