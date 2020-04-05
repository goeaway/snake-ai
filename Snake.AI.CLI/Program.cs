using System;
using System.Threading;

namespace Snake.AI.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var controller = new AIController(Guid.NewGuid().ToString(), new GameFactory(50, 20, Direction.Right));

            while (true)
            {
                var result = controller.Act();

                Thread.Sleep(500);
            }
        }
    }
}
