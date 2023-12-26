using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public abstract class GameItem // общий класс для игровых объектов
    {
        public int width { get; set; } // ширина объекта
        public int height { get; set; } // высота объекта
    }
}
