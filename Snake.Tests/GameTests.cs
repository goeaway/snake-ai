using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Snake.Tests
{
    [TestClass]
    [TestCategory("Game")]
    public class GameTests
    {
        [TestMethod]
        public void Ctor_Throws_On_Missing_Board()
        {

        }

        [TestMethod]
        public void Ctor_Throws_On_Missing_Controller()
        {

        }

        [TestMethod]
        public void Ctor_Throws_On_Missing_Random()
        {

        }

        [TestMethod]
        public void Ctor_Throws_On_Missing_PickupHandlers()
        {

        }

        [TestMethod]
        public void Ctor_Adds_Food_To_Board()
        {

        }

        [TestMethod]
        public void Ctor_Adds_Single_Snake_Bit_To_Middle_Of_Board()
        {

        }

        [TestMethod]
        public void Move_Opposite_Direction_Is_Reversed()
        {

        }

        [TestMethod]
        public void Move_Returns_False_If_Direction_Goes_Out_Of_Bounds()
        {

        }

        [TestMethod]
        public void Move_Returns_False_If_Next_Board_Piece_Is_Snake()
        {

        }

        [TestMethod]
        [DataRow(Direction.Down)]
        public void Move_Updates_Snake_In_Direction_Set(Direction direction)
        {

        }

        [TestMethod]
        public void Move_Adds_Food_If_None_On_Board()
        {

        }

        [TestMethod]
        public void Move_Uses_Pickup_Handlers_If_Next_Board_Piece_Is_Not_Empty()
        {

        }

        [TestMethod]
        public void Move_Raises_Event_If_Pickup_Handler_Found_And_Returns_True()
        {

        }

        [TestMethod]
        public void Move_No_Event_Raised_If_Handler_Found_And_Returns_False()
        {

        }

        [TestMethod]
        public void Move_No_Event_Raised_If_No_Pickup_Handler_Found()
        {

        }

        [TestMethod]
        public void Move_Uses_Only_First_Handler_Found()
        {

        }

        [TestMethod]
        public void ReactToItem_Uses_Handler_If_Found()
        {

        }

        [TestMethod]
        public void ReactToItem_Uses_Only_First_Handler_Found()
        {

        }
    }
}
