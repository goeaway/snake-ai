using System;
using System.Collections.Generic;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    public class MoneyPickupHandler : IItemPickupHandler
    {
        public string Item => Consts.Items.Money;

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            game.Score += 4;
            return false;
        }
    }
}
