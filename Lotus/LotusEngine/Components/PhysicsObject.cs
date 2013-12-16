using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine.Components
{

    /// <summary>
    /// Vigtigt at denne component bliver kaldt til sidst.
    /// </summary>
    public class PhysicsObject : Component
    {
        public Vector2 Velocity;

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            transform.position += Velocity; 
        }
    }
}
