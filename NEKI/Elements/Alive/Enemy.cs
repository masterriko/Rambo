using NEKI.Elements.Objects;
using NEKI.Elements.UsefulStuff;
using NEKI.Elements.Algo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Data;
using NEKI;

namespace NEKI.Elements.Alive
{
    public class Enemy : Entity
    {
        public int VisionRadius = 30; 
        public int VisionAngle = 60;
        private Random rnd = new Random();
        public Enemy(Position position, int health, int damage) : base(ObjectType.Enemy, position, "E", 10000, 0, health, damage)
        {
            Position = position;
            Health = health;
            Damage = damage;
        }

        public override string MouseOver()
        {
            return "It's a soldier";
        }

        public override Bullet? Shoot()
        {

            Position newPosition = GetNewPosition(Direction);
            Moved();
            return new Bullet(newPosition, Damage, Direction, Damage, false);
        }
        public Bullet? Move(Tiles tiles, Player player, Dictionary<Position, Ammo> ammos)
        {
            if (PlayerInSight(player.Position)) 
            {
                List<Direction> dir = AStar.GetDirections(tiles, player.Position, Position);
                bool allSame;
                if (dir != null && dir.Count > 0)
                {
                    allSame = dir.All(d => d == dir.First());
                    if (AStar.EuclidDistance(Position, player.Position) > 10)
                    {

                        Position newPosition = GetNewPosition(dir.First());
                        if (tiles.GetObjectFromPosition(newPosition) == null)
                        {
                            Direction = dir.First();
                            tiles.RemoveObjectFromPosition(newPosition);
                            tiles.MoveObjectToPosition(this, Position, newPosition);
                            UpdatePosition(newPosition);
                        }
                        
                    }
                    else
                    {
                        if (allSame)
                        {
                            Direction = dir.First();
                            if (tiles.GetObjectFromPosition(GetNewPosition(Direction)) != null)
                            {
                                if (tiles.GetObjectFromPosition(GetNewPosition(Direction)).Type == ObjectType.Player)
                                {
                                    player.Health -= Damage;
                                    return null;
                                }
                            }
                            else
                            {
                                return Shoot();
                            }

                        }
                        else
                        {
                            Position newPosition = GetNewPosition(dir.First());
                            if (tiles.GetObjectFromPosition(newPosition) == null)
                            {
                                Direction = dir.First();
                                tiles.RemoveObjectFromPosition(newPosition);
                                tiles.MoveObjectToPosition(this, Position, newPosition);
                                UpdatePosition(newPosition);
                            }
                        }
                    }
                }
            }
            else
            {
                List<Direction> directions = new List<Direction>((Direction[])Enum.GetValues(typeof(Direction)));


                while (directions.Count > 0)
                {
                    // Pick a random direction
                    int index = rnd.Next(directions.Count);
                    Direction dir = directions[index];
                    Position newPosition = GetNewPosition(dir);

                    if (tiles.GetObjectFromPosition(newPosition) == null)
                    {
                        Direction = dir;
                        tiles.RemoveObjectFromPosition(newPosition);
                        tiles.MoveObjectToPosition(this, Position, newPosition);
                        UpdatePosition(newPosition);
                        break;
                    }
                    else
                    {
                        // Remove the chosen direction from the list
                        directions.RemoveAt(index);
                    }
                   
                }
            }
            return null;
        }

        private bool PlayerInSight(Position position)
        {
            double deltaX = position.X - Position.X;
            double deltaY = position.Y - Position.Y;
            double angleToPlayer = Math.Atan2(deltaY, deltaX) * (180 / Math.PI); 

            // Determine the center angle based on the robot's direction
            double centerAngle = Direction switch
            {
                Direction.Right => 0,
                Direction.Up => 90,
                Direction.Left => 180,
                Direction.Down => -90,
                _ => 0
            };

            // Normalize angles to be between -180 and 180
            double NormalizeAngle(double angle)
            {
                while (angle > 180) angle -= 360;
                while (angle < -180) angle += 360;
                return angle;
            }

            angleToPlayer = NormalizeAngle(angleToPlayer);
            centerAngle = NormalizeAngle(centerAngle);

            // Check if the player is within the robot's vision angle
            double halfVisionAngle = VisionAngle / 2.0;
            if (angleToPlayer >= centerAngle - halfVisionAngle && angleToPlayer <= centerAngle + halfVisionAngle)
            {
                // Check if the player is within the vision radius
                double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                if (distance <= VisionRadius)
                {
                    return true; // Player is within field of view
                }
            }

            return false; // Player is not within field of view
        }
    }
}
