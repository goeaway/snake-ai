using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Abstractions
{
    /// <summary>
    /// Handles the result of the snake eating a particular item on the board
    /// </summary>
    public interface IItemPickupHandler
    {
        /// <summary>
        /// Gets the item this handler handles
        /// </summary>
        string Item { get; }
        /// <summary>
        /// Alters the game as a result of the snake eating the item. Returns true if an event should be fired
        /// </summary>
        /// <param name="game"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool HandleItem(Game game, (int X, int Y) pos);
    }
}
