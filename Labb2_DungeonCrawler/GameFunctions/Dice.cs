using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_DungeonCrawler;

public class Dice
{
    public int SidesPerDice { get; set; }
    public int NumberOfDice { get; set; }
    public int Modifier { get; set; }
    private Random random;
    public Dice(int sidesPerDice, int numberOfDice, int modifier)
    {
        if (sidesPerDice < 0) throw new ArgumentOutOfRangeException("Value can't be less than 0");
        if (numberOfDice < 0) throw new ArgumentOutOfRangeException("Value can't be less than 0");
        SidesPerDice = sidesPerDice;
        NumberOfDice = numberOfDice;
        Modifier = modifier;
        random = new Random();
    }
    public int Throw()
    {
        int result = 0;
        for (int i = 0; i < NumberOfDice; i++)
        {
            result += random.Next(SidesPerDice);   
        }
        return result + Modifier;
    }
    public override string ToString()
    {
        if(Modifier < 0) return $"{NumberOfDice}d{SidesPerDice}{Modifier}";
        else return $"{NumberOfDice}d{SidesPerDice}+{Modifier}";
    }
}
