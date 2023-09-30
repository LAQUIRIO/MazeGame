using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        [DataRow(Direction.N, 1,2,Direction.N,1,2)]
        [DataRow(Direction.E, 0,2,Direction.E,0,2)]
        [DataRow(Direction.S, -9,-2,Direction.S,-9,-2)]
        [DataRow(Direction.W, 5, 5, Direction.W, 5, 5)]
        [DataRow(null, null,null,Direction.None,0,0)]
        
        public void PlayerTest(Direction facing, int x, int y, Direction ExpectedFacing, int ExpectedX, int ExpectedY)
        {
            Player player = new(null, facing, x, y);
            Assert.AreEqual(ExpectedFacing, player.Facing);
            Assert.AreEqual(ExpectedX, player.Position.X);
            Assert.AreEqual(ExpectedY, player.Position.Y);
            Assert.AreEqual(ExpectedFacing, player.Facing);
        }

        [TestMethod()]
        [DataRow(0, Direction.N)]
        [DataRow(Math.PI/2, Direction.E)]
        [DataRow(Math.PI, Direction.S)]
        [DataRow(3*Math.PI/2, Direction.W)]
        [DataRow(Direction.None, Direction.None)]
        public void GetRotationTest(double expected, Direction direction)
        {
            Player player = new(null, direction);
            Assert.AreEqual(expected, player.GetRotation(), .01);
        }

        [TestMethod()]
        [DataRow(1,2, Direction.N)]
        [DataRow(1,1, Direction.E)]
        [DataRow(1,1, Direction.S)]
        [DataRow(2, 1, Direction.W)]
        public void MoveBackwardTest(int x, int y, Direction facing)
        {
            Block[,] blocks = new Block[,] {
            {Block.Solid,Block.Solid,Block.Solid,Block.Solid,Block.Solid },
            {Block.Solid,Block.Empty,Block.Empty,Block.Empty,Block.Solid},
            {Block.Solid,Block.Empty,Block.Solid,Block.Empty,Block.Solid},
            {Block.Solid,Block.Empty,Block.Empty,Block.Empty,Block.Solid},
            {Block.Solid,Block.Solid,Block.Solid,Block.Solid,Block.Solid} };
            Player p = new Player(blocks,facing);
            MapVector expected = new MapVector(x, y);
            p.MoveBackward();
            Assert.AreEqual(expected, p.Position);
        }

        [TestMethod()]
        [DataRow(1, 1, Direction.N)]
        [DataRow(2, 1, Direction.E)]
        [DataRow(1, 2, Direction.S)]
        [DataRow(1, 1, Direction.W)]
        [DataRow(1,1, Direction.None)]
        public void MoveForwardTest(int x, int y, Direction facing)
        {
            Block[,] blocks = new Block[,] {
            {Block.Solid,Block.Solid,Block.Solid,Block.Solid,Block.Solid },
            {Block.Solid,Block.Empty,Block.Empty,Block.Empty,Block.Solid},
            {Block.Solid,Block.Empty,Block.Solid,Block.Empty,Block.Solid},
            {Block.Solid,Block.Empty,Block.Empty,Block.Empty,Block.Solid},
            {Block.Solid,Block.Solid,Block.Solid,Block.Solid,Block.Solid} };
            Player p = new Player(blocks, facing);
            MapVector expected = new MapVector(x, y);
            p.MoveForward();
            Assert.AreEqual(expected, p.Position);
        }

        [TestMethod()]
        [DataRow(Direction.W, Direction.N)]
        [DataRow(Direction.N, Direction.E)]
        [DataRow(Direction.E, Direction.S)]
        [DataRow(Direction.S, Direction.W)]
        [DataRow(Direction.None, Direction.None)]
        public void TurnLeftTest(Direction Expected,Direction startDirection)
        {
            Player player = new(null, startDirection);
            player.TurnLeft();
            Assert.AreEqual(Expected, player.Facing);
        }

        [TestMethod()]
        [DataRow(Direction.E, Direction.N)]
        [DataRow(Direction.S, Direction.E)]
        [DataRow(Direction.W, Direction.S)]
        [DataRow(Direction.N, Direction.W)]
        [DataRow(Direction.None, Direction.None)]
        [DataRow(Direction.None, null)]
        public void TurnRightTest(Direction Expected, Direction startDirection)
        { 
            Player player = new(null, startDirection);
            player.TurnRight();
            Assert.AreEqual(Expected, player.Facing);
        }
    }
}