using NEKI.Elements.Alive;
using NEKI.Elements.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEKI.Elements.UsefulStuff;

namespace NEKI
{
    public class Tiles
    {
        private Random rand = new Random();

        private int corridorWidth = 3;
        public GameElement?[,] Map { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Tiles(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public void InitializeMap()
        {
            GameElement[,] map = new GameElement[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Wall wall = new Wall(new Position(i, j));
                    map[i, j] = wall; // Default to walls
                }
            }
            Map = map;

            GenerateMap(1, 1);
            FillBorder();
        }

        private void GenerateMap(int x, int y)
        {
            // Define the directions (up, down, left, right)
            int[] directions = { 0, 1, 2, 3 }; // 0: Up, 1: Down, 2: Left, 3: Right
            Shuffle(directions); // Randomize the directions to ensure a random maze

            // Try each direction
            foreach (var direction in directions)
            {
                int newX = x, newY = y;

                switch (direction)
                {
                    case 0: newX -= corridorWidth; break; // Up
                    case 1: newX += corridorWidth; break; // Down
                    case 2: newY -= corridorWidth; break; // Left
                    case 3: newY += corridorWidth; break; // Right
                }

                // Check if the new position is within bounds and is a wall ('W')
                if (Check(newX, newY))
                {
                    // Carve a path to the new position
                    CreateCorridor(x, y, newX, newY); // Carve out a larger path

                    // Recursively generate the maze from the new position
                    GenerateMap(newX, newY);
                }
            }
        }

        private void CreateCorridor(int startX, int startY, int endX, int endY)
        {
            // Determine the direction of the corridor
            int dx = endX - startX;
            int dy = endY - startY;

            // Carve the corridor by iterating over the direction
            if (dx != 0) // Vertical movement (Up/Down)
            {
                int step = dx > 0 ? 1 : -1;
                for (int i = 0; i < Math.Abs(dx); i++)
                {
                    for (int j = -corridorWidth / 2; j <= corridorWidth / 2; j++) // Wider corridor
                    {
                        if (startX + i * step >= 0 && startX + i * step < Map.GetLength(0) &&
                            startY + j >= 0 && startY + j < Map.GetLength(1))
                        {
                            //Nothing nothing = new Nothing(new Position(i, j));
                            Map[startX + i * step, startY + j] = null; // Carve path
                        }
                    }
                }
            }
            else if (dy != 0) // Horizontal movement (Left/Right)
            {
                int step = dy > 0 ? 1 : -1;
                for (int i = 0; i < Math.Abs(dy); i++)
                {
                    for (int j = -corridorWidth / 2; j <= corridorWidth / 2; j++) // Wider corridor
                    {
                        if (startX + j >= 0 && startX + j < Map.GetLength(0) &&
                            startY + i * step >= 0 && startY + i * step < Map.GetLength(1))
                        {
                            //Nothing nothing = new Nothing(new Position(i, j));
                            Map[startX + j, startY + i * step] = null; // Carve path
                        }
                    }
                }
            }
        }

        private bool Check(int x, int y)
        {

            return x > 0 && y > 0 && x < Map.GetLength(0) && y < Map.GetLength(1) && Map[x, y] != null && Map[x, y].Type.Equals(ObjectType.Wall);

        }

        private void Shuffle(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = rand.Next(i, array.Length);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        private void FillBorder()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        Wall wall = new Wall(new Position(i, j));
                        Map[i, j] = wall;
                    }
                    if (i == Height - 1 || j == Width - 1)
                    {
                        Wall wall = new Wall(new Position(i, j));
                        Map[i, j] = wall;
                    }
                }
            }
        }

        public GameElement? GetObjectFromPosition(Position position)
        {
            return Map[position.Y, position.X];
        }

        public void AddNewObjectToPosition(GameElement obj)
        {
            Map[obj.Position.Y, obj.Position.X] = obj;
        }

        public void MoveObjectToPosition(GameElement obj, Position oldPosition, Position newPosition)
        {
            Map[oldPosition.Y, oldPosition.X] = null;
            Map[newPosition.Y, newPosition.X] = obj;
        }

        public void RemoveObjectFromPosition(Position position)
        {
            Map[position.Y, position.X] = null;
        }

        public string GetObjectName(Position position)
        {
            GameElement? gameElement = GetObjectFromPosition(position);
            if (gameElement == null)
            {
                return " ";
            }
            else
            {
                return gameElement.Name;
            }
        }
        public bool IsWalkable(int x, int y)
        {
            return Map[y, x] == null;
        }
    }
}
