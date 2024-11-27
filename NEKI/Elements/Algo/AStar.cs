using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.Algo
{
    using NEKI.Elements.Alive;
    using NEKI;
    using NEKI.Elements.UsefulStuff;
    using System;
    using System.Collections.Generic;

    class AStar
    {
        public static List<Direction> GetDirections(Tiles tiles, Position playerPosition, Position enemyPosition)
        {
            int[,] grid = Convert(tiles, playerPosition, enemyPosition);

            // Starting and goal points
            Node start = new Node(playerPosition.X, playerPosition.Y);
            Node goal = new Node(enemyPosition.X, enemyPosition.Y);

            List<Node> path = AStarAlgo(grid, start, goal);
            if (path != null)
            {
                return GiveDirections(path);
            }
            else
            {
                return null;
            }
        }

        static List<Direction> GiveDirections(List<Node> path)
        {
            List<Direction> directions = new List<Direction>();

            for (int i = 1; i < path.Count; i++) 
            {
                Node previous = path[i - 1];
                Node current = path[i];

                if (current.X == previous.X + 1 && current.Y == previous.Y)
                {
                    directions.Add(Direction.Up);
                }
                else if (current.X == previous.X - 1 && current.Y == previous.Y)
                {
                    directions.Add(Direction.Down);
                }
                else if (current.Y == previous.Y + 1 && current.X == previous.X)
                {
                    directions.Add(Direction.Left);
                }
                else if (current.Y == previous.Y - 1 && current.X == previous.X)
                {
                    directions.Add(Direction.Right);
                }
            }

            return directions;
        }

        public static double EuclidDistance(Position start, Position end)
        {
            int x = start.X - end.X;
            int y = start.Y - end.Y;
            return Math.Sqrt(x * x - y * y);
        }

        static int[,] Convert(Tiles tiles, Position playerPosition, Position enemyPosition)
        {
            int[,] map = new int[tiles.Height, tiles.Width];
            for(int i = 0;i<tiles.Width; i++)
            {
                for(int j = 0; j < tiles.Height; j++)
                {
                    if (tiles.Map[j, i] == null || (playerPosition.X == j && playerPosition.Y == i) || (enemyPosition.X == j && enemyPosition.Y == i))
                    {
                        map[j, i] = 0;
                    }
                    else
                    {
                        map[j, i] = 1;
                    }
                }
            }

            return map;
        }

        static List<Node> AStarAlgo(int[,] grid, Node start, Node goal)
        {
            // Open and closed sets
            List<Node> openSet = new List<Node> { start };
            HashSet<Node> closedSet = new HashSet<Node>();

            // Initialize g and f scores
            start.G = 0;
            start.F = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                // Get the node with the lowest F score
                Node current = GetLowestFScoreNode(openSet);

                if (current.Equals(goal))
                {
                    // Reconstruct and return the path
                    return ReconstructPath(current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                // Check all neighbors
                foreach (Node neighbor in GetNeighbors(grid, current))
                {
                    if (closedSet.Contains(neighbor) || grid[neighbor.X, neighbor.Y] == 1)
                        continue;

                    // Calculate tentative G score
                    int tentativeG = current.G + 1;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeG >= neighbor.G)
                    {
                        continue;
                    }

                    // Update neighbor values
                    neighbor.Parent = current;
                    neighbor.G = tentativeG;
                    neighbor.F = neighbor.G + Heuristic(neighbor, goal);
                }
            }

            // No path found
            return null;
        }

        static int Heuristic(Node a, Node b)
        {
            // Manhattan distance heuristic
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        static Node GetLowestFScoreNode(List<Node> nodes)
        {
            Node lowest = nodes[0];
            foreach (var node in nodes)
            {
                if (node.F < lowest.F)
                {
                    lowest = node;
                }
            }
            return lowest;
        }

        static List<Node> GetNeighbors(int[,] grid, Node node)
        {
            List<Node> neighbors = new List<Node>();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            // Possible directions (up, down, left, right)
            int[,] directions = {
            { -1, 0 }, { 1, 0 },
            { 0, -1 }, { 0, 1 }
        };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = node.X + directions[i, 0];
                int newY = node.Y + directions[i, 1];

                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                {
                    neighbors.Add(new Node(newX, newY));
                }
            }

            return neighbors;
        }

        static List<Node> ReconstructPath(Node current)
        {
            List<Node> path = new List<Node>();
            while (current != null)
            {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
    }

    class Node : IEquatable<Node>
    {
        public int X { get; }
        public int Y { get; }
        public int G { get; set; } // Cost from start to current node
        public int F { get; set; } // Total cost (G + heuristic)
        public Node Parent { get; set; } // To reconstruct the path

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && Equals(node);
        }

        public bool Equals(Node other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

}
