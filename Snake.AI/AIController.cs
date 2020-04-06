using Snake.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Snake.AI
{
    public class AIController : IController
    {
        private const double SHORTCUT_SNAKE_SIZE_PERCENT_STOP = .5;
        private readonly IGameFactory _gameFactory;
        private List<(int X, int Y)> _hamiltonPath;
        private int _index;

        public event EventHandler<OnGameAlteredEventArgs> OnGameAltered;

        public string Id { get; }

        public Game CurrentGame { get; private set; }

        public ConsoleColor Color { get; }

        public AIController(AIOptions options, IGameFactory gameFactory)
        {
            Id = options.Id;
            Color = options.Color;

            _gameFactory = gameFactory;
            Reset();
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

        public bool Act()
        {
            _index = GetNextIndex(_hamiltonPath, CurrentGame.Snake.GetHeadPosition());

            var nextPos = _hamiltonPath[_index];
            var headPos = CurrentGame.Snake.GetHeadPosition();

            // don't follow the hamiltonian cycle the whole time if we have the space, otherwise it's boring and slow
            // this tries to skip parts of the cycle if it won't cause us to go into ourselves
            if (CurrentGame.Snake.Count() < SHORTCUT_SNAKE_SIZE_PERCENT_STOP * CurrentGame.Board.Count)
            {
                var newShortPath = RoutingEngine.ShortestRoutePositions(
                    CurrentGame.Board.ToMultiArray(),
                    headPos,
                    CurrentGame.Board.GetFoodPosition(),
                    CurrentGame.Snake.GetPositions());

                if (newShortPath.Count > 1)
                {
                    // newShortPath[0] is the snake's head so we don't care about that one
                    var nextInShortPath = newShortPath[1]; 

                    var headIndex = _hamiltonPath.IndexOf(headPos);
                    var tailIndex = _hamiltonPath.IndexOf(CurrentGame.Snake.Position);
                    var foodIndex = _hamiltonPath.IndexOf(CurrentGame.Board.GetFoodPosition());
                    var nextIndex = _hamiltonPath.IndexOf(nextInShortPath);
                    // don't take a shortcut if we'd end up right on our own tail
                    if (newShortPath.Count != 2 && Math.Abs(foodIndex - tailIndex) != 1)
                    {
                        var headRelativeToTail = GetRelativeDistance(tailIndex, headIndex, CurrentGame.Board.Count);
                        var nextRelativeToTail = GetRelativeDistance(tailIndex, nextIndex, CurrentGame.Board.Count);
                        var foodRelativeToTail = GetRelativeDistance(tailIndex, foodIndex, CurrentGame.Board.Count);
                        // if next doesn't take us backwards and we end up closer to the food than the tail
                        if (nextRelativeToTail > headRelativeToTail && nextRelativeToTail <= foodRelativeToTail)
                        {
                            nextPos = nextInShortPath;
                            // keep track of the current index, if next time we can't find a route, we can just use the index to get pack on the hamilton path
                            _index = _hamiltonPath.IndexOf(nextPos) - 1;
                        }
                    }
                }
            }

            var direction = DirectionExtensions.GetDirectionFromXY(headPos, nextPos);
            _index = GetNextIndex(_hamiltonPath, nextPos);

            return CurrentGame.Move(direction);
        }

        public void Reset()
        {
            CurrentGame = _gameFactory.CreateGame();
            _hamiltonPath = RoutingEngine.HamiltonianCycle(CurrentGame.Board.ToMultiArray());
        }
    }
}
