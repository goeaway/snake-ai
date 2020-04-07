using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    public class RandomPickupHandler : IItemPickupHandler
    {
        public string Item => Consts.Items.Random;

        private readonly Random _randomiser;

        public RandomPickupHandler(Random randomiser)
        {
            _randomiser = randomiser;
        }

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            // pick a random handler and do that
            return game.PickupHandlers
                .Where(h => h.Item != Consts.Items.Food)
                .ToList()[_randomiser.Next(0, game.PickupHandlers.Count - 2)]
                .HandleItem(game, pos);
        }
    }
}
