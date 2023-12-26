using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace laba3_battlecity
{
        internal class Program
        {
            public static void Main(string[] args)
            {
            
                try
                {
                    Game game = new Game();
                    //game.SaveFirst();
                    game.DrawMap();
                    game.Start();
                }
                catch (Exception e) 
                {
                    Game game = new Game();
                    game.LoadFirst();
                }
        }
        }
    
}
    

