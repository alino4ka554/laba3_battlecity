using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public class BlockWall : GameItem // класс для неразрушаемой стены
    {
        public BlockWall() // конструктор
        {
            width = 18;
            height = 28;
            
        }

    }
}
