using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Snake.Abstractions;
using Snake.ItemHandlers;

namespace Snake.Tests.ItemHandlers
{
    [TestClass]
    [TestCategory("Item Handlers")]
    public class FoodHandlerTests
    {
        [TestMethod]
        public void Food_Adds_To_Snake()
        {
            var controllerMoq = new Mock<IController>();
            var board = TestUtils.CreateTestBoard();

            var random = new Random(1);
            var handler = new FoodHandler(random);

            var game = new Game(controllerMoq.Object, board, random, new List<IItemPickupHandler> { handler });

            var result = handler.PickupItem(game, (1,1), out var item);
        }

        [TestMethod]
        public void Food_Adds_To_Score()
        {

        }

        [TestMethod]
        public void Food_Adds_Random_Item()
        {

        }

        [TestMethod]
        public void Food_Returns_False()
        {

        }

        [TestMethod]
        public void Food_Outs_Food()
        {

        }
    }
}
