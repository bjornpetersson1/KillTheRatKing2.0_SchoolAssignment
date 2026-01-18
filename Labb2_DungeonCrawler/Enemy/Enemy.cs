using Labb2_DungeonCrawler.GameFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_DungeonCrawler;

public abstract class Enemy : LevelElement
{
    public abstract void Update(Player player);
    public override void PrintUnitInfo()
    {
        Console.SetCursorPosition(0, 3);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, 3);
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        if (HP > 0) Console.WriteLine($"|{Symbol}: {Name} | HP: {HP} | Attack: {AttackDice} | Defence: {DefenceDice} |");
        else Console.WriteLine($"{Name} is now killed and dead");
    }
}
