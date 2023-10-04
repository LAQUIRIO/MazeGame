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
    public class MapVectorTests
    {
        [TestMethod()]
        [DataRow(1,2)]
        [DataRow(-4,5)]
        public void MapVectorTest(int x, int y)
        {
            MapVector vector = new MapVector(x, y);
            Assert.AreEqual(x, vector.X);
            Assert.AreEqual(y, vector.Y);
        }

        [TestMethod()]
        [DataRow(1,2,true)]
        [DataRow(-4,5,false)]
        [DataRow(1,-5,false)]
        [DataRow(0,0,true)]
        public void MapVectorTest1(int x, int y, bool expected)
        {
            MapVector vector = new MapVector(x,y);
            Assert.AreEqual(expected, vector.IsValid);
        }

        [TestMethod()]
        [DataRow(1,2,5,6,true)]
        [DataRow(-4,5,8,9,false)]
        [DataRow(0,0,-1,9,false)]
        [DataRow(0,0,0,0,false)]
        public void InsideBoundaryTest(int x,int y, int H, int W, bool expected)
        {
            MapVector mapVector = new MapVector(x,y);
            Assert.AreEqual(expected, mapVector.InsideBoundary(H,W));
        }

        [TestMethod()]
        [DataRow(3,4,5)]
        [DataRow(7,24,25)]
        public void MagnitudeTest(int x,int y,int expected)
        {
            MapVector mapVector = new MapVector(x, y);
            Assert.AreEqual(expected, mapVector.Magnitude());
        }
        
        [TestMethod()]
        [DataRow(1,2,2,2,4)]
        [DataRow(-4,5,-2,8,-10)]
        public void multiplyTest(int x,int y,int scalar,int ExpectedX,int ExpectedY)
        {
            MapVector vector = new MapVector(x, y);
            MapVector expected = new MapVector(ExpectedX, ExpectedY);
            Assert.AreEqual(expected, vector * scalar);
        }

        [TestMethod()]
        [DataRow(1,2)]
        [DataRow(-4,5)]
        public void EqualsTest(int x,int y)
        {
            MapVector mapVector = new MapVector(x, y);
            MapVector mapVector2 = new MapVector(x, y);
            Assert.IsTrue(mapVector.Equals(mapVector2));
        }

        [TestMethod()]
        [DataRow(1,2)]
        [DataRow(-4,5)]
        public void GetHashCodeTest(int x,int y)
        {
            MapVector mapVector = new MapVector(x, y);
            MapVector mapVector2 = new MapVector(x, y);
            MapVector mapVector3 = new MapVector(0, 0);
            Assert.AreEqual(mapVector2.GetHashCode(), mapVector.GetHashCode(),.01);
            Assert.AreNotEqual(mapVector3, mapVector.GetHashCode());
        }
    }
}