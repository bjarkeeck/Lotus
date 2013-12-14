using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    public class Range<T> where T : IComparable<T>
    {
        public T Minimum { get; set; }
        public T Maximum { get; set; }

        public Range(T min, T max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
        public Range(T singleValue)
        {
            this.Minimum = singleValue;
            this.Maximum = singleValue;
        }
    }
}
