using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_DungeonCrawler.Menu;

public static class MenuHelper
{
    public static int ShowMenu(string title, List<MenuOption> options, bool allowEscape = true)
    {
        int index = 0;
        ConsoleKey key;
        index = options.FindIndex(o => o.IsEnabled);
        if (index == -1) return -1;
        Console.Clear();


        int startWriteTop = (Console.WindowHeight - options.Count) / 2;
        int startWriteLeft = (Console.WindowWidth - title.Length) / 2;

        Console.SetCursorPosition(startWriteLeft, startWriteTop);
        ColorFlashWrite(title);
       
        Console.WriteLine();

        int menuTop = Console.CursorTop;
        int menuLeft = startWriteLeft;

        do
        {

            for (int i = 0; i < options.Count; i++)
            {
                Console.SetCursorPosition(menuLeft, menuTop + i);
                
                if (i == index)
                {
                    if (options[i].IsEnabled)
                    {
                        ColorFlashWrite($"> {options[i].Text.PadRight(startWriteLeft)}    ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"  {options[i].Text.PadRight(startWriteLeft)}");
                    }
                }
                else
                {
                    if (options[i].IsEnabled)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write($"  {options[i].Text.PadRight(startWriteLeft)}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"  {options[i].Text.PadRight(startWriteLeft)}");
                    }
                }
            }

            Console.ResetColor();

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow)
            {
                do
                {
                    index = (index == 0) ? options.Count -1 : index -1;
                } while (!options[index].IsEnabled);
            }
            else if (key == ConsoleKey.DownArrow)
            {
                do
                {
                    index = (index == options.Count -1) ? 0 : index +1;
                } while (!options[index].IsEnabled);
            }

            if (allowEscape && key == ConsoleKey.Escape)
                return -1;

        } while (key != ConsoleKey.Enter);

        return index;
    }
    public static bool ConfirmYesNo(string question)
    {
        ConsoleKey key;
        Console.Clear();
        Console.WriteLine($"{question} [Y/N]");

        do
        {
            key = Console.ReadKey(true).Key;
        } while (key != ConsoleKey.Y && key != ConsoleKey.N);

        return key == ConsoleKey.Y;
    }
    private static void ColorFlashWrite(string input)
    {
        ConsoleColor[] colors =
        {
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkGray,
            ConsoleColor.White
        };
        int inputTop = Console.CursorTop;
        int inputLeft = Console.CursorLeft;

        for (int i = 0; i < colors.Length; i++)
        {
            Console.SetCursorPosition(inputLeft, inputTop);
            Console.ForegroundColor = colors[i];
            Thread.Sleep(100);
            Console.Write(input);
        }
    }
}
