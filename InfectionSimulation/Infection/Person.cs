using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace InfectionSimulation
{
    class Person : GameObject
    {
        public bool Infected { get; set; }

        public override void UpdateOn(World world)
        {
            IEnumerable<Person> near = world.ObjectsAt(Position).Cast<Person>();
            if (Infected)
            {
                Color = Color.Red;
                foreach (Person p in near)
                {
                    p.Infected = true;
                }
            }
            else
            {
                Color = Color.Blue;
            }

            Forward(world.Random(1, 2));
            Turn(world.Random(-25, 25));
        }//AA: Para mi este está bastante bien.
        
    }
}
