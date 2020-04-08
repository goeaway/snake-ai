using Snake.Abstractions;
using Snake.ItemHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Factories
{
    public class CountdownGameFactory : IGameFactory
    {
        private readonly int _boardX;
        private readonly int _boardY;
        private readonly Random _random;
        private readonly IList<IItemPickupHandler> _pickupHandlers;

        public CountdownGameFactory(int boardX, int boardY, int? seed = null)
        {
            _boardX = boardX;
            _boardY = boardY;
            _random = new Random(seed ?? Environment.TickCount);
            _pickupHandlers = new List<IItemPickupHandler>
            {
                new FoodHandler(_random),
                new SpeedHandler(),
                new NegaHandler(),
                new SuperHandler(),
                new MoneyHandler()
            };

            _pickupHandlers.Add(new RandomHandler(_random, _pickupHandlers)); ;
        }

        public Game CreateGame(IController controller) => new Game(controller, new Board(_boardX, _boardY), _random, _pickupHandlers);
    }
}
