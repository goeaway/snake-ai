using Snake.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly int _boardX;
        private readonly int _boardY;
        private readonly Direction _startDirection;
        private readonly int? _seed;


        public GameFactory(int boardX, int boardY, Direction startDirection, int? seed = null)
        {
            _boardX = boardX;
            _boardY = boardY;
            _startDirection = startDirection;
            _seed = seed;
        }

        public Game CreateGame()
        {
            return new Game(new Board(_boardX, _boardY), _startDirection, _seed);
        }
    }
}
