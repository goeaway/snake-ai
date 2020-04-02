using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Snake.AI
{
    public class Controller
    {
        private readonly Action<Board> _postMoveAction;

        public Controller()
        {
        }

        public Controller(Action<Board> postMoveAction)
        {
            _postMoveAction = postMoveAction;
        }

        public bool Act(Game game)
        {
            var hamiltonianPath = RoutingEngine.HamiltonianCycle(game.Board.ToMultiArray());

            var (headX, headY) = game.Snake.GetHeadPosition();

            var startPos = headX * headY;

            for (var i = startPos; i < hamiltonianPath.Count; i++)
            {
                if (game.Board.GetEmpty().Count() == 0)
                {
                    break;
                }

                var direction = hamiltonianPath[i];
                if (!game.Move(direction))
                {
                    return false;
                }

                _postMoveAction?.Invoke(game.Board);

                // make sure we keep going round the path, reset our i to 0
                if (i == hamiltonianPath.Count -1)
                {
                    i = -1;
                }
            }
            
            // if out of the loop, return true
            return true;
        }
    }
}
