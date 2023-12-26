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

    [Serializable]
    public class SaveData
    {
        public PlayerTank PlayerTank { get; set; }
        public List<ComputerTank> ComputerTanks { get; set; }
        public Base Baza { get; set; }
        public BlockWall Wall { get; set; }

    }


    public class Game
    {
        public PlayerTank playerTank { get; set; } //игровой танк
        private List<ComputerTank> computerTanks; //список вражеских танков
        private Base baza; //база
        private BlockWall wall; //стена
        private const int rows = 20;
        private const int cols = 60;

        public Game() //конструктор
        {
            playerTank = new PlayerTank();
            playerTank.coordinates = new Tuple<int, int>(15, 25); //координаты игрового танка
            baza = new Base();
            wall = new BlockWall();
            computerTanks = new List<ComputerTank>
            {
            new ComputerTank { coordinates = new Tuple<int, int>(5, 10) }, //координаты вражеских танков
            new ComputerTank { coordinates = new Tuple<int, int>(2, 50) },
            new ComputerTank { coordinates = new Tuple<int, int>(7, 40)}
            };
            if (File.Exists("game.dat"))
            {
                Load();
            }
            else LoadFirst();
        }

        public void Start() //начало игры
        {
            Console.Clear();

            while (true)
            {
                foreach (var tank in computerTanks)
                {
                    if ((tank.coordinates.Item1 < rows - 1 && tank.Mode != 2) || (tank.coordinates.Item2 == 1 && tank.Mode != 3))
                    {
                        tank.Mode = 1;
                        if (tank.coordinates.Item2 == 1)
                            tank.coordinates = new Tuple<int, int>(tank.coordinates.Item1 + 1, tank.coordinates.Item2);
                    }
                    if (tank.coordinates.Item2 == cols - 1 && tank.Mode != 3)
                    {
                        tank.Mode = 2;
                        tank.coordinates = new Tuple<int, int>(tank.coordinates.Item1 + 1, tank.coordinates.Item2);
                    }
                    if (tank.coordinates.Item1 == rows - 1)
                    {
                        tank.Mode = 3;
                        tank.coordinates = new Tuple<int, int>(tank.coordinates.Item1 - 2, tank.coordinates.Item2);
                    }
                    if (tank.coordinates.Item2 == cols - 1 && tank.Mode == 3)
                    {
                        tank.Mode = 4;
                        tank.coordinates = new Tuple<int, int>(tank.coordinates.Item1 - 1, tank.coordinates.Item2);
                    }
                    if ((tank.coordinates.Item1 >= 16 && tank.coordinates.Item1 <= 19) && (tank.coordinates.Item2 >= 27 && tank.coordinates.Item2 <= 33))
                    {
                        if (tank.Mode != 4) tank.Mode++;
                        else tank.Mode--;
                    }
                    tank.Movement(); //движение вражеских танков
                    tank.Shoot(); 
                }
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    HandleInput(key, playerTank);
                    if (playerTank.IsShooting == false)
                    {
                        playerTank.MovementPlay(); //движение игрового танка
                    }
                    if (playerTank.Mode == 0) LoadFirst();
                }
                playerTank.bullet.Movement(); 
                DrawMap();
                Thread.Sleep(100);
                Console.Clear(); 
            }
        }

        public void HandleInput(ConsoleKeyInfo key, PlayerTank playerTank) //считывание данных с клавиатуры для движения игрового танка
        {
            int nextX = playerTank.coordinates.Item1;
            int nextY = playerTank.coordinates.Item2;
            switch (key.Key)
            {
                case ConsoleKey.W:
                    nextX--;
                    playerTank.Mode = 3;
                    playerTank.IsShooting = false;
                    break;
                case ConsoleKey.S:
                    nextX++;
                    playerTank.Mode = 4;
                    playerTank.IsShooting = false;
                    break;
                case ConsoleKey.A:
                    nextY--;
                    playerTank.Mode = 2;
                    playerTank.IsShooting = false;
                    break;
                case ConsoleKey.D:
                    nextY++;
                    playerTank.Mode = 1;
                    playerTank.IsShooting = false;
                    break;
                case ConsoleKey.Spacebar:
                    playerTank.bullet.Spawn(playerTank.GetDirection(), playerTank.coordinates);
                    playerTank.IsShooting = true;
                    break;
            }
            if (nextX >= 0 && nextX < rows && nextY >= 0 && nextY < cols)
            {
                if ((playerTank.coordinates.Item1 >= 16 && playerTank.coordinates.Item1 <= 19) && (playerTank.coordinates.Item2 >= 27 && playerTank.coordinates.Item2 <= 33))
                {
                    playerTank.Mode = 0;
                }
            }
            else playerTank.Mode = 0;
        }

        public void DrawMap() //прорисовка карты
        {
            Console.SetCursorPosition(0, 0);
            Save();
            char[,] gameBoard = new char[rows, cols];

            // Заполняем границы
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == 0 || i == rows - 1)
                    {
                        gameBoard[i, j] = '-';
                    }
                    else if (j == 0 || j == cols - 1)
                    {
                        gameBoard[i, j] = '|';
                    }
                    else
                    {
                        gameBoard[i, j] = ' ';
                    }
                }
            }

            //Рисуем игрока
            if (playerTank != null && (playerTank.coordinates.Item1 < rows && playerTank.coordinates.Item2 < cols))
            {
                gameBoard[playerTank.coordinates.Item1, playerTank.coordinates.Item2] = 'T';
            }

            //Рисуем снаряд игрока
            if (playerTank != null && playerTank.bullet != null && playerTank.bullet.isVisible(playerTank.bullet.coordinates))
            {
                gameBoard[playerTank.bullet.coordinates.Item1, playerTank.bullet.coordinates.Item2] = '*';
            }


            if (baza != null && baza.isDestroyed == false) //рисуем базу
            {
                gameBoard[baza.width, baza.height] = '$';
            }

            if (wall != null)
            {
                gameBoard[wall.width, wall.height] = '+';
                gameBoard[wall.width - 1, wall.height] = '+';
                gameBoard[wall.width - 1, wall.height + 1] = '+';
                gameBoard[wall.width - 1, wall.height + 2] = '+';
                gameBoard[wall.width - 1, wall.height + 3] = '+';
                gameBoard[wall.width - 1, wall.height + 4] = '+';
                gameBoard[wall.width, wall.height + 4] = '+';
            }

            //Рисуем вражеские танки
            foreach (var tank in computerTanks)
            {
                if (tank != null && (tank.coordinates.Item1 < rows && tank.coordinates.Item2 < cols))
                {
                    gameBoard[tank.coordinates.Item1, tank.coordinates.Item2] = 'W';
                }

                if (tank != null && tank.bullet.isVisible(tank.bullet.coordinates))
                {
                    gameBoard[tank.bullet.coordinates.Item1, tank.bullet.coordinates.Item2] = '*'; // символ для снаряда
                }
                if (tank.IsCollision(playerTank, tank.coordinates))
                {
                    LoadFirst();
                }
                if (tank.coordinates.Item1 == playerTank.bullet.coordinates.Item1 || tank.coordinates.Item2 == playerTank.bullet.coordinates.Item2)
                {
                    tank.isDead = true;
                    tank.Restore();
                }
                if (playerTank.coordinates.Item1 == tank.bullet.coordinates.Item1 || playerTank.coordinates.Item2 == tank.bullet.coordinates.Item2)
                {
                    LoadFirst();
                }
            }

            // Выводим игровое поле в консоль
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(gameBoard[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void SaveFirst() //сохранение первоначальной версии игры
        {
            SaveData data = new SaveData()
            {
                PlayerTank = playerTank,
                ComputerTanks = computerTanks,
                Baza = baza,
                Wall = wall,
            };
            BinaryFormatter formatter = new BinaryFormatter();
            // Сериализация объекта и сохранение в файл
            using (FileStream fs = new FileStream("gamefirst.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, data);
            }
        }
        public void LoadFirst() //загрузка первоначальной версии игры
        {
            // Десериализация объекта из файла
            using (FileStream fs = new FileStream("gamefirst.dat", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                SaveData saveData = (SaveData)formatter.Deserialize(fs);

                // Присвоение сохраненных значений текущим объектам
                playerTank = saveData.PlayerTank;
                computerTanks = saveData.ComputerTanks;
                baza = saveData.Baza;
                wall = saveData.Wall;
            }
            DrawMap();
            Console.Clear();
        }

        public void Save() //сохранение игры
        {
            SaveData data = new SaveData()
            {
                PlayerTank = playerTank,
                ComputerTanks = computerTanks,
                Baza = baza,
                Wall = wall,
            };
            BinaryFormatter formatter = new BinaryFormatter();
            // Сериализация объекта и сохранение в файл
            using (FileStream fs = new FileStream("game.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, data);
            }
        }
        public void Load() //загрузка сохраненной игры
        {
            // Десериализация объекта из файла
            using (FileStream fs = new FileStream("game.dat", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                SaveData saveData = (SaveData)formatter.Deserialize(fs);

                // Присвоение сохраненных значений текущим объектам
                playerTank = saveData.PlayerTank;
                computerTanks = saveData.ComputerTanks;
                baza = saveData.Baza;
                wall = saveData.Wall;
            }
            DrawMap();
            Console.Clear();
        }
    }
}

