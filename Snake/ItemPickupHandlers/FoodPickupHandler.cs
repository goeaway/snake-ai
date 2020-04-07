using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    /// <summary>
    /// Adds an extra <see cref="SnakeBit"/> to the end and increments the score
    /// </summary>
    internal class FoodPickupHandler : IItemPickupHandler
    {
        private readonly Random _randomiser;

        public FoodPickupHandler(Random randomiser)
        {
            _randomiser = randomiser;
        }

        public string Item => Consts.Items.Food;

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            game.Score++;
            var (tX, tY) = game.Snake.Position;
            game.Snake = new SnakeBit(tX, tY, game.Snake);

            // occaisionally add a new item to the board
            if (_randomiser.Next(0, 5) == 1)
            {
                var empty = game.Board.GetEmpty().ToList();
                var (x, y) = empty[_randomiser.Next(0, empty.Count)];

                game.Board.Update(x, y,
                    game.PickupHandlers
                        .Where(p => p.Item != Consts.Items.Food)
                        .ToList()[_randomiser.Next(0, game.PickupHandlers.Count() - 1)].Item
                );
            }
            // this item has no effect on other players
            return false;
        }
    }
}
