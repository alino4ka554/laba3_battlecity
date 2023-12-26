using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public class Bullet : GameItem // класс для снаряда танка
    {
        public int reload; // переменная, отслеживающая перезарядку
        public Tuple<int, int> coordinates { get; set; } // координаты снаряда
        private Tuple<int, int> direction; // направление снаряда
        int speed { get; set; } // скорость снаряда
        public Bullet() // конструктор
        {
            reload = 0;
            speed = 6;
            width = 23;
            height = 23;
            direction = new Tuple<int, int>(0, 0);
            coordinates = new Tuple<int, int>(-50, -50);
        }
        public void Reloading() // отслеживание перезарядки
        {
            reload++;
        }
        public bool isVisible(Tuple<int, int> coord) // проверяет, вышел ли снаряд за пределы окна
        {
            if (coord.Item1 > 19 || coord.Item2 > 59 || coord.Item1 < 1 || coord.Item2 < 1)
            {
                return false;
            }
            else
                return true;
        }
        public void Destroy() // разрушение снаряда при столкновении или выходе за рамки
        {
            Reloading();
            coordinates = new Tuple<int, int>(-50, -50);
            direction = new Tuple<int, int>(0, 0);
        }
        public void Movement()
        {
            int X = coordinates.Item1;
            int Y = coordinates.Item2;

            // Проверяем, не выходит ли снаряд за границы игрового поля
            if (isVisible(new Tuple<int, int>(X + direction.Item1 * speed, Y + direction.Item2 * speed)))
            {
                coordinates = new Tuple<int, int>(X + direction.Item1 * speed, Y + direction.Item2 * speed);
            }
            else
            {
                // Если снаряд выходит за границы, разрушаем его
                Destroy();
            }
        }

        public void Spawn(Tuple<int, int> dir, Tuple<int, int> p1) // появление снаряда
        {
            direction = dir;
            coordinates = new Tuple<int, int>(p1.Item1, p1.Item2);
        }
    }
}
