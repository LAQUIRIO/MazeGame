using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeRecursion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze;

namespace MazeRecursion.Tests
{
    [TestClass()]
    public class MazeGeneratorTests
    {
        private IMapProvider _mapProvider = new MazeGenerator(null, null);
        [TestInitialize]
        public void TestInitialize()
        {
            MapVector vector = new MapVector(0, 0);
            _mapProvider = new MazeGenerator(vector, 1);
        }
        [TestMethod()]
        public void CreateMapWithValuesTest()
        {
            Direction[,] ExpectedMaze = new Direction[,]
            {
                {Direction.E, Direction.W|Direction.S, Direction.E|Direction.S, Direction.S|Direction.W},
                {Direction.S, Direction.N|Direction.E, Direction.W|Direction.N, Direction.N|Direction.S},
                {Direction.N|Direction.S|Direction.E, Direction.E|Direction.S|Direction.W, Direction.W, Direction.N|Direction.S},
                {Direction.N|Direction.S, Direction.N|Direction.E, Direction.S|Direction.W, Direction.S|Direction.N},
                {Direction.N|Direction.E, Direction.W, Direction.E|Direction.N, Direction.W|Direction.N},
            };
            int expectedWidth = 4;
            int expectedHeight = 5;
            Direction[,] directions = _mapProvider.CreateMap(4,5);
            Assert.AreEqual(expectedHeight, directions.GetLength(0));
            Assert.AreEqual(expectedWidth, directions.GetLength(1));
            for (int i = 0; i < expectedHeight; i++)
            {
                for (int j = 0; j < expectedWidth; j++)
                {
                    Assert.AreEqual(ExpectedMaze[i, j], directions[i, j]);
                }
            }
        }

        
        [TestMethod()]
        public void CreateMapTest()
        {
            Direction[,] ExpectedMaze = new Direction[,]
            {
                {Direction.E, Direction.W|Direction.S, Direction.E|Direction.S, Direction.E|Direction.W,Direction.W|Direction.S},
                {Direction.S, Direction.N|Direction.E, Direction.W|Direction.N, Direction.S,Direction.N|Direction.S},
                {Direction.S|Direction.N, Direction.E|Direction.S, Direction.E|Direction.W, Direction.N|Direction.S|Direction.W, Direction.N|Direction.S},
                {Direction.N|Direction.S, Direction.N|Direction.S, Direction.S|Direction.E, Direction.N|Direction.W, Direction.S|Direction.N},
                {Direction.N|Direction.E, Direction.W|Direction.N, Direction.N|Direction.E, Direction.W|Direction.E, Direction.N|Direction.W }
            };
            int expectedArraySizes = 5;
            Direction[,] directions = _mapProvider.CreateMap();
            Assert.AreEqual(expectedArraySizes, directions.GetLength(0));
            Assert.AreEqual(expectedArraySizes, directions.GetLength(1));
            for (int i = 0; i < expectedArraySizes; i++)
            {
                for (int j = 0; j < expectedArraySizes; j++)
                {
                    Assert.AreEqual(ExpectedMaze[i, j], directions[i, j]);
                }
            }
        }

        [TestMethod()]
        [DataRow(5, 5)]
        [DataRow(10, 10)]
        [DataRow(15, 15)]
        [DataRow(20, 10)]
        [DataRow(10, 20)]
        public void RandomMapGenerationTest(int with, int height)
        {
            _mapProvider = new MazeGenerator(null, null);
            Direction[,] directions = _mapProvider.CreateMap(with, height);
            Assert.AreEqual(with, directions.GetLength(1));
            Assert.AreEqual(height, directions.GetLength(0));
            foreach (Direction dir in directions)
            {
                Assert.IsTrue(dir != Direction.None);
            }
        }

    }
}