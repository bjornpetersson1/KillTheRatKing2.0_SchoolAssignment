using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_DungeonCrawler.GameFunctions;


//TODO det här behövs refaktoriseras
public abstract class RandomMap : LevelElement
{

    public static string GenerateMap()
    {
        Random random = new Random();
        char[,] generatedMap = new char[20, 60];

        //bygger spelplanen
        for (int i = 0; i < 20; i++)
        {
            for (global::System.Int32 j = 0; j < 60; j++)
            {
                generatedMap[i, j] = ' ';
            }
        }
        //startrutan
        int width = 9;
        int hight = 7;
        for (int i = 0; i < 7; i++)
        {
            for (global::System.Int32 j = 0; j < 9; j++)
            {
                if (i == 0 || i == 6) generatedMap[i, j] = '#';
                else if (j == 0 || j == 8) generatedMap[i, j] = '#';
                else if (i == 3 && j == 4) generatedMap[i, j] = '@';
            }
        }



        int numOfRooms = random.Next(2, 6);
        var centerCoOrs = new List<CenterOfRoom>();
        centerCoOrs.Add(new CenterOfRoom(3, 4));
        int roomsGenerated = 0;
        while (roomsGenerated != numOfRooms)
        {
            width = random.Next(8, 13);
            hight = random.Next(5, 11);
            int x = random.Next(0, 60 - width);
            int y = random.Next(0, 20 - hight);
            bool isCollideing = false;

            for (int i = 0; i < hight; i++)
            {
                for (global::System.Int32 j = 0; j < width; j++)
                {
                    if (generatedMap[i + y, j + x] == '#')
                    {
                        isCollideing = true;
                        continue;
                    }
                }
            }
            if (!isCollideing)
            {
                for (int i = 0; i < hight; i++)
                {
                    for (global::System.Int32 j = 0; j < width; j++)
                    {
                        if (i == 0 || i == hight - 1) generatedMap[i + y, j + x] = '#';
                        else if (j == 0 || j == width - 1) generatedMap[i + y, j + x] = '#';
                    }
                }
                centerCoOrs.Add(new CenterOfRoom { X = x + (width / 2), Y = y + (hight / 2) });
                roomsGenerated++;
            }
        }

        var startCoOr = centerCoOrs[0];
        centerCoOrs = centerCoOrs
            .OrderBy(p => Math.Abs(p.X - startCoOr.X) + Math.Abs(p.Y - startCoOr.Y))
            .ToList();
        for (int i = 0; i < centerCoOrs.Count - 1; i++)
        {
            var from = centerCoOrs[i];
            var to = centerCoOrs[i + 1];
            int fromX = from.X;
            int fromY = from.Y;
            fromY = fromY + (Math.Sign(to.Y - fromY)) * 2;
            fromX = fromX + (Math.Sign(to.X - fromX)) * 2;
            int distanceY = Math.Abs((to.Y - (Math.Sign(to.Y - fromY)) * 2) - fromY);
            int distanceX = Math.Abs((to.X - (Math.Sign(to.X - fromX)) * 2) - fromX);
            bool yFlag = false;
            for (global::System.Int32 j = 0; j < distanceY; j++)
            {
                if (j == 0 || j == distanceX - 1)
                {
                    generatedMap[fromY, fromX - 1] = '#';
                    generatedMap[fromY, fromX + 1] = '#';
                }
                else
                {
                    generatedMap[fromY, fromX - 1] = '#';
                    generatedMap[fromY, fromX + 1] = '#';
                    generatedMap[fromY - 1, fromX] = '#';
                    generatedMap[fromY + 1, fromX] = '#';
                }

                fromY += Math.Sign(to.Y - fromY);
                yFlag = true;
            }
            for (global::System.Int32 j = 0; j < distanceX; j++)
            {
                if ((j == 0 && yFlag == false) || (j == distanceX - 1 && yFlag == false))
                {
                    generatedMap[fromY - 1, fromX] = '#';
                    generatedMap[fromY + 1, fromX] = '#';
                }
                else
                {
                    generatedMap[fromY, fromX - 1] = '#';
                    generatedMap[fromY, fromX + 1] = '#';
                    generatedMap[fromY - 1, fromX] = '#';
                    generatedMap[fromY + 1, fromX] = '#';
                }

                fromX += Math.Sign(to.X - fromX);
            }
        }
        for (int i = 0; i < centerCoOrs.Count - 1; i++)
        {
            var from = centerCoOrs[i];
            var to = centerCoOrs[i + 1];
            int fromX = from.X;
            int fromY = from.Y;
            fromY = fromY + (Math.Sign(to.Y - fromY)) * 2;
            fromX = fromX + (Math.Sign(to.X - fromX)) * 2;
            int distanceY = Math.Abs((to.Y - (Math.Sign(to.Y - fromY)) * 2) - fromY);
            int distanceX = Math.Abs((to.X - (Math.Sign(to.X - fromX)) * 2) - fromX);
            for (global::System.Int32 j = 0; j < distanceY; j++)
            {
                generatedMap[fromY, fromX] = ' ';
                fromY += Math.Sign(to.Y - fromY);
            }
            for (global::System.Int32 j = 0; j < distanceX + 1; j++)
            {
                generatedMap[fromY, fromX] = ' ';
                fromX += Math.Sign(to.X - fromX);
            }
        }

        //generera fiender
        char[] enemies = new char[] { 'R', 'r', 's' };
        for (int i = 1; i < centerCoOrs.Count; i++)
        {
            int randomEnemy = random.Next(enemies.Length);
            if (generatedMap[centerCoOrs[i].Y + 1, centerCoOrs[i].X] == ' ')
                generatedMap[centerCoOrs[i].Y + 1, centerCoOrs[i].X] = enemies[randomEnemy];
            if (generatedMap[centerCoOrs[i].Y - 1, centerCoOrs[i].X] == ' ')
                generatedMap[centerCoOrs[i].Y - 1, centerCoOrs[i].X] = enemies[randomEnemy];
            if (generatedMap[centerCoOrs[i].Y, centerCoOrs[i].X + 1] == ' ')
                generatedMap[centerCoOrs[i].Y, centerCoOrs[i].X + 1] = enemies[randomEnemy];
            if (generatedMap[centerCoOrs[i].Y, centerCoOrs[i].X - 1] == ' ')
                generatedMap[centerCoOrs[i].Y, centerCoOrs[i].X - 1] = enemies[randomEnemy];
        }


        var mySB = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            for (global::System.Int32 j = 0; j < 60; j++)
            {
                mySB.Append(generatedMap[i, j]);
            }
            mySB.AppendLine();
        }
        File.WriteAllText("ProjectFiles\\GeneratedMap.txt", mySB.ToString());
        return "ProjectFiles\\GeneratedMap.txt";
    }

    public struct CenterOfRoom
    {
        public int Y;
        public int X;

        public CenterOfRoom(int y, int x)
        {
            this.Y = y;
            this.X = x;
        }
    }



}

