using NEKI;
using NEKI.Elements.Objects;
using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace NEKI.Elements.Alive
{
    public class Player : Entity
    {
        public readonly int Inventory = 100;
        public int Score { get; set; }

        public int Gun { get; private set; }

        public Player(Position position, int steps, int damage, int health=100, int score=0, int bullets = 100, int gun=10) : base(ObjectType.Player, position, "P", bullets, steps, health, damage)
        {
            Score = score;
            Bullets = bullets;
            Gun = gun;
            Health = health;
            Steps = steps;
        }
        public override string MouseOver()
        {
            return "Hello. It's me";
        }
        public override Bullet? Shoot()
        {
            if (Gun > 0)
            {
                Position newPosition = GetNewPosition(Direction);
                Gun -= 1;
                return new Bullet(newPosition, Damage, Direction, 10, true);
            }

            return null;
        }

        public void PickupAmmo(int bullets)
        {
            if (Bullets + bullets <= Inventory)
            {
                Bullets += bullets;
            }
        }

        public void AddScore() { 
            Score += 1;
        }

        public string Reload()
        {
            if (Bullets <= 0)
            {
                return "No Ammo";
            }
            else if (Gun == 10)
            {
                return "Gun full";
            }
            else if (Gun + Bullets < 10)
            {
                Gun += Bullets;
                Bullets = 0;
            }
            else
            {
                Bullets -= 10 - Gun;
                Gun = 10;
            }
            return "Reloaded";
        }

        public void Move(Tiles map, Direction direction, Dictionary<Position, Ammo> ammos)
        {
            Position newPosition = GetNewPosition(direction);
            Direction = direction;

            if (map.GetObjectFromPosition(newPosition) == null)
            {
                map.MoveObjectToPosition(this, Position, newPosition);
                UpdatePosition(newPosition);
                Steps -= 1;
            }
            else if (map.GetObjectFromPosition(newPosition).Type == ObjectType.Enemy)
            {
                Health = -1;

            }
            else if (map.GetObjectFromPosition(newPosition).Type == ObjectType.Ammo)
            {
                Ammo ammo = ammos[newPosition];
                Steps += ammo.Steps;
                PickupAmmo(ammo.Amount);
                ammos.Remove(newPosition);
                map.RemoveObjectFromPosition(newPosition);
                map.MoveObjectToPosition(this, Position, newPosition);
                Position.X = newPosition.X;
                Position.Y = newPosition.Y;
            }
        }
    }
}
