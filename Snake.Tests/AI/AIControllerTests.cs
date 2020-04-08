using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Snake.Abstractions;
using Snake.AI;
using Snake.Factories;

namespace Snake.Tests.AI
{
    [TestClass]
    [TestCategory("AI Controller")]
    public class AIControllerTests
    {
        [TestMethod]
        public void Ctor_Uses_Color_From_Options()
        {
            const ConsoleColor EXPECTED_COLOR = ConsoleColor.Blue;

            var options = new AIOptions
            {
                Color = EXPECTED_COLOR
            };

            var gameFactory = new CountdownGameFactory(10, 10);
            var router = new Router();
            var controller = new AIController(options, gameFactory, router);

            Assert.AreEqual(EXPECTED_COLOR, controller.Color);
        }

        [TestMethod]
        public void Ctor_Uses_Id_From_Options()
        {
            const string EXPECTED_ID = "id";

            var options = new AIOptions
            {
                Id = EXPECTED_ID
            };

            var gameFactory = new CountdownGameFactory(10, 10);
            var router = new Router();
            var controller = new AIController(options, gameFactory, router);

            Assert.AreEqual(EXPECTED_ID, controller.Id);
        }

        [TestMethod]
        public void Reset_Creates_New_Game()
        {
            var gameFactory = new CountdownGameFactory(10, 10);
            var router = new Router();
            var controller = new AIController(new AIOptions(), gameFactory, router);
            var current = controller.CurrentGame;

            controller.Reset();
            Assert.AreNotSame(current, controller.CurrentGame);
        }

        [TestMethod]
        public void Reset_Creates_New_Hamiltonian_Cycle()
        {
            var gameFactory = new CountdownGameFactory(10, 10);
            var routerMock = new Mock<IRouter>();
            
            var controller = new AIController(new AIOptions(), gameFactory, routerMock.Object);

            routerMock
                .Verify(m => m.HamiltonianCycle(It.IsAny<char[,]>()), Times.Once);

            controller.Reset();

            routerMock
                .Verify(m => m.HamiltonianCycle(It.IsAny<char[,]>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Act_Moves_Snake_In_Game()
        {
            var gameFactory = new CountdownGameFactory(10, 10);
            var router = new Router();
            var controller = new AIController(AIOptions.GetDefault(0), gameFactory, router);

            var startPos = controller.CurrentGame.Snake.GetHeadPosition();

            var result = controller.Act();

            var endPos = controller.CurrentGame.Snake.GetHeadPosition();

            Assert.AreNotEqual(startPos, endPos);
        }

        [TestMethod]
        public void Act_Uses_Shortest_Route_To_Food_When_Snake_Less_Than_Size_Limit()
        {

        }

        [TestMethod]
        public void Act_Stays_On_Hamiltonian_Path_When_Shortest_Route_Cuts_Snake()
        {

        }

        [TestMethod]
        public void Act_Stays_On_Hamiltonian_Path_When_Snake_Larger_Than_Size_Limit_And_Does_Not_Cut_Snake()
        {

        }
    }
}
