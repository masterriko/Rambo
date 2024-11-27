using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.UsefulStuff
{
    public abstract class GameElement
    {
        public ObjectType Type { get; private set; }
        public Position Position { get; set; }
        public string Name { get; protected set; }
        public GameElement(ObjectType type, Position position, string name)
        {
            Type = type;
            Position = position;
            Name = name;
        }

        public Position GetNewPosition(Direction direction)
        {
            Position newPosition = new Position(Position.Y, Position.X);

            if (direction == Direction.Up)
            {
                newPosition.X -= 1;

            }
            else if (direction == Direction.Down)
            {
                newPosition.X += 1;

            }
            else if (direction == Direction.Left)
            {

                newPosition.Y -= 1;

            }
            else if (direction == Direction.Right)
            {

                newPosition.Y += 1;
            }

            return newPosition;
        }

        public void UpdatePosition(Position newPosition)
        {
            Position.X = newPosition.X;
            Position.Y = newPosition.Y;
        }
    }
}
