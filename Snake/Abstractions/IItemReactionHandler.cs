using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Abstractions
{
    public interface IItemReactionHandler
    {
        /// <summary>
        /// Alters the game as a result of a snake in another game eating an item.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="pos"></param>
        void ReactToItem(Game game, (int X, int Y) pos);
    }
}
