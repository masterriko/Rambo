using NEKI.Elements.Levels;
using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Pages
{
    public class WelcomePage
    {
        public static string YourNickname(int positionX, int positionY)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(positionX - 10, positionY);
            Console.Write("Type your username: ");
            string userName = Console.ReadLine();
            while (true)
            {
                if (userName == null || userName == String.Empty)
                {
                    Console.SetCursorPosition(positionX - 5, positionY + 2);
                    Console.WriteLine("Invalid username");
                    Console.SetCursorPosition(positionX - 10, positionY);
                    Console.Write("Type your username: ");
                    userName = Console.ReadLine();
                    
                }
                else
                {
                    Console.Clear();
                    return userName;
                }
            }
        }

        public static Scale Dificulty(int positionX, int positionY)
        {
            Console.CursorVisible = false;
            string difficultyLine = "EASY   MEDIUM   HARD";
            string line = "---                 ";
            Console.SetCursorPosition(positionX - 8, positionY);
            Console.WriteLine(difficultyLine);
            Console.SetCursorPosition(positionX - 8, positionY + 1);
            Console.WriteLine(line);

            void ClearLine()
            {
                Console.SetCursorPosition(positionX - 8, positionY + 1);
                Console.WriteLine("                    ");
                Console.SetCursorPosition(positionX - 8, positionY + 1);
            }

            void UnderLine(int i)
            {
                ClearLine();
                switch (i)
                {
                    case 0:
                        Console.WriteLine("---                 ");
                        break;
                    case 1:
                        Console.WriteLine("       ------       ");
                        break;
                    case 2:
                        Console.WriteLine("                ----");
                        break;

                }
            }

            int i = 0;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        if (i > 0)
                        {
                            i--;
                            UnderLine(i);
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        if (i < 2)
                        {
                            i++;
                            UnderLine(i);
                        }

                    }else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        switch (i)
                        {
                            case 0:
                                return Scale.Easy;
                            case 1:
                                return Scale.Medium;
                            case 2:
                                return Scale.Hard;
                        }
                    }
                }
            }
        }
    }
}
