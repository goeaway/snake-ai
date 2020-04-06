using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Abstractions
{
    public interface IGameFactory
    {
        Game CreateGame();
    }
}
