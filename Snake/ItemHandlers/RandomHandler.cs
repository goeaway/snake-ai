using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    public class RandomHandler : IItemPickupHandler
    {
        public char Item => BoardPiece.Random;

        private readonly IEnumerable<IItemPickupHandler> _pickupHandlers;

        public RandomHandler(IEnumerable<IItemPickupHandler> pickupHandlers)
        {
            _pickupHandlers = pickupHandlers;
        }

        public bool PickupItem(Game game, (int X, int Y) pos, Random random, out char item)
        {
            // pick a random handler and do that
            return _pickupHandlers
                .Where(h => h.Item != BoardPiece.Food && h.Item != Item)
                .ToList()[random.Next(0, _pickupHandlers.Count() - 3)]
                .PickupItem(game, pos, random, out item);
        }
    }
}
