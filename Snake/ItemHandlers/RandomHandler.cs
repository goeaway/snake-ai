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

        private readonly Random _randomiser;
        private readonly IEnumerable<IItemPickupHandler> _pickupHandlers;

        public RandomHandler(Random randomiser, IEnumerable<IItemPickupHandler> pickupHandlers)
        {
            _randomiser = randomiser;
            _pickupHandlers = pickupHandlers;
        }

        public bool PickupItem(Game game, (int X, int Y) pos, out char item)
        {
            // pick a random handler and do that
            return _pickupHandlers
                .Where(h => h.Item != BoardPiece.Food && h.Item != Item)
                .ToList()[_randomiser.Next(0, _pickupHandlers.Count() - 3)]
                .PickupItem(game, pos, out item);
        }
    }
}
