using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.Objects
{
    public abstract class GameObject : GameElement
    {
        public GameObject(ObjectType type, Position position, string name) : base(type, position, name){}
        public abstract string MouseOver();
        public abstract bool Interact();
    }
}
