using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.Objects
{
    public class Bullet : GameObject
    {
        public int Damage { get; private set; }

        public int Speed { get; private set; }
        public Direction Direction { get; private set; }

        public bool Players { get; private set; }
        public Bullet(Position position, int damage, Direction direction, int speed, bool players) : base(ObjectType.Bullet, position, "-") { 
            Position = position;
            Damage = damage;
            Direction = direction;
            Speed = speed;
            Players = players;
            if (Direction == Direction.Up || Direction == Direction.Down)
            {
                Name = "|";
            }
            else
            {
                Name = "-";
            }
        }

        public Bullet(Bullet other) : base(ObjectType.Bullet, new Position(other.Position.Y, other.Position.X), "-") 
        {
            Position = new Position(other.Position.Y, other.Position.X); 
            Damage = other.Damage;  
            Direction = other.Direction; 
            Speed = other.Speed; 
        }

        public override string MouseOver()
        {
            return "Impossible to go through";
        }

        public override bool Interact()
        {
            return false;
        }
    }
}
