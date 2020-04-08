using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.AI
{
    public interface IRouter
    {
        IList<(int X, int Y)> Shortest<T>(T[,] grid,
            (int X, int Y) from,
            (int X, int Y) to,
            params (int X, int Y)[] disallowed);

        IList<(int X, int Y)> Longest<T>(
            T[,] grid,
            (int X, int Y) from,
            (int X, int Y) to,
            params (int X, int Y)[] disallowed);

        IList<(int X, int Y)> HamiltonianCycle<T>(T[,] grid);
    }
}
