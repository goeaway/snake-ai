using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake.Abstractions;

namespace Snake.ItemPickupHandlers
{
    public class SuperPickupHandler : IItemPickupHandler
    {
        public string Item => Consts.Items.Super;

        public bool HandleItem(Game game, (int X, int Y) pos)
        {
            game.Score += 10;

            var positionsToRemove = game.Snake.GetPositions();

            // add score and reduce this snake to its head only
            game.Snake = game.Snake.GetHead();

            // remove all but the head position (last in the array)
            foreach (var (remX, remY) in positionsToRemove.Take(positionsToRemove.Count() - 1))
            {
                game.Board.Update(remX, remY, Consts.Items.Empty);
            }

            return false;
        }
    }
}
