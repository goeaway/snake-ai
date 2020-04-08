using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    public class MoneyHandler : IItemPickupHandler
    {
        public char Item => BoardPiece.Money;

        public bool PickupItem(Game game, (int X, int Y) pos, Random random, out char item)
        {
            game.Score += 4;
            item = Item;
            return false;
        }
    }
}
