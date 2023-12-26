using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public class ComputerTank : Tank // класс для вражеских танков
    {
        private int delta = 0; // отслеживает время, после которого надо сменить направление движения        
        public int restoreTime { get; set; } // время возраждения танка
        public bool isStopped { get; set; } // отслеживает, остановлен ли танк
        public int restoreCoord { get; set; } // координаты для появления танка

        public Bullet bullet { get; set; }

        public ComputerTank()
        {
            bullet = new Bullet();
        }

        public void Shoot() //выстрел
        {
            if (bullet.reload > 20) // условие для перезарядки
            {
                // Рандомное решение о выстреле (например, с вероятностью 10%)
                if (new Random().Next(0, 10) == 0)
                {
                    bullet.Spawn(GetDirection(), coordinates);
                }

                bullet.Reloading();
            }

            // Проверка видимости снаряда и перемещение
            if (bullet.isVisible(bullet.coordinates))
            {
                bullet.Movement();
            }
            else
            {
                bullet.Destroy();
            }
        }

    private void Rotate() // поворот танка в определнный промежуток времени
        {
            Random rand = new Random();
            if (delta > 300)
            {
                Mode = rand.Next(1, 5);
                delta = 0;
            }
        }

        

        public void Movement() //движение танков
        {
            if (!isStopped && !isDead)
            {
                Speed = 1;
                if (Mode == 1)
                    coordinates = new Tuple<int, int>(coordinates.Item1, coordinates.Item2 + Speed);
                else if (Mode == 2)
                    coordinates = new Tuple<int, int>(coordinates.Item1, coordinates.Item2 - Speed);
                else if (Mode == 3)
                    coordinates = new Tuple<int, int>(coordinates.Item1, coordinates.Item2 + Speed);
                else if (Mode == 4)
                    coordinates = new Tuple<int, int>(coordinates.Item1, coordinates.Item2 - Speed);
            }
        }
        public bool IsCollision(PlayerTank otherTank, Tuple<int, int> newCoordinates) //проверка на столкновение
        {
            int thisTankLeft = coordinates.Item1;
            int thisTankTop = coordinates.Item2;

            int otherTankLeft = otherTank.coordinates.Item1;
            int otherTankTop = otherTank.coordinates.Item2;

            // Проверка на столкновение по осям X и Y
            bool collisionX = thisTankLeft == otherTankLeft;
            bool collisionY = otherTankTop == thisTankTop;

            // Если есть пересечение по обеим осям, значит, танки столкнулись
            return collisionX && collisionY;
        }

        public void Restore() // возраждения танков
        {
            if (!isDead)
            {
                restoreTime = 0;
            }
            else
            {
                restoreTime++;
                if (restoreTime > 1)
                {
                    coordinates = new Tuple<int, int>(5, 5);
                    isDead = false;
                }
            }
        }

    }
}
