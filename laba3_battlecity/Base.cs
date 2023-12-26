using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public class Base : GameItem // класс для базы
    {
        public bool isDestroyed { get; set; } // индикатор, показывающий, уничтожена ли база
        public Base() // конструктор
        {
            isDestroyed = false;
            width = 18;
            height = 30;
        }
    }
}
