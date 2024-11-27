using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.Objects
{
    public class Wall : GameObject
    {
        public Wall(Position position) : base(ObjectType.Wall, position, "W") { }

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
