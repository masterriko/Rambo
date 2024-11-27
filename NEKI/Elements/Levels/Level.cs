using NEKI.Elements.Alive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEKI.Elements.UsefulStuff;
using NEKI;

namespace NEKI.Elements.Levels
{
    public enum Scale
    {
        Easy,
        Medium,
        Hard
    }
    public class Level
    {
        public Map Map { get; set; }
        public Scale Difficulty { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }
        public Level(Scale difficulty, int height, int width)
        {

            Difficulty = difficulty;
            Width = width;
            Height = height;

        }
        public void GenerateGame()
        {
            Map map = null;
            if (Difficulty == Scale.Easy)
            {
                map = new Map(Width, Height, 10, 20, 30, 4, 5, 50);
            }
            else if (Difficulty == Scale.Medium)
            {
                map = new Map(Width, Height, 20, 15, 50, 10, 10, 20);
            }
            else if (Difficulty == Scale.Hard)
            {
                map = new Map(Width, Height, 25, 10, 100, 20, 5, 10);
            }
            Map = map;
        }

        public string GetObjectName(Position position)
        {
            return Map.Tiles.GetObjectName(position);
        }
    }
}
