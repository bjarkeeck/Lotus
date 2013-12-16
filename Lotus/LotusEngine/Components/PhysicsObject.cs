using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine.Components
{

    /// <summary>
    /// Kræver en 
    /// </summary>
    public class PhysicsObject : Component
    {

        private RectangleCollider rectangle { get { return this.gameObject.GetComponent<RectangleCollider>(); } }

        public Vector2 Velocity;
         
        public override void Update()
        {
            foreach (RectangleCollider rect in GameObject.FindAllComponents<RectangleCollider>(x => x.Collide))
            {

            }
        }

    }
}
