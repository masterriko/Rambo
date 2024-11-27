using NEKI.Elements.Objects;
using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.Alive
{
    public abstract class Entity : GameElement
    {
        public int Bullets {  get; set; }
        public int Steps { get; set; }
        public int Health { get; set; }

        public int Damage { get; set; }

        public int Speed { get; set; }

        public Direction Direction { get; set; }


        public Entity(ObjectType type, Position position, string name, int bullets, int steps, int health, int damage, Direction direction = Direction.Right, int speed = 20) : base(type, position, name)
        {
            Bullets = bullets;
            Steps = steps;
            Health = health;
            Direction = direction;
            Damage = damage;
            Speed = speed;
            
        }
        public abstract string MouseOver();
        public abstract Bullet? Shoot();

        public bool Shot(int damage)
        {
            Health -= damage;
            return Health <= 0;
        }

        public void Moved()
        {
            Steps--;
        }

        public void StartMoving(int steps)
        {
            Steps = steps;
        }
    }
}
