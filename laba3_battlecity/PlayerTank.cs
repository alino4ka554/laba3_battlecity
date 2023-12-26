using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3_battlecity
{
    [Serializable]
    public class PlayerTank : Tank // класс для танка игрока
    {
        public Bullet bullet { get; set; }

        public bool IsShooting { get; set; }

        public PlayerTank()
        {
            bullet = new Bullet();
        }

        public bool isNoBarrier(List<Tank> tanks) // проверяет, есть ли вражеский танк перед танком игрока
        {
            for (int i = 1; i < tanks.Count; i++)
            {
                if (tanks[0].NextRadius == (tanks[i].Radius) && !tanks[i].isDead)
                    return false;
            }
            return true;
        }

    }

}
