using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel;

namespace Maze.Tests
{
    [TestClass()]
    public class MapTests
    {
        private Mock<IMapProvider> mapProvider = new();
        private Direction[,] Directions = new Direction[,] { { Direction.S, Direction.S },
            {Direction.N|Direction.E,Direction.N|Direction.W } };
        private void loadMapProvider(Direction[,]? directions)
        {
            if (directions==null)
            {
                mapProvider.Setup(map => map.CreateMap()).Returns(Directions);
            }
            else
            {
                mapProvider.Setup(map => map.CreateMap()).Returns(directions);
            }
        }

        [TestMethod()]
        public void MapTest()
        {
            loadMapProvider(null);
            Map map = new Map(mapProvider.Object);
            Assert.AreEqual(0, map.Width);
            Assert.AreEqual(0, map.Height);
            map.CreateMap();
            Assert.AreEqual(5, map.Width);
            Assert.AreEqual(5, map.Height);
            Assert.AreEqual(5, map.MapGrid.GetLength(0));
            Assert.AreEqual(5, map.MapGrid.GetLength(1));
            Assert.AreEqual(Block.Solid, map.MapGrid[0, 0]);
            Assert.AreEqual(Block.Empty, map.MapGrid[1, 1]);
        }

        [TestMethod()]
        public void gameOverTest()
        {
            Direction[,] directions = new Direction[,] {
            { Direction.N},
            };
            loadMapProvider(directions);
            Map map2 = new Map(mapProvider.Object);
            map2.CreateMap();
            Assert.IsTrue(map2.IsGameFinished);

            loadMapProvider(null);
            Map map = new Map(mapProvider.Object);
            map.CreateMap();
            Assert.IsFalse(map.IsGameFinished);
        }


        [TestMethod()]
        public void CreateMapTest()
        {
            Block[,] expected = new Block[,] { 
                { Block.Solid,Block.Solid, Block.Solid,Block.Solid, Block.Solid }, 
                { Block.Solid, Block.Empty, Block.Solid,Block.Empty, Block.Solid }, 
                {Block.Solid, Block.Empty, Block.Solid, Block.Empty, Block.Solid},
                { Block.Solid,Block.Empty, Block.Empty,Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };
            loadMapProvider(null);
            Map map = new Map(mapProvider.Object);
            map.CreateMap();
            for (int Y = 0; Y<expected.GetLength(0); Y++)
            {
                for (int X = 0; X < expected.GetLength(1); X++)
                {
                    Assert.AreEqual(expected[Y, X], map.MapGrid[Y, X]);
                }
                

            }
        }
        //TODO: implement tests for CreateMapTest and SaveDirectionMapTest
        [TestMethod()]
        [DataRow(5, 5)]
        [DataRow(10, 10)]
        [DataRow(9, 10)]
        [DataRow(10, 7)]
        public void CreateMapTestWithValues(int width, int height)
        {
            Direction[,] directions = new Direction[height, width];
            directions.SetValue(Direction.W, 0, 0);
            directions.SetValue(Direction.E, 0, 1);
            Block[,] expected = new Block[height*2+1, width*2+1];
            mapProvider.Setup(map => map.CreateMap(width, height)).Returns(directions);
            Map map = new Map(mapProvider.Object);
            map.CreateMap(width, height);
            Assert.AreEqual(height*2+1, map.Height);
            Assert.AreEqual(width*2+1, map.Width);
            Assert.AreEqual(expected.GetLength(0), map.MapGrid.GetLength(0));
            Assert.AreEqual(expected.GetLength(1), map.MapGrid.GetLength(1));
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SaveDirectionMapTest()
        {
            loadMapProvider(null);
            Map map = new Map(mapProvider.Object);
            map.SaveDirectionMap("");
        }
    }
}