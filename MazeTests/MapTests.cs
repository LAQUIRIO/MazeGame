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
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateMapTest1()
        {
            loadMapProvider(null);
            Map map = new Map(mapProvider.Object);
            map.CreateMap(1,2);
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