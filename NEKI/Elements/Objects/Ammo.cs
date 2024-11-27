using NEKI.Elements.UsefulStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.Objects
{
    public class Ammo : GameObject
    {
        public int Amount { get; private set; }
        public int Steps { get; private set; }
        public Ammo(int amount, int steps, Position position) : base(ObjectType.Ammo, position, "*") { 

            Amount = amount;
            Steps = steps;

        }

        public override string MouseOver()
        {
            return "Give me that";
        }

        public override bool Interact()
        {
            return true;
        }
    }
}
