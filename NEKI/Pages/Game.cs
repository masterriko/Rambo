using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEKI.Elements;
using NEKI.Elements.Alive;
using NEKI.Elements.Levels;
using NEKI;
using NEKI.Elements.Objects;
using NEKI.Elements.UsefulStuff;

namespace NEKI.Pages
{
    public class Game
    {
        private int GAMEWIDTH;
        private int GAMEHEIGHT;
        private Level Level;
        private Map Map;
        private bool doNothing = false;
        private int centerX;
        private int centerY;

        public void StartNewGame()
        {
            centerX = Console.WindowWidth / 2;
            centerY = Console.WindowHeight / 2;
            WelcomePage.YourNickname(centerX, centerY);
            Scale scale = WelcomePage.Dificulty(centerX, centerY);
            SetGameSize(scale);
            Level level = new Level(scale, GAMEHEIGHT, GAMEWIDTH);
            level.GenerateGame();
            Level = level;
            Map = level.Map;
            Console.CursorVisible = false;
            while (true)
            {
                if (Map.GameOver()!)
                {
                    if (!doNothing)
                    {
                        Clear();
                        GameOverText();
                        doNothing = true;
                    }
                    else
                    {
                        ListenToEvents();
                    }
                }
                else
                {
                    Paint();
                    SomeInfo();
                    Map.MoveEnemies();
                    Console.SetCursorPosition(0, 0);
                    Update();
                    ListenToEvents();
                    doNothing = false;
                }

            }
        }

        private void SetGameSize(Scale scale)
        {
            Random rnd = new Random();
            switch (scale)
            {
                case Scale.Easy:
                    GAMEWIDTH = rnd.Next(20, 25);
                    GAMEHEIGHT = rnd.Next(10, 12);
                    break;
                case Scale.Medium:
                    GAMEWIDTH = rnd.Next(25, 35);
                    GAMEHEIGHT = rnd.Next(12, 15);
                    break;
                case Scale.Hard:
                    GAMEWIDTH = rnd.Next(35, 50);
                    GAMEHEIGHT = rnd.Next(15, 25);
                    break;
            }
        }

        public void ListenToEvents()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (Map.GameOver())
                {
                    if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        Level = new Level(Scale.Easy, GAMEHEIGHT, GAMEWIDTH);
                        Level.GenerateGame();
                        Map = Level.Map;
                        Clear();
                    }
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        Map.MovePlayer(Direction.Up);
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        Map.MovePlayer(Direction.Down);
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        Map.MovePlayer(Direction.Left);
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        Map.MovePlayer(Direction.Right);

                    }
                    else if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        Map.PlayerShoot();
                    }
                    else if (keyInfo.Key == ConsoleKey.R)
                    {
                        Map.Player.Reload();
                    }
                }
            }
        }

        public void SomeInfo()
        {
            int positionX = centerX + (int)GAMEWIDTH / 2;
            int positionY = centerY - (int)GAMEHEIGHT / 2;
            Console.SetCursorPosition(positionX + 2, positionY);
            Console.Write("| HEALTH:  " + Map.Player.Health.ToString().PadLeft(3, ' ') + "/100" + (Map.Player.Health <= 25 ? "| LOW HEALTH" : "            "));
            Console.SetCursorPosition(positionX + 2, positionY + 4);
            Console.Write("| GUN:     " + Map.Player.Gun.ToString().PadLeft(2, ' ') + "/10" + (Map.Player.Gun <= 0 ? "| RELOAD" : "        "));
            Console.SetCursorPosition(positionX + 2, positionY + 5);
            Console.WriteLine("| BULLETS:" + Map.Player.Bullets.ToString().PadLeft(3, ' ') + "/" + Map.Player.Inventory + (Map.Player.Bullets <= 0 ? "| NO MORE BULLETS" : "               "));
            Console.SetCursorPosition(positionX + 2, positionY + 2);
            Console.WriteLine("| STEPS:   " + Map.Player.Steps.ToString().PadLeft(2));
            Console.SetCursorPosition(centerX - 4, centerY + (int)GAMEHEIGHT / 2 + 1);
            Console.WriteLine("SCORE: " + Map.Player.Score + "   ");
            Console.SetCursorPosition(positionX + 2, centerY + (int)GAMEHEIGHT / 2 - 1);
            Console.WriteLine("| ENEMIES: " + Map.enemies.Count.ToString().PadLeft(2, ' ') + "/" + Map.NumberOfEnemies);
        }


        public void Paint()
        {

            for (int i = 0; i < GAMEHEIGHT; i++)
            {
                string line = "";
                for (int j = 0; j < GAMEWIDTH; j++)
                {
                    Console.SetCursorPosition(centerX - GAMEWIDTH / 2, centerY - GAMEHEIGHT / 2 + i);
                    line += Level.GetObjectName(new Position(j, i));
                }

                Console.WriteLine(line);
            }
        }

        public void Clear()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                string line = "";
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    line += " ";
                }
                Console.WriteLine(line);
            }
            Console.SetCursorPosition(0, 0);
        }

        public void GameOverText()
        {

            string[] gameOverArt = new string[]
            {
                " #####        #       ##     ##  #######     #####   #     #  ######  #### ",
                "#     #      # #      # #   # #  #          #     #  #     #  #      #    #",
                "#           #   #     #  # #  #  #          #     #  #     #  #      #    #",
                "#  ####    #######    #   #   #  #####      #     #  #     #  #####  ##### ",
                "#     #   #       #   #       #  #          #     #  #     #  #      #    #",
                "#     #  #         #  #       #  #          #     #   #   #   #      #    #",
                " #####  #           # #       #  #######     #####     ##     ###### #    #"
            };

            int positionX = Console.WindowWidth / 2 - gameOverArt[0].Length / 2;
            int positionY = Console.WindowHeight / 2 - gameOverArt.Length / 2;

            for (int i = 0; i < gameOverArt.Length; i++)
            {
                Console.SetCursorPosition(positionX, positionY + i);
                Console.WriteLine(gameOverArt[i]);
            }

            string pressSpace = "Press SPACE to start again";
            Console.SetCursorPosition(positionX + gameOverArt[0].Length / 2 - pressSpace.Length / 2, positionY + gameOverArt.Length + 1);
            Console.WriteLine(pressSpace);
            string higestScore = $"Your score:{Map.Player.Score.ToString().PadLeft(3)}   Highest Score: 1000";

            Console.SetCursorPosition(positionX + gameOverArt[0].Length / 2 - higestScore.Length / 2, positionY + gameOverArt.Length + 5);
            Console.WriteLine(higestScore);
        }

        public void Update()
        {
            Map.UpdateMap();
        }

        public void Stop()
        {

        }
    }
}
