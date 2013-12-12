using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotusEngine;

namespace Lotus
{
    class Test : Component
    {
        public override void Update()
        {
            transform.rotation += 50 * Time.DeltaTime;
        }
    }
}
