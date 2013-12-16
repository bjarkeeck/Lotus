using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    public class IntRange
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public IntRange(int min, int max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
        public IntRange(int singleValue)
        {
            this.Minimum = singleValue;
            this.Maximum = singleValue;
        }
    }
    public class FloatRange
    {
        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public FloatRange(float min, float max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
        public FloatRange(float singleValue)
        {
            this.Minimum = singleValue;
            this.Maximum = singleValue;
        }
    }
}
