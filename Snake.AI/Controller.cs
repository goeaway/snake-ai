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
        private readonly bool _takeShortcuts;
        private const double SHORTCUT_SNAKE_SIZE_PERCENT_STOP = .5;

        public Controller(bool takeShortcuts, Action<Board> postMoveAction)
        {
            _postMoveAction = postMoveAction;
            _takeShortcuts = takeShortcuts;
        }

        private int GetNextIndex(IList<(int X, int Y)> path, (int X, int Y) current)
        {
            return GetNextIndex(path, path.IndexOf(current));
        }

        private int GetNextIndex(IList<(int X, int Y)> path, int index)
        {
            return path.Count - 1 <= index ? 0 : index + 1;
        }

        private int GetRelativeDistance(int from, int to, int size)
        {
            if (from > to)
            {
                to += size;
            }

            return to - from;
        }

        public bool Act(Game game)
        {
            var hamiltonianPath = RoutingEngine.HamiltonianCycle(game.Board.ToMultiArray());
            var index = GetNextIndex(hamiltonianPath, game.Snake.GetHeadPosition());

            while (game.Board.GetEmpty().Count() != 0)
            {
                var nextPos = hamiltonianPath[index];
                var headPos = game.Snake.GetHeadPosition();

                // don't follow the hamiltonian cycle the whole time if we have the space, otherwise it's boring and slow
                // this tries to skip parts of the cycle if it won't cause us to go into ourselves
                if (_takeShortcuts && game.Snake.Count() < SHORTCUT_SNAKE_SIZE_PERCENT_STOP * game.Board.Count)
                {
                    var newShortPath = RoutingEngine.ShortestRoutePositions(
                        game.Board.ToMultiArray(),
                        headPos,
                        game.Board.GetFoodPosition(),
                        game.Snake.GetPositions());

                    if (newShortPath.Count > 1)
                    {
                        // newShortPath[0] is the snake's head so we don't care about that one
                        var nextInShortPath = newShortPath[1];

                        var headIndex = hamiltonianPath.IndexOf(headPos);
                        var tailIndex = hamiltonianPath.IndexOf(game.Snake.Position);
                        var foodIndex = hamiltonianPath.IndexOf(game.Board.GetFoodPosition());
                        var nextIndex = hamiltonianPath.IndexOf(nextInShortPath);
                        // don't take a shortcut if we'd end up right on our own tail
                        if (newShortPath.Count != 2 && Math.Abs(foodIndex - tailIndex) != 1)
                        {
                            var headRelativeToTail = GetRelativeDistance(tailIndex, headIndex, game.Board.Count);
                            var nextRelativeToTail = GetRelativeDistance(tailIndex, nextIndex, game.Board.Count);
                            var foodRelativeToTail = GetRelativeDistance(tailIndex, foodIndex, game.Board.Count);
                            // if next doesn't take us backwards and we end up closer to the food than the tail
                            if (nextRelativeToTail > headRelativeToTail && nextRelativeToTail <= foodRelativeToTail)
                            {
                                nextPos = nextInShortPath;
                                // keep track of the current index, if next time we can't find a route, we can just use the index to get pack on the hamilton path
                                index = hamiltonianPath.IndexOf(nextPos) - 1;
                            }
                        }
                    }
                }

                var direction = DirectionExtensions.GetDirectionFromXY(headPos, nextPos);
                if (!game.Move(direction))
                {
                    return false;
                }

                _postMoveAction?.Invoke(game.Board);

                index = GetNextIndex(hamiltonianPath, nextPos);
            }

            // if out of the loop, return true
            return true;
        }
    }
}
