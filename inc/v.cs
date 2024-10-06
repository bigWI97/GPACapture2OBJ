using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPACapture2OBJ.inc
{
    internal class v
    {
        public float x = float.PositiveInfinity, y = float.PositiveInfinity, z = float.PositiveInfinity;

        public override string ToString()
        {
            return $"v {x} {y} {z}";
        }
    }
}
