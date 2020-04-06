using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    internal class FoodPickupHandler : IItemPickupHandler
    {
        private readonly Random _randomiser;

        public FoodPickupHandler(Random randomiser)
        {
            _randomiser = randomiser;
        }

        public string Item => Board.Food;

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            game.Score++;
            var (tX, tY) = game.Snake.Position;
            game.Snake = new SnakeBit(tX, tY, game.Snake);

            if (_randomiser.Next(0, 20) == 1)
            {
                var empty = game.Board.GetEmpty().ToList();
                var (x, y) = empty[_randomiser.Next(0, empty.Count)];

                game.Board.Update(x, y,
                    game.PickupHandlers
                        .Where(p => p.Item != Board.Food)
                        .ToList()[_randomiser.Next(0, game.PickupHandlers.Count() - 1)].Item
                );
            }

            return true;
        }
    }
}
