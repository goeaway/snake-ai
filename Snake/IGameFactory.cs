using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public interface IGameFactory
    {
        Game CreateGame();
    }
}
