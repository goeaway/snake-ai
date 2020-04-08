using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemHandlers
{
    /// <summary>
    /// Adds an extra <see cref="SnakeBit"/> to the end and increments the score
    /// </summary>
    public class FoodHandler : IItemPickupHandler
    {
        public char Item => BoardPiece.Food;

        public bool PickupItem(Game game, (int X, int Y) pos, Random random, out char item)
        {
            game.Score++;
            var (tX, tY) = game.Snake.Position;
            game.Snake = new SnakeBit(tX, tY, game.Snake);

            // occasionally add a new item to the board
            if (random.Next(0, 10) == 1)
            {
                var randomPos = game.Board.GetRandomEmptyPosition(random);

                if (randomPos != (-1, -1))
                {
                    game.Board.Update(randomPos.X, randomPos.Y,
                        game.ItemsUsed.Where(i => i != BoardPiece.Food).ToList()[random.Next(0, game.ItemsUsed.Count() - 1)]);
                }
            }

            item = Item;
            // this item has no effect on other players
            return false;
        }
    }
}
