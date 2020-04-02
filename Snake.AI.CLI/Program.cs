using System;
using System.Threading;

namespace Snake.AI.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var controller = new Controller(board =>
            {
                Console.SetCursorPosition(0,0);
                Console.WriteLine(board);
                Thread.Sleep(5);
            });

            while (true)
            {
                var result = controller.Act(new Game(new Board(40, 20), Direction.Right));

                Thread.Sleep(500);
            }
        }
    }
}
