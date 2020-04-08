using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Tests
{
    public static class TestUtils
    {
        public static Board CreateTestBoard()
        {
            return new Board(10, 10);
        }
    }
}
