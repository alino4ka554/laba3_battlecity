using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public class Tank : GameItem // родительский класс для танков 
    {
        public int NextRadius { get; set; } // следующее положение танка
        public int Radius { get; set; } // положение танка
        public bool isDead { get; set; } // отслеживание, мертвый ли танк
        public Tuple<int, int> coordinates { get; set; } // отслеживает координаты танка

        public bool isStopped { get; set; } // отслеживает, остановлен ли танк
        public Bullet bullet { get; set; } // снаряд танка
        public int Mode { get; set; } // направление движения
        public int Speed { get; set; } // скорость танка
        public Tank() // конструктор
        {
            isDead = false;
            bullet = new Bullet();
            Speed = 2;
            Mode = 3;
            width = 60;
            height = 60;
           // coordinates = new Tuple<int, int>(0, 0);  // начальные координаты (100, 100)

        }

        public void MovementPlay() //движение танка
        {
           if (!isStopped && !isDead)
            {
                Speed = 1;
                if (Mode == 1)
                    coordinates = new Tuple<int, int>(coordinates.Item1, coordinates.Item2 + Speed);
                else if (Mode == 2)
                    coordinates = new Tuple<int, int>(coordinates.Item1, coordinates.Item2 - Speed);
                else if (Mode == 3)
                    coordinates = new Tuple<int, int>(coordinates.Item1 - Speed, coordinates.Item2);
                else if (Mode == 4)
                    coordinates = new Tuple<int, int>(coordinates.Item1 + Speed, coordinates.Item2);
            }
        }


        public Tuple<int, int> GetDirection() // определяет направление движения снаряда
        {
            Tuple<int, int> dir;
            if (Mode == 1)
                dir = new Tuple<int, int>(1, 0);
            else if (Mode == 2)
                dir = new Tuple<int, int>(-1, 0);
            else if (Mode == 3)
                dir = new Tuple<int, int>(0, -1);
            else
                dir = new Tuple<int, int>(0, 1);
            return dir;
        }

        public bool isValidMove(Tuple<int, int> coord) // функция для того, чтобы танк не выел за рамки формы
        {
            if (coord.Item1 > 1120 || coord.Item2 > 700 || coord.Item1 < 1 || coord.Item2 < 1)
            {
                return false;
            }
            else
                return true;
        }
    }

}
