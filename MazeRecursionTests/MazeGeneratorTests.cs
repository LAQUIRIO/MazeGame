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
        public void createMapWithValuesTest()
        {
            Direction[,] ExpectedMaze = new Direction[,]
            {
                {Direction.S, Direction.E|Direction.S, Direction.E|Direction.W, Direction.S|Direction.W},
                {Direction.N|Direction.E, Direction.N|Direction.W, Direction.S, Direction.N|Direction.E},
                {Direction.E|Direction.S, Direction.E|Direction.S|Direction.W, Direction.N|Direction.W, Direction.E|Direction.S},
                {Direction.N|Direction.S, Direction.N|Direction.E, Direction.S|Direction.W, Direction.N|Direction.E},
            };
            int expectedArraySizes = 5;
            Direction[,] directions = _mapProvider.CreateMap(4,4);
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
        public void CreateMapTest()
        {
            Direction[,] ExpectedMaze = new Direction[,]
            {
                {Direction.S, Direction.E|Direction.S, Direction.E|Direction.W, Direction.S|Direction.W, Direction.S},
                {Direction.N|Direction.E, Direction.N|Direction.W, Direction.S, Direction.N|Direction.E, Direction.N|Direction.S|Direction.W},
                {Direction.E|Direction.S, Direction.E|Direction.S|Direction.W, Direction.N|Direction.W, Direction.E|Direction.S, Direction.N|Direction.W},
                {Direction.N|Direction.S, Direction.N|Direction.E, Direction.S|Direction.W, Direction.N|Direction.E, Direction.S|Direction.W},
                {Direction.N|Direction.E, Direction.W, Direction.N|Direction.E, Direction.W|Direction.E, Direction.N|Direction.W }
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

    }
}