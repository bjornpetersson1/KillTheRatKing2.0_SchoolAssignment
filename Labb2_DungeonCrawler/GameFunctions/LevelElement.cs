using Labb2_DungeonCrawler;
using Labb2_DungeonCrawler.GameFunctions;



public abstract class LevelElement
{
    public int xCordinate { get; set; }
    public int yCordinate { get; set; }
    public char Symbol { get; set; }
    public ConsoleColor MyColor { get; set; }
    public string? Name { get; set; }
    public int TurnsPlayed { get; set; }
    public int XP { get; set; }
    public Dice? AttackDice { get; set; }
    public Dice? DefenceDice { get; set; }
    public int HP { get; set; }


    public virtual void PrintUnitInfo() { }
    public static void LevelChoice()
    {
        ConsoleKeyInfo userChoice;
        bool validChoiceFlag = false;
        Console.ForegroundColor = ConsoleColor.Green;
        do
        {
            userChoice = Console.ReadKey(true);
            switch (userChoice.Key)
            {
                case ConsoleKey.D1:
                    Console.SetCursorPosition(15, 16);
                    Console.Write("press [1] to play level 1");
                    LevelData.Load("ProjectFiles\\Level1.txt");
                    validChoiceFlag = true;
                    break;
                case ConsoleKey.D2:
                    Console.SetCursorPosition(15, 17);
                    Console.Write("press [2] to play level 2");
                    LevelData.Load("ProjectFiles\\Level2.txt");
                    validChoiceFlag = true;
                    break;
                case ConsoleKey.D3:
                    Console.SetCursorPosition(15, 18);
                    Console.Write("press [3] to play level 3");
                    LevelData.Load("ProjectFiles\\Level3.txt");
                    validChoiceFlag = true;
                    break;
                case ConsoleKey.D4:
                    Console.SetCursorPosition(15, 19);
                    Console.Write("press [4] to generate a random level");
                    LevelData.Load(RandomMap.GenerateMap());
                    validChoiceFlag = true;
                    break;
            }
        } while (!validChoiceFlag);
        Thread.Sleep(500);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(23, 10);
        switch(userChoice.Key)
        {
            case ConsoleKey.D1:
                Console.WriteLine("loading level 1...");
                Console.SetCursorPosition(15, 12);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("move around by using the arrow keys");
                break;
            case ConsoleKey.D2:
                Console.WriteLine("loading level 2...");
                Console.SetCursorPosition(15, 12);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("did you know that the ratking has 2 tails?");
                break;
            case ConsoleKey.D3:
                Console.WriteLine("loading level 3...");
                Console.SetCursorPosition(15, 12);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("did you know that you can shoot la[z]er?");
                break;
            case ConsoleKey.D4:
                Console.WriteLine("generating level...");
                Console.SetCursorPosition(15, 12);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("hopefully there are no holes in the wall and");
                Console.SetCursorPosition(18, 13);
                Console.Write("no indestructable rocks in the way");
                break;
        }
        Thread.Sleep(4000);
    }
    public void Draw()
    {
        Console.SetCursorPosition(xCordinate, yCordinate);
        Console.ForegroundColor = MyColor;
        Console.Write(Symbol);
    }
    public void Erase()
    {
        Console.SetCursorPosition(this.xCordinate, this.yCordinate);
        Console.Write(' ');
    }
    public double GetDistanceTo(Player player)
    {
        return Math.Sqrt(Math.Abs(Math.Pow(this.yCordinate - player.yCordinate, 2) + Math.Abs(Math.Pow(this.xCordinate - player.xCordinate, 2))));
    }
    public bool IsSpaceAvailable()
    {
        CoOrdinate targetSpace = new CoOrdinate(this);


        return LevelData.Elements != null &&
               !LevelData.Elements.Any(k => k != this && k.yCordinate == targetSpace.YCord && k.xCordinate == targetSpace.XCord);
    }
    public void CollideAndConcequences(Player player)
    {
        var collider = this.GetCollider();
        if (collider is not Wall && !(collider is Enemy && this is Enemy))
        {
            Console.SetCursorPosition(0, 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 1);
            PrintFightresult(Fight(collider), collider, player);
            if (collider.HP > 0) collider.PrintFightresult(collider.Fight(this), this, player);
            this.PrintUnitInfo();
            collider.PrintUnitInfo();
        }
    }
    public LevelElement? GetCollider()
    {
        CoOrdinate targetSpace = new CoOrdinate(this);
        if (LevelData.Elements == null)
            return null;
        return LevelData.Elements.FirstOrDefault(k => k != this && k.xCordinate == targetSpace.XCord && k.yCordinate == targetSpace.YCord);
    }
    public int Attack(LevelElement enemy)
    {
        int attack = this.AttackDice.Throw();
        int defence = enemy.DefenceDice.Throw();
        int result = attack - defence;

        if (result > 0)
        {
            enemy.HP -= (result);
            return result;
        }
        else return -1;
    }
    public int Fight(LevelElement enemy)
    {

        if (enemy != this && (enemy is Enemy || enemy is Player))
        {
            return this.Attack(enemy);
        }
        else return -1;
    }
    public void PrintFightresult(int fightreturn, LevelElement enemy, Player player)
    {
        if (enemy is TheKingsTail)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{player.Name} attacked the Kings tail and it had no effect. You can't damage the tail");
        }
        else if (enemy is Lazer)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} attacked the lazer and it had no effect. You can't damage the lazer");
        }
        else if (fightreturn != -1 && this is Lazer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{player.Name} used {this.Name} on {enemy.Name} with {this.AttackDice} attack and {enemy.Name} defended with {enemy.DefenceDice}. Attack was successfull and did {fightreturn} damage");
        }
        else if (fightreturn != -1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{this.Name} attacked {enemy.Name} with {this.AttackDice} and {enemy.Name} defended with {enemy.DefenceDice}. Attack was successfull and did {fightreturn} damage");
        }
        else if (this is Lazer)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{player.Name} used {this.Name} on {enemy.Name} with {this.AttackDice} attack and {enemy.Name} defended with {enemy.DefenceDice}. Attack failed and did no damage");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{this.Name} attacked {enemy.Name} with {this.AttackDice} and {enemy.Name} defended with {enemy.DefenceDice}. Attack failed and did no damage");
        }
    }



}