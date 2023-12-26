using Microsoft.VisualStudio.TestTools.UnitTesting;
using laba3_battlecity;
using System;
using System.IO;
using System.Collections.Generic;


namespace UnitTestProject1
{
    [TestClass]
    public class BulletTests
    {
        private Bullet bul;
        public void Setup()
        {
            bul = new Bullet();
        }
        [TestMethod]
        public void isVisibleTest() // тест метода isVisible
        {
            bul = new Bullet();
            bool result = false;
            Assert.AreEqual(result, bul.isVisible(bul.coordinates)); // проверка результата, при отрицательных координатах снаряда
            result = false;
            bul.coordinates = new Tuple<int, int>(250, 100);
            Assert.AreEqual(result, bul.isVisible(bul.coordinates));
        }
    }

    [TestClass]
    public class PlayerTankTests
    {
        private List<Tank> tanks;
        private PlayerTank tank;
        public void Setup()
        {
            tanks = new List<Tank>(); // создаем список танков
            tank = new PlayerTank(); // создаем танк
            tanks.Add(tank);
            tank = new PlayerTank();
            tanks.Add(tank);
        }
        [TestMethod]
        public void isNoBarrierTest() // тест метода isNoBarrier
        {
            tanks = new List<Tank>(); // создаем список танков
            tank = new PlayerTank(); // создаем танк
            tanks.Add(tank);
            tank = new PlayerTank();
            tanks.Add(tank);
            tanks[0].NextRadius = 100;
            tanks[1].Radius = 100;
            bool result = false;
            Assert.AreEqual(result, tank.isNoBarrier(tanks)); // проверяет результат, если танки сталкиваются
            result = true;
            tanks[1].Radius = 60;
            Assert.AreEqual(result, tank.isNoBarrier(tanks)); // проверяет результат, если танки не сталкиваются
        }
    }

    [TestClass]
    public class TankTests
    {
        private Tank tank;
        public void Setup()
        {
            tank = new Tank(); // создаем танк
        }
        [TestMethod]
        public void isValidMoveTest() // тест метода isValidMove
        {
            tank = new Tank(); // создаем танк
            Tuple<int, int> tankCoordinates = new Tuple<int, int>(1200, -1);
            bool result = false;
            Assert.AreEqual(result, tank.isValidMove(tankCoordinates)); // проверяем результат при неверных координатах
            tankCoordinates = new Tuple<int, int>(1000, 100);
            result = true;
            Assert.AreEqual(result, tank.isValidMove(tankCoordinates)); // проверяем результат при верных координатах
        }

        [TestMethod]
        public void GetDirectionTest() // тест метода GetDirection
        {
            tank = new Tank(); // создаем танк
            Tuple<int, int> result = new Tuple<int, int>(1, 0);
            tank.Mode = 1;
            Assert.AreEqual(result, tank.GetDirection()); // проверяем результат при движении танка влево
            result = new Tuple<int, int>(0, 1);
            tank.Mode = 4;
            Assert.AreEqual(result, tank.GetDirection()); // проверяем результат при движении танка вниз
        }
    }
}
