using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snake.AI;

namespace Snake.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void AStar_Gets_The_Best_Route()
        {
            var grid = new string[,]
            {
                {"0", "0", "0", "0", "0", "0"},
                {"0", "1", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "1", "0"},
                {"0", "0", "0", "0", "0", "0"},
            };

            // 1,1
            // 4,5

            var path = RoutingEngine.ShortestRoutePositions(grid, (1, 1), (4, 5));

            Assert.AreEqual((4,5), path.Last());
        }

        [TestMethod]
        public void AStar_Works_On_Bounds()
        {
            var grid = new string[,]
            {
                {"0", "1", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "0", "0"},
                {"0", "0", "0", "0", "2", "0"},
            };

            // 1,0
            // 4,6

            var path = RoutingEngine.ShortestRoutePositions(grid, (1, 0), (4, 6));

            Assert.AreEqual((4, 6), path.Last());
        }

        [TestMethod]
        public void Gets_Longest_Route()
        {
            var grid = new string[,]
            {
            // x  0    1    2    3    4    5       y
                {"2", "0", "1", "0", "0", "0"}, // 0
                {"0", "0", "0", "0", "0", "0"}, // 1
                {"0", "0", "0", "0", "0", "0"}, // 2
                {"0", "0", "0", "0", "0", "0"}, // 3
                {"0", "0", "0", "0", "0", "0"}, // 4
                {"0", "0", "0", "0", "0", "0"}, // 5
                {"0", "0", "0", "0", "0", "0"}, // 6
            };

            var path = RoutingEngine.LongestRouteDirections(grid, (2, 0), (0, 0), (1, 0));
            Assert.AreEqual(41, path.Count);
        }


        [TestMethod]
        public void Creates_A_List_Of_Directions_That_Goes_To_Each_Place_In_A_Grid()
        {
            var grid = new int[10, 20];
            var start = (4, 4);

            //var directions = HamiltonianCycleEngine.Generate(grid, start);

            //Assert.AreEqual(200, directions.Count);
        }
    }
}
