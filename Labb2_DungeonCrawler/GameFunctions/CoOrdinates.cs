using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_DungeonCrawler;

public struct CoOrdinate
{
    public int XCord { get; set; }
    public int YCord { get; set; }
    public CoOrdinate(LevelElement element)
    {
        XCord = element.xCordinate;
        YCord = element.yCordinate;
    }
}
