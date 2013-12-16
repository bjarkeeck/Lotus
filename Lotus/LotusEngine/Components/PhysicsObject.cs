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
            //foreach (BitmapCollider bmp in GameObject.FindAllComponents<BitmapCollider>().Where(x => x.Collide))
            //{
            //    bmp.CheckCollision(gameObject.GetComponent<RectangleCollider>(), ref Velocity);
            //}


            foreach (RectangleCollider rect in GameObject.FindAllComponents<RectangleCollider>(x => x.Collide))
            {

            }

        }

    }
}
